using System;
using System.Collections;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystemApi = UnityEngine.InputSystem.InputSystem;

namespace ProjectLucid.Offline
{
    // The desktop controller backend (owner decision, #454): Unity Input System gamepads on macOS,
    // Windows and Linux. Like the original ApplePluginControllerProvider, the device and name lists
    // stay parallel and drop a controller when it disconnects. BaseActiveBindings passes a name's
    // index to the binding as its joystick index, and the controller sources index the device list
    // with it, so both always name the same device.
    //
    // The original polling compares only the count, which misses a controller replaced between two
    // polls. This provider runs its own polling at the configured rate instead and notifies once per
    // poll that follows any connect or disconnect, so a replacement refreshes the bindings too.
    //
    // Each connect gets a new connection number, also when Input System revives the same Gamepad
    // object, so a view over the list can tell a reconnect from a controller that stayed connected.
    public sealed class InputSystemControllerProvider : BaseControllerProvider
    {
        private readonly float m_pollingRateInSeconds;
        private readonly List<Gamepad> m_gamepads = new List<Gamepad>();
        private readonly List<string> m_names = new List<string>();
        private readonly List<int> m_connections = new List<int>();
        private int m_connected;
        private int m_changes;
        private int m_notifiedChanges;
        private bool m_listening;
        private Coroutine m_pollingCoroutine;
        private IInputSystemControllerView m_view;

        public InputSystemControllerProvider(float controllerConnectionPollingRateInSeconds)
            : base(controllerConnectionPollingRateInSeconds)
        {
            m_pollingRateInSeconds = controllerConnectionPollingRateInSeconds;
        }

        public IReadOnlyList<Gamepad> Gamepads => m_gamepads;

        // The connection number of each gamepad, parallel to Gamepads.
        public IReadOnlyList<int> Connections => m_connections;

        // Counts every connect, disconnect and stop, so views over the device list know when to rebuild.
        public int Changes => m_changes;

        public override IReadOnlyList<string> GetControllerNames() => m_names;

        // The view over these gamepads (InputSystemGCControllers, in the Apple input package). Like the
        // original ApplePluginControllerProvider's own controller list there is one per provider, and
        // the provider releases it when it stops listening.
        public TView View<TView>(Func<InputSystemControllerProvider, TView> create) where TView : class, IInputSystemControllerView
        {
            if (m_view == null) m_view = create(this);
            return (TView)m_view;
        }

        // The base polling is not started: this provider's polling replaces it.
        public override void Initialise()
        {
            StartListening();
            m_pollingCoroutine = Hardlight.Utils.CoroutineUtils.RunCoroutine(ConnectionPolling());
        }

        public override void Shutdown()
        {
            Hardlight.Utils.CoroutineUtils.StopUtilCoroutine(ref m_pollingCoroutine);
            StopListening();
            base.Shutdown();
        }

        // Adds the gamepads already connected, then follows device changes.
        public void StartListening()
        {
            if (m_listening) return;
            m_listening = true;
            foreach (InputDevice device in InputSystemApi.devices)
                Add(device);
            InputSystemApi.onDeviceChange += OnDeviceChange;
        }

        // Like the original ApplePluginControllerProvider.Shutdown, both lists empty without a
        // notification; the change count moves so nothing built over them stays current, and the view
        // releases what it registered.
        public void StopListening()
        {
            if (!m_listening) return;
            m_listening = false;
            InputSystemApi.onDeviceChange -= OnDeviceChange;
            m_gamepads.Clear();
            m_names.Clear();
            m_connections.Clear();
            m_notifiedChanges = ++m_changes;
            m_view?.Release();
        }

        // One poll: notify if any connect or disconnect happened since the last notification.
        public bool NotifyIfChanged()
        {
            if (m_changes == m_notifiedChanges) return false;
            m_notifiedChanges = m_changes;
            OnControllerConnectionUpdate.Invoke();
            return true;
        }

        private IEnumerator ConnectionPolling()
        {
            WaitForSeconds wait = new WaitForSeconds(m_pollingRateInSeconds);
            while (true)
            {
                NotifyIfChanged();
                yield return wait;
            }
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                case InputDeviceChange.Reconnected:
                    Add(device);
                    break;
                case InputDeviceChange.Removed:
                case InputDeviceChange.Disconnected:
                    Remove(device);
                    break;
            }
        }

        private void Add(InputDevice device)
        {
            if (!(device is Gamepad gamepad) || m_gamepads.Contains(gamepad)) return;
            m_gamepads.Add(gamepad);
            m_names.Add(ControllerName(gamepad));
            m_connections.Add(++m_connected);
            ++m_changes;
        }

        private void Remove(InputDevice device)
        {
            int index = device is Gamepad gamepad ? m_gamepads.IndexOf(gamepad) : -1;
            if (index < 0) return;
            m_gamepads.RemoveAt(index);
            m_names.RemoveAt(index);
            m_connections.RemoveAt(index);
            ++m_changes;
        }

        // The authored binding identifiers are substrings of the vendor names Game Controller reports
        // ("Xbox", "DUALSHOCK", "Wireless Controller"), and the last identifier a name contains picks
        // the binding's input type and so its glyph set. DualShock 4 and DualSense pads report their
        // macOS names and resolve to the PlayStation set, as on macOS. Xbox pads report Input System's
        // own name, "Xbox Controller": the macOS name "Xbox Wireless Controller" would end on
        // "Wireless Controller" and show PlayStation glyphs. Any other gamepad keeps its own name and
        // takes the binding's fallback, as an unrecognised controller did on macOS.
        public static string ControllerName(Gamepad gamepad)
        {
            if (InputSystemApi.IsFirstLayoutBasedOnSecond(gamepad.layout, "XInputController"))
                return "Xbox Controller";
            if (InputSystemApi.IsFirstLayoutBasedOnSecond(gamepad.layout, "DualSenseGamepadHID"))
                return "DualSense Wireless Controller";
            if (InputSystemApi.IsFirstLayoutBasedOnSecond(gamepad.layout, "DualShockGamepad"))
                return "DUALSHOCK 4 Wireless Controller";
            return gamepad.displayName ?? gamepad.name;
        }
    }

    // A view over an InputSystemControllerProvider's gamepads that the provider releases when it stops.
    public interface IInputSystemControllerView
    {
        void Release();
    }
}

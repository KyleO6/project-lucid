using System.Collections.Generic;
using Apple.GameController.Controller;
using Hardlight;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace ProjectLucid.Offline
{
    // Presents the Input System gamepads of InputSystemControllerProvider as Game Controller
    // controllers, so the original Apple source, bindings and processors run unchanged (H-080). Each
    // connection keeps one GCController, so its button-press history survives a refresh caused by
    // another device; a reconnect, even of the same Gamepad object, gets a new GCController with
    // fresh history, as the original service constructs one per connection. The list is rebuilt in
    // the provider's order, so index i is the device the provider named at i.
    //
    // There is one view per provider. It registers each of its controllers' handles so that
    // GCController.Poll, through GCControllerService.PollController, reads the gamepad here instead of
    // the Game Controller framework (H-081), and removes only its own registrations, when a controller
    // disconnects and when the provider stops.
    public sealed class InputSystemGCControllers : IControllerProvider<GCController>, IInputSystemControllerView
    {
        private const string UniqueIdPrefix = "InputSystem:";
        private static readonly Dictionary<string, Gamepad> s_gamepadsByUniqueId = new Dictionary<string, Gamepad>();
        private static int s_views;

        private readonly InputSystemControllerProvider m_provider;
        private readonly string m_uniqueIdPrefix;
        private readonly List<GCController> m_controllers = new List<GCController>();
        private readonly Dictionary<int, GCController> m_controllersByConnection = new Dictionary<int, GCController>();
        private int m_builtChanges = -1;

        private InputSystemGCControllers(InputSystemControllerProvider provider)
        {
            m_provider = provider;
            m_uniqueIdPrefix = UniqueIdPrefix + ++s_views + ":";
        }

        // The handles registered for polling across every view: none once every provider has stopped.
        public static int RegisteredHandles => s_gamepadsByUniqueId.Count;

        // The original cast, then the Input System provider's view; any other provider stays null, as the cast left it.
        public static IControllerProvider<GCController> For(IBaseControllerProvider baseControllerProvider)
        {
            if (baseControllerProvider is IControllerProvider<GCController> controllerProvider)
                return controllerProvider;
            if (baseControllerProvider is InputSystemControllerProvider inputSystemProvider)
                return inputSystemProvider.View(provider => new InputSystemGCControllers(provider));
            return null;
        }

        public IReadOnlyList<string> GetControllerNames() => m_provider.GetControllerNames();

        public FastAction OnControllerConnectionUpdate
        {
            get => m_provider.OnControllerConnectionUpdate;
            set => m_provider.OnControllerConnectionUpdate = value;
        }

        public IReadOnlyList<GCController> GetControllers()
        {
            if (m_builtChanges != m_provider.Changes)
                Rebuild();
            return m_controllers;
        }

        // The provider stopped: every controller disconnects and its registration goes.
        public void Release()
        {
            foreach (GCController controller in m_controllersByConnection.Values)
                Disconnect(controller);
            m_controllersByConnection.Clear();
            m_controllers.Clear();
            m_builtChanges = m_provider.Changes;
        }

        private void Rebuild()
        {
            m_builtChanges = m_provider.Changes;
            IReadOnlyList<Gamepad> gamepads = m_provider.Gamepads;
            IReadOnlyList<string> names = m_provider.GetControllerNames();
            IReadOnlyList<int> connections = m_provider.Connections;
            var connected = new Dictionary<int, GCController>();
            m_controllers.Clear();
            for (int i = 0; i < gamepads.Count; ++i)
            {
                if (!m_controllersByConnection.TryGetValue(connections[i], out GCController controller))
                {
                    controller = new GCController(Handle(m_uniqueIdPrefix + connections[i], names[i]));
                    s_gamepadsByUniqueId.Add(controller.Handle.UniqueId, gamepads[i]);
                }
                connected.Add(connections[i], controller);
                m_controllers.Add(controller);
            }
            foreach (KeyValuePair<int, GCController> pair in m_controllersByConnection)
                if (!connected.ContainsKey(pair.Key))
                    Disconnect(pair.Value);
            m_controllersByConnection.Clear();
            foreach (KeyValuePair<int, GCController> pair in connected)
                m_controllersByConnection.Add(pair.Key, pair.Value);
        }

        private static void Disconnect(GCController controller)
        {
            controller.IsConnected = false;
            s_gamepadsByUniqueId.Remove(controller.Handle.UniqueId);
        }

        private static GCControllerHandle Handle(string uniqueId, string name)
        {
            return new GCControllerHandle
            {
                UniqueId = uniqueId,
                ProductCategory = ProductCategory(name),
                VendorName = name,
                IsAttachedToDevice = false,
            };
        }

        // The Game Controller product categories GCControllerHandle.GetControllerType recognises.
        private static string ProductCategory(string name)
        {
            switch (name)
            {
                case "Xbox Controller": return "Xbox One";
                case "DUALSHOCK 4 Wireless Controller": return "DualShock 4";
                case "DualSense Wireless Controller": return "DualSense";
                default: return "Gamepad";
            }
        }

        // H-081: the gamepad's current state in Game Controller's layout. Triggers are the back
        // shoulders and View/Share is Options, as Game Controller reports them. Each D-pad axis is
        // read from its two buttons, so a diagonal is a full -1 or 1 on both axes, as Game Controller
        // reports it; Input System's D-pad vector is normalised. A gamepad that is no longer
        // connected reads as None, as a disconnected Game Controller controller does.
        public static GCControllerInputState Poll(GCControllerHandle handle)
        {
            if (handle.UniqueId == null || !s_gamepadsByUniqueId.TryGetValue(handle.UniqueId, out Gamepad gamepad) || !gamepad.added)
                return GCControllerInputState.None;
            return new GCControllerInputState
            {
                ButtonMenu = gamepad.startButton.ReadValue(),
                ButtonOptions = gamepad.selectButton.ReadValue(),
                ButtonA = gamepad.buttonSouth.ReadValue(),
                ButtonB = gamepad.buttonEast.ReadValue(),
                ButtonY = gamepad.buttonNorth.ReadValue(),
                ButtonX = gamepad.buttonWest.ReadValue(),
                ShoulderRightFront = gamepad.rightShoulder.ReadValue(),
                ShoulderRightBack = gamepad.rightTrigger.ReadValue(),
                ShoulderLeftFront = gamepad.leftShoulder.ReadValue(),
                ShoulderLeftBack = gamepad.leftTrigger.ReadValue(),
                DpadHorizontal = gamepad.dpad.right.ReadValue() - gamepad.dpad.left.ReadValue(),
                DpadVertical = gamepad.dpad.up.ReadValue() - gamepad.dpad.down.ReadValue(),
                ThumbstickLeftHorizontal = gamepad.leftStick.x.ReadValue(),
                ThumbstickLeftVertical = gamepad.leftStick.y.ReadValue(),
                ThumbstickLeftButton = gamepad.leftStickButton.ReadValue(),
                ThumbstickRightHorizontal = gamepad.rightStick.x.ReadValue(),
                ThumbstickRightVertical = gamepad.rightStick.y.ReadValue(),
                ThumbstickRightButton = gamepad.rightStickButton.ReadValue(),
                TouchpadButton = gamepad.TryGetChildControl<ButtonControl>("touchpadButton")?.ReadValue() ?? 0f,
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using Apple.GameController.Controller;
using Hardlight;
using NUnit.Framework;
using ProjectLucid.Offline;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystemApi = UnityEngine.InputSystem.InputSystem;
using Object = UnityEngine.Object;

namespace ProjectLucid.Editor.Tests
{
    // The Input System controller backend (#454) and its Game Controller view (H-080..H-082). The
    // fixture isolates the Input System, so only the gamepads a test adds exist; no coroutine,
    // Game Controller framework or hardware device is used. Tests that press controls use the
    // DualShockGamepad layout: the fixture cannot write the DualShock 4 HID report.
    public sealed class InputSystemControllerProviderTests : InputTestFixture
    {
        private const BindingFlags AnyInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static InputSystemControllerProvider Listening(out List<int> notifications)
        {
            var provider = (InputSystemControllerProvider)PortableControllerSelection.Create(3f);
            var calls = new List<int> { 0 };
            notifications = calls;
            provider.OnControllerConnectionUpdate += () => ++calls[0];
            provider.StartListening();
            return provider;
        }

        // Poll once, as the provider's polling does at its configured rate.
        private static int Poll(InputSystemControllerProvider provider, List<int> notifications)
        {
            int before = notifications[0];
            provider.NotifyIfChanged();
            return notifications[0] - before;
        }

        [Test]
        public void NotifiesOncePerPollAfterConnectDisconnectReplacementAndSwap()
        {
            Gamepad xbox = (Gamepad)InputSystemApi.AddDevice("XInputController");
            InputSystemControllerProvider provider = Listening(out List<int> notifications);
            try
            {
                Assert.That(provider.GetControllerNames(), Is.EqualTo(new[] { "Xbox Controller" }), "a gamepad connected before listening");
                Assert.That(Poll(provider, notifications), Is.EqualTo(1), "initial connection");
                Assert.That(Poll(provider, notifications), Is.EqualTo(0), "unchanged");

                InputSystemApi.AddDevice<Keyboard>();
                Assert.That(Poll(provider, notifications), Is.EqualTo(0), "a keyboard is not a controller");

                // Equal-length replacement between two polls: the original count-only polling never reports it.
                Gamepad dualShock = (Gamepad)InputSystemApi.AddDevice("DualShock4GamepadHID");
                InputSystemApi.RemoveDevice(xbox);
                Assert.That(provider.GetControllerNames(), Is.EqualTo(new[] { "DUALSHOCK 4 Wireless Controller" }));
                Assert.That(provider.Gamepads, Is.EqualTo(new[] { dualShock }));
                Assert.That(Poll(provider, notifications), Is.EqualTo(1), "replacement in the same slot");

                InputSystemApi.AddDevice(xbox);
                Assert.That(provider.GetControllerNames(), Is.EqualTo(new[] { "DUALSHOCK 4 Wireless Controller", "Xbox Controller" }));
                Assert.That(Poll(provider, notifications), Is.EqualTo(1), "reconnect");

                // Swap the two slots between polls: the count never changes, the order does.
                InputSystemApi.RemoveDevice(dualShock);
                InputSystemApi.AddDevice(dualShock);
                Assert.That(provider.Gamepads, Is.EqualTo(new[] { xbox, dualShock }));
                Assert.That(Poll(provider, notifications), Is.EqualTo(1), "swap");

                InputSystemApi.RemoveDevice(xbox);
                InputSystemApi.RemoveDevice(dualShock);
                Assert.That(provider.GetControllerNames(), Is.Empty);
                Assert.That(Poll(provider, notifications), Is.EqualTo(1), "disconnect of every controller");
                Assert.That(Poll(provider, notifications), Is.EqualTo(0));
            }
            finally { provider.StopListening(); }

            InputSystemApi.AddDevice(xbox);
            Assert.That(provider.GetControllerNames(), Is.Empty, "a stopped provider no longer follows devices");
            Assert.That(Poll(provider, notifications), Is.EqualTo(0));
        }

        [Test]
        public void NamesLetTheAuthoredIdentifiersPickEachGlyphSet()
        {
            // The last authored identifier a name contains sets the binding's input type
            // (BaseInputBinding.Setup); these are the shipped identifiers in their authored order.
            var identifiers = new[]
            {
                ("Remote", InputType.Remote), ("DUALSHOCK", InputType.PS4Controller), ("DualSense", InputType.PS5Controller),
                ("Nimbus", InputType.MfiController), ("HORIPAD", InputType.MfiController), ("Xbox", InputType.XboneController),
                ("GV161", InputType.MfiController), ("GV160", InputType.MfiController), ("GV157", InputType.MfiController),
                ("GV150", InputType.MfiController), ("GV140", InputType.MfiController), ("Xbox One", InputType.XboneController),
                ("MFi", InputType.MfiController), ("Riot", InputType.MfiController), ("Wireless Controller", InputType.PS4Controller),
                ("Wired Controller", InputType.PS4Controller), ("Wired Controller", InputType.PS4Controller),
            };
            var expected = new Dictionary<string, (string, InputType)>
            {
                { "XInputController", ("Xbox Controller", InputType.XboneController) },
                { "DualShock4GamepadHID", ("DUALSHOCK 4 Wireless Controller", InputType.PS4Controller) },
                { "DualSenseGamepadHID", ("DualSense Wireless Controller", InputType.PS4Controller) },
            };
            foreach (KeyValuePair<string, (string, InputType)> pair in expected)
            {
                var gamepad = (Gamepad)InputSystemApi.AddDevice(pair.Key);
                string name = InputSystemControllerProvider.ControllerName(gamepad);
                InputType type = InputType.Unsupported;
                foreach ((string identifier, InputType identifierType) in identifiers)
                    if (name.Contains(identifier)) type = identifierType;
                Assert.That((name, type), Is.EqualTo(pair.Value), pair.Key);
            }
            Gamepad generic = InputSystemApi.AddDevice<Gamepad>();
            Assert.That(InputSystemControllerProvider.ControllerName(generic), Is.EqualTo(generic.displayName ?? generic.name));
        }

        // Two gamepads through a real controller monitor: each binding, its input and its glyph stay with the
        // same device when the first disconnects, when a different pad replaces it, and when the first pad's
        // own Gamepad object reconnects. The glyphs come from the monitor's original lookup (0x0600002b),
        // which asks the source for a Game Controller symbol and, with none (H-082), the authored glyph map.
        [Test]
        public void BindingInputAndGlyphFollowEachGamepadThroughDisconnectReplacementAndReconnect()
        {
            Gamepad xbox = (Gamepad)InputSystemApi.AddDevice("XInputController");
            Gamepad dualShock = (Gamepad)InputSystemApi.AddDevice("DualShockGamepad");
            WithMonitor(rig =>
            {
                IApplePluginControllerInputKeySource keys = rig.Source.KeySource;

                Press(dualShock.buttonSouth);
                rig.Source.Update();
                ApplePluginControllerInputBinding xboxBinding = rig.CurrentAt(0);
                ApplePluginControllerInputBinding dualShockBinding = rig.CurrentAt(1);
                Assert.That(xboxBinding.BindingInputType, Is.EqualTo(InputType.XboneController));
                Assert.That(dualShockBinding.BindingInputType, Is.EqualTo(InputType.PS4Controller));
                Assert.That(keys.GetKey(GCControllerInputName.ButtonSouth, xboxBinding.JoystickIndex), Is.False);
                Assert.That(keys.GetKey(GCControllerInputName.ButtonSouth, dualShockBinding.JoystickIndex), Is.True);
                Assert.That(rig.Glyph(xboxBinding), Is.SameAs(rig.XboxSouth));
                Assert.That(rig.Glyph(dualShockBinding), Is.SameAs(rig.PlayStationSouth));

                InputSystemApi.RemoveDevice(xbox);
                Assert.That(rig.Provider.NotifyIfChanged(), Is.True);
                rig.Source.Update();
                Assert.That(rig.Bindings.CurrentInputBindings.Count, Is.EqualTo(1));
                ApplePluginControllerInputBinding remaining = rig.CurrentAt(0);
                Assert.That(remaining.BindingInputType, Is.EqualTo(InputType.PS4Controller), "the refreshed binding is the DualShock's");
                Assert.That(keys.GetKey(GCControllerInputName.ButtonSouth, remaining.JoystickIndex), Is.True, "and reads the DualShock");
                Assert.That(keys.GetKey(GCControllerInputName.ButtonSouth, 1), Is.False, "the vacated index reads nothing");
                Assert.That(rig.Glyph(remaining), Is.SameAs(rig.PlayStationSouth));

                Release(dualShock.buttonSouth);
                rig.Source.Update();
                Assert.That(keys.GetKey(GCControllerInputName.ButtonSouth, remaining.JoystickIndex), Is.False);

                // H-082: no Game Controller symbol, so the monitor falls back to its authored glyph map.
                Assert.That(rig.Source.GlyphSource.TryGetGlyph(GCControllerInputName.ButtonSouth, remaining.JoystickIndex, out Texture2D glyph), Is.False);
                Assert.That(glyph, Is.Null);

                // Replacement: a different Xbox pad takes the second slot; the DualShock keeps its history.
                Press(dualShock.buttonSouth);
                rig.Source.Update();
                Assert.That(keys.GetKeyDown(GCControllerInputName.ButtonSouth, 0), Is.True);
                // GCController keeps a button's history only across polls in which it is asked for the button
                // (its GetButtonDown), as the processors ask every frame, so each poll below asks for both pads.
                Gamepad replacement = (Gamepad)InputSystemApi.AddDevice("XInputController");
                Assert.That(rig.Provider.NotifyIfChanged(), Is.True);
                rig.Source.Update();
                ApplePluginControllerInputBinding kept = rig.CurrentAt(0), replacing = rig.CurrentAt(1);
                Assert.That((kept.BindingInputType, replacing.BindingInputType), Is.EqualTo((InputType.PS4Controller, InputType.XboneController)));
                Assert.That(keys.GetKey(GCControllerInputName.ButtonSouth, kept.JoystickIndex), Is.True, "the DualShock is still held");
                Assert.That(keys.GetKeyDown(GCControllerInputName.ButtonSouth, kept.JoystickIndex), Is.False, "and its press is not new");
                Assert.That(keys.GetKey(GCControllerInputName.ButtonSouth, replacing.JoystickIndex), Is.False);
                Press(replacement.buttonSouth);
                rig.Source.Update();
                Assert.That(keys.GetKeyDown(GCControllerInputName.ButtonSouth, replacing.JoystickIndex), Is.True, "the replacement reads its own pad");
                Assert.That(keys.GetKeyDown(GCControllerInputName.ButtonSouth, kept.JoystickIndex), Is.False);
                Assert.That(rig.Glyph(kept), Is.SameAs(rig.PlayStationSouth));
                Assert.That(rig.Glyph(replacing), Is.SameAs(rig.XboxSouth));

                // Reconnect: the first Xbox pad's own Gamepad object comes back in place of the replacement.
                InputSystemApi.RemoveDevice(replacement);
                InputSystemApi.AddDevice(xbox);
                Assert.That(rig.Provider.Gamepads, Is.EqualTo(new[] { dualShock, xbox }));
                Assert.That(rig.Provider.NotifyIfChanged(), Is.True);
                rig.Source.Update();
                ApplePluginControllerInputBinding stayed = rig.CurrentAt(0), reconnected = rig.CurrentAt(1);
                Assert.That((stayed.BindingInputType, reconnected.BindingInputType), Is.EqualTo((InputType.PS4Controller, InputType.XboneController)));
                Assert.That(keys.GetKey(GCControllerInputName.ButtonSouth, stayed.JoystickIndex), Is.True, "the DualShock is still held");
                Assert.That(keys.GetKeyDown(GCControllerInputName.ButtonSouth, stayed.JoystickIndex), Is.False, "through the reconnect");
                Assert.That(keys.GetKey(GCControllerInputName.ButtonSouth, reconnected.JoystickIndex), Is.False, "the replacement's press is gone");
                Press(xbox.buttonSouth);
                rig.Source.Update();
                Assert.That(keys.GetKeyDown(GCControllerInputName.ButtonSouth, reconnected.JoystickIndex), Is.True, "the reconnected pad reads its own press");
                Assert.That(keys.GetKey(GCControllerInputName.ButtonSouth, stayed.JoystickIndex), Is.True);
                Assert.That(keys.GetKeyDown(GCControllerInputName.ButtonSouth, stayed.JoystickIndex), Is.False);
                Assert.That(rig.Glyph(stayed), Is.SameAs(rig.PlayStationSouth));
                Assert.That(rig.Glyph(reconnected), Is.SameAs(rig.XboxSouth));
            });
        }

        // The original service removes a disconnected controller (0x0600006b) and constructs a new GCController,
        // with empty button history, on connect (0x06000069, 0x06000052). Input System revives the same Gamepad
        // object on reconnect; when that happens between two refreshes, the pad's first press after the
        // reconnect is still a new press. A refresh caused by another device keeps an uninterrupted controller.
        [Test]
        public void ReconnectingTheSameGamepadStartsAFreshButtonHistory()
        {
            Gamepad gamepad = (Gamepad)InputSystemApi.AddDevice("DualShockGamepad");
            InputSystemControllerProvider provider = Listening(out List<int> _);
            var source = new SourceApplePluginControllerProvider();
            try
            {
                source.Initialise(provider);
                IApplePluginControllerInputKeySource keys = source.KeySource;
                Press(gamepad.buttonSouth);
                source.Update();
                Assert.That(keys.GetKeyDown(GCControllerInputName.ButtonSouth, 0), Is.True);
                source.Update();
                Assert.That(keys.GetKeyDown(GCControllerInputName.ButtonSouth, 0), Is.False, "consecutive polls keep the history");
                Assert.That(keys.GetKey(GCControllerInputName.ButtonSouth, 0), Is.True);

                Gamepad other = (Gamepad)InputSystemApi.AddDevice("DualShockGamepad");
                source.Update();
                Assert.That(keys.GetKey(GCControllerInputName.ButtonSouth, 0), Is.True);
                Assert.That(keys.GetKeyDown(GCControllerInputName.ButtonSouth, 0), Is.False, "a rebuild for another device keeps the history");
                InputSystemApi.RemoveDevice(other);

                // Disconnect and reconnect the same Gamepad object with no refresh between them.
                InputSystemApi.RemoveDevice(gamepad);
                InputSystemApi.AddDevice(gamepad);
                Press(gamepad.buttonSouth);
                source.Update();
                Assert.That(provider.Gamepads, Is.EqualTo(new[] { gamepad }));
                Assert.That(keys.GetKeyDown(GCControllerInputName.ButtonSouth, 0), Is.True, "the reconnected pad's first press is new");
            }
            finally
            {
                source.Shutdown();
                provider.Shutdown();
            }
        }

        // Stopping a provider empties both lists, disconnects its view's controllers and removes their
        // registrations, so a pad that stays added is no longer polled through them, while another provider's
        // view keeps its controller and button history. Restarting registers new controllers. Each round tears
        // a real monitor down, and WithMonitor checks the subscriptions and registrations return to their entry
        // state.
        [Test]
        public void StoppingReleasesOnlyThatProvidersControllersAndRestartingRegistersAgain()
        {
            Gamepad gamepad = (Gamepad)InputSystemApi.AddDevice("DualShockGamepad");
            int entry = InputSystemGCControllers.RegisteredHandles;
            InputSystemControllerProvider staying = Listening(out List<int> _);
            var stayingSource = new SourceApplePluginControllerProvider();
            try
            {
                stayingSource.Initialise(staying);
                IApplePluginControllerInputKeySource stayingKeys = stayingSource.KeySource;
                Press(gamepad.buttonSouth);
                stayingSource.Update();
                Assert.That(stayingKeys.GetKeyDown(GCControllerInputName.ButtonSouth, 0), Is.True);
                Assert.That(InputSystemGCControllers.For(staying), Is.SameAs(InputSystemGCControllers.For(staying)), "one view per provider");
                GCController stayingController = InputSystemGCControllers.For(staying).GetControllers()[0];
                Assert.That(InputSystemGCControllers.RegisteredHandles, Is.EqualTo(entry + 1));

                for (int round = 0; round < 3; round++)
                    WithMonitor(rig =>
                    {
                        IReadOnlyList<GCController> controllers = rig.View.GetControllers();
                        GCController first = controllers[0];
                        Assert.That(InputSystemGCControllers.RegisteredHandles, Is.EqualTo(entry + 2));
                        rig.Source.Update();
                        Assert.That(rig.Source.KeySource.GetKeyDown(GCControllerInputName.ButtonSouth, 0), Is.True, "a new view's controller starts its own history");

                        rig.Provider.StopListening();
                        Assert.That(rig.Provider.GetControllerNames(), Is.Empty);
                        Assert.That(rig.View.GetControllers(), Is.Empty);
                        Assert.That(controllers, Is.Empty, "the view's list empties with the provider's");
                        Assert.That(first.IsConnected, Is.False);
                        Assert.That(gamepad.added, Is.True);
                        Assert.That(GCControllerService.PollController(first.Handle), Is.EqualTo(GCControllerInputState.None), "a released handle no longer reads the pad");
                        Assert.That(InputSystemGCControllers.RegisteredHandles, Is.EqualTo(entry + 1), "only the stopped provider's registration goes");

                        stayingSource.Update();
                        Assert.That(stayingController.IsConnected, Is.True);
                        Assert.That(stayingKeys.GetKey(GCControllerInputName.ButtonSouth, 0), Is.True);
                        Assert.That(stayingKeys.GetKeyDown(GCControllerInputName.ButtonSouth, 0), Is.False, "the other provider keeps its history");

                        rig.Provider.StartListening();
                        GCController restarted = rig.View.GetControllers()[0];
                        Assert.That(restarted, Is.Not.SameAs(first));
                        Assert.That(InputSystemGCControllers.RegisteredHandles, Is.EqualTo(entry + 2));
                        Assert.That(GCControllerService.PollController(restarted.Handle).ButtonA, Is.EqualTo(1f));
                    });
            }
            finally
            {
                stayingSource.Shutdown();
                staying.Shutdown();
            }
            Assert.That(InputSystemGCControllers.RegisteredHandles, Is.EqualTo(entry));
        }

        // The original refresh instantiates a binding clone per matching controller and drops the previous list
        // without destroying it. WithMonitor takes every published list, at initialisation and on each connection
        // update, whether or not the body reads it, and destroys those clones with the rest of the fixture. After
        // repeated successful uses, a body failure and a failing teardown step, no clone, subscription or
        // registration outlives the fixture, and a teardown failure never replaces the body's failure.
        [Test]
        public void MonitorFixtureReleasesEveryBindingCloneItCreates()
        {
            InputSystemApi.AddDevice("DualShockGamepad");
            int registered = InputSystemGCControllers.RegisteredHandles;
            HashSet<ApplePluginControllerInputBinding> entry = LiveBindings();
            var seen = new List<ApplePluginControllerInputBinding>();
            InputSystemControllerProvider provider = null;
            int listeners = -1;
            void Observe(Rig rig)
            {
                provider = rig.Provider;
                listeners = rig.EntryListeners;
                seen.AddRange(rig.Bindings.CurrentInputBindings);
            }
            void AssertReleased(string label)
            {
                Assert.That(seen, Is.Not.Empty, label + ": the refresh created clones");
                Assert.That(seen.TrueForAll(clone => clone == null), Is.True, label + ": every clone destroyed");
                Assert.That(LiveBindings().SetEquals(entry), Is.True, label + ": no binding outlives the fixture");
                Assert.That(Listeners(provider), Is.EqualTo(listeners), label + ": subscriptions back to entry");
                Assert.That(InputSystemGCControllers.RegisteredHandles, Is.EqualTo(registered), label + ": registrations back to entry");
                seen.Clear();
            }

            // Repeated successful uses, each refreshing twice more on a second pad's connect and disconnect,
            // with none of the clones read through CurrentAt.
            for (int round = 0; round < 3; round++)
            {
                WithMonitor(rig =>
                {
                    Observe(rig);
                    Gamepad second = (Gamepad)InputSystemApi.AddDevice("DualShockGamepad");
                    rig.Provider.NotifyIfChanged();
                    Assert.That(rig.Bindings.CurrentInputBindings, Has.Count.EqualTo(2));
                    seen.AddRange(rig.Bindings.CurrentInputBindings);
                    InputSystemApi.RemoveDevice(second);
                    rig.Provider.NotifyIfChanged();
                    seen.AddRange(rig.Bindings.CurrentInputBindings);
                });
                Assert.That(new HashSet<ApplePluginControllerInputBinding>(seen), Has.Count.EqualTo(4), "four refreshes, one clone per pad each");
                AssertReleased("round " + round);
            }

            // A body failure after initialisation, before any clone is read.
            var body = new InvalidOperationException("injected body failure");
            Exception thrown = Assert.Catch(() => WithMonitor(rig => { Observe(rig); throw body; }));
            Assert.That(thrown, Is.SameAs(body));
            Assert.That(thrown.Data.Contains(CleanupKey), Is.False);
            AssertReleased("body failure");

            // A failing first teardown step: every later step still runs, and the body's failure is kept with the
            // teardown failure attached; with no body failure the teardown failure is thrown.
            var teardown = new InvalidOperationException("injected teardown failure");
            thrown = Assert.Catch(() => WithMonitor(rig => { Observe(rig); throw body; }, rig => throw teardown));
            Assert.That(thrown, Is.SameAs(body));
            Assert.That(((AggregateException)thrown.Data[CleanupKey]).InnerExceptions, Does.Contain(teardown));
            AssertReleased("body and teardown failure");

            thrown = Assert.Catch(() => WithMonitor(Observe, rig => throw teardown));
            Assert.That(thrown, Is.TypeOf<AggregateException>());
            Assert.That(((AggregateException)thrown).InnerExceptions, Does.Contain(teardown));
            AssertReleased("teardown failure");
        }

        private const string CleanupKey = "FixtureCleanup";

        private static HashSet<ApplePluginControllerInputBinding> LiveBindings() =>
            new HashSet<ApplePluginControllerInputBinding>(Resources.FindObjectsOfTypeAll<ApplePluginControllerInputBinding>());

        [Test]
        public void PollReadsEveryGamepadControlInGameControllerLayout()
        {
            Gamepad gamepad = (Gamepad)InputSystemApi.AddDevice("DualShockGamepad");
            InputSystemControllerProvider provider = Listening(out List<int> _);
            try
            {
                IReadOnlyList<GCController> controllers = InputSystemGCControllers.For(provider).GetControllers();
                Assert.That(controllers.Count, Is.EqualTo(1));
                GCControllerHandle handle = controllers[0].Handle;
                Assert.That(handle.VendorName, Is.EqualTo("DUALSHOCK 4 Wireless Controller"));
                Assert.That(handle.GetControllerType(), Is.EqualTo(GCControllerType.DualShock));

                Press(gamepad.startButton); Press(gamepad.selectButton); Press(gamepad.buttonSouth); Press(gamepad.buttonNorth);
                Press(gamepad.leftShoulder); Press(gamepad.rightStickButton);
                Press(gamepad.TryGetChildControl<UnityEngine.InputSystem.Controls.ButtonControl>("touchpadButton"));
                Set(gamepad.rightTrigger, 0.75f);
                Set(gamepad.leftStick, new Vector2(-0.5f, 0.25f));
                Set(gamepad.rightStick, new Vector2(0.125f, -1f));
                Press(gamepad.dpad.left); Press(gamepad.dpad.up);

                GCControllerInputState state = GCControllerService.PollController(handle);
                Assert.That(new[] { state.ButtonMenu, state.ButtonOptions, state.ButtonA, state.ButtonB, state.ButtonX, state.ButtonY },
                    Is.EqualTo(new[] { 1f, 1f, 1f, 0f, 0f, 1f }));
                Assert.That(new[] { state.ShoulderLeftFront, state.ShoulderLeftBack, state.ShoulderRightFront, state.ShoulderRightBack },
                    Is.EqualTo(new[] { 1f, 0f, 0f, 0.75f }));
                Assert.That(new[] { state.DpadHorizontal, state.DpadVertical }, Is.EqualTo(new[] { -1f, 1f }));
                Assert.That(state.ThumbstickLeftHorizontal, Is.EqualTo(gamepad.leftStick.x.ReadValue()));
                Assert.That(state.ThumbstickLeftVertical, Is.EqualTo(gamepad.leftStick.y.ReadValue()));
                Assert.That(state.ThumbstickLeftHorizontal, Is.LessThan(0f));
                Assert.That(state.ThumbstickRightHorizontal, Is.EqualTo(gamepad.rightStick.x.ReadValue()));
                Assert.That(state.ThumbstickRightVertical, Is.EqualTo(gamepad.rightStick.y.ReadValue()));
                Assert.That(state.ThumbstickRightVertical, Is.LessThan(0f));
                Assert.That(new[] { state.ThumbstickLeftButton, state.ThumbstickRightButton, state.TouchpadButton }, Is.EqualTo(new[] { 0f, 1f, 1f }));

                // GCController.Poll reaches the hook, and its button-down history works on the result.
                controllers[0].Poll();
                Assert.That(controllers[0].GetButtonDown(GCControllerInputName.ButtonSouth), Is.True);
                controllers[0].Poll();
                Assert.That(controllers[0].GetButtonDown(GCControllerInputName.ButtonSouth), Is.False);

                InputSystemApi.RemoveDevice(gamepad);
                Assert.That(InputSystemGCControllers.For(provider).GetControllers(), Is.Empty);
                Assert.That(GCControllerService.PollController(handle), Is.EqualTo(GCControllerInputState.None), "a disconnected controller reads None");
            }
            finally { provider.StopListening(); }
        }

        [Test]
        public void SourceKeepsTheOriginalCastForGameControllerProvidersAndWithTheAdaptersOff()
        {
            InputSystemApi.AddDevice("XInputController");
            InputSystemControllerProvider provider = Listening(out List<int> _);
            var apple = new ApplePluginControllerProvider(3f);
            try
            {
                Assert.That(InputSystemGCControllers.For(apple), Is.SameAs(apple), "a Game Controller provider passes through unchanged");
                Assert.That(InputSystemGCControllers.For(new UnityControllerNameProvider(3f)), Is.Null, "any other provider fails the cast, as in the original");

                var source = new SourceApplePluginControllerProvider();
                source.Initialise(provider);
                Assert.That(ControllerProviderOf(source), Is.InstanceOf<InputSystemGCControllers>());
                using (OfflineAdapters.Override(false))
                {
                    var original = new SourceApplePluginControllerProvider();
                    original.Initialise(provider);
                    Assert.That(ControllerProviderOf(original), Is.Null, "the original cast fails for the Input System provider");
                    original.Initialise(apple);
                    Assert.That(ControllerProviderOf(original), Is.SameAs(apple));
                }
            }
            finally { provider.StopListening(); }
        }

        private sealed class Rig
        {
            public InputSystemControllerProvider Provider;
            public ApplePluginControllerInputMonitor Monitor;
            public ActiveApplePluginControllerInputBindings Bindings;
            public SourceApplePluginControllerProvider Source;
            public Texture2D XboxSouth, PlayStationSouth;
            public int EntryListeners;
            public readonly List<Object> Created = new List<Object>();

            // Takes custody of a published binding list: the original refresh never destroys its clones.
            public void Collect(IReadOnlyList<ApplePluginControllerInputBinding> current)
            {
                foreach (ApplePluginControllerInputBinding clone in current)
                    if (!Created.Contains(clone)) Created.Add(clone);
            }

            public IControllerProvider<GCController> View => InputSystemGCControllers.For(Provider);

            public ApplePluginControllerInputBinding CurrentAt(int joystickIndex) =>
                InputSystemControllerProviderTests.CurrentAt(Bindings, joystickIndex, Created);

            // The monitor's original glyph lookup for South at the binding's input type and index.
            public Texture2D Glyph(ApplePluginControllerInputBinding binding) =>
                (Texture2D)typeof(ApplePluginControllerInputMonitor).GetMethod("GetGlyphForKeyAndInputType", AnyInstance)
                    .Invoke(Monitor, new object[] { GCControllerInputName.ButtonSouth, binding.BindingInputType, binding.JoystickIndex });
        }

        // An owned controller monitor through its real lifecycle: authored with its own bindings (the shipped
        // DUALSHOCK and Xbox identifiers) and a glyph map with a distinct South glyph per input type, initialised
        // over a listening provider as HLInputModule does, then destroyed, so OnDestroy runs the original
        // Shutdown, before the provider shuts down. Every binding clone the refresh publishes is collected as it
        // is published and destroyed with the fixture's own objects. Each teardown step is attempted even when an
        // earlier one fails; a teardown failure is attached to a body failure rather than replacing it. Afterwards
        // the provider's subscriptions and the registered handles are back where they started. teardownFault
        // injects a failure as the first teardown step.
        private static void WithMonitor(Action<Rig> body, Action<Rig> teardownFault = null)
        {
            int registered = InputSystemGCControllers.RegisteredHandles;
            var rig = new Rig { Provider = Listening(out List<int> _) };
            int listeners = Listeners(rig.Provider);
            rig.EntryListeners = listeners;
            Action<IReadOnlyList<ApplePluginControllerInputBinding>> collect = rig.Collect;
            Exception primary = null;
            try
            {
                rig.XboxSouth = Own(rig, new Texture2D(1, 1) { name = "Xbox South" });
                rig.PlayStationSouth = Own(rig, new Texture2D(1, 1) { name = "PlayStation South" });
                var glyphs = Authored<ApplePluginControllerGlyphMap>(rig);
                SetField(glyphs, "m_inputGlyphs", new List<ApplePluginControllerGlyphMap.MappedGlyphs>
                {
                    South(InputType.XboneController, rig.XboxSouth), South(InputType.PS4Controller, rig.PlayStationSouth),
                });
                var binding = Authored<ApplePluginControllerInputBinding>(rig);
                SetField(binding, "m_bindingData", new List<ApplePluginControllerBindingData>());
                SetField(binding, "m_bindingIdentifiers", new List<BindingIdentifier>
                {
                    new BindingIdentifier { Identifier = "DUALSHOCK", InputType = InputType.PS4Controller },
                    new BindingIdentifier { Identifier = "Xbox", InputType = InputType.XboneController },
                });
                rig.Bindings = Authored<ActiveApplePluginControllerInputBindings>(rig);
                SetField(rig.Bindings, "m_macOSBindings", new List<ApplePluginControllerInputBinding> { binding });
                rig.Bindings.OnCurrentInputBindingsUpdate += collect;
                rig.Monitor = Authored<ApplePluginControllerInputMonitor>(rig);
                var authored = new SerializedObject(rig.Monitor);
                authored.FindProperty("m_inputBinding").objectReferenceValue = rig.Bindings;
                authored.FindProperty("m_glyphMapButtonSupplier").objectReferenceValue = glyphs;
                authored.ApplyModifiedPropertiesWithoutUndo();
                rig.Monitor.Initialise(rig.Provider, Authored<ControlMapping>(rig), new GlyphLookupSystem());
                rig.Source = (SourceApplePluginControllerProvider)typeof(ApplePluginControllerInputMonitor)
                    .GetField("m_sourceProvider", AnyInstance).GetValue(rig.Monitor);
                body(rig);
            }
            catch (Exception failure)
            {
                primary = failure;
                throw;
            }
            finally
            {
                var failures = new List<Exception>();
                void Attempt(Action step) { try { step(); } catch (Exception failure) { failures.Add(failure); } }
                if (teardownFault != null) Attempt(() => teardownFault(rig));
                if (rig.Bindings != null) Attempt(() => rig.Collect(rig.Bindings.CurrentInputBindings));
                if (rig.Monitor != null) Attempt(() => Object.DestroyImmediate(rig.Monitor));
                if (rig.Bindings != null) Attempt(() => rig.Bindings.OnCurrentInputBindingsUpdate -= collect);
                Attempt(() => rig.Provider.Shutdown());
                foreach (Object owned in rig.Created) if (owned != null) Attempt(() => Object.DestroyImmediate(owned));
                if (failures.Count != 0)
                {
                    var cleanup = new AggregateException("WithMonitor teardown failed.", failures);
                    if (primary != null) primary.Data[CleanupKey] = cleanup;
                    else throw cleanup;
                }
            }
            Assert.That(Listeners(rig.Provider), Is.EqualTo(listeners), "teardown removes the bindings' subscription");
            Assert.That(InputSystemGCControllers.RegisteredHandles, Is.EqualTo(registered), "teardown releases the view's registrations");
            Assert.That(rig.Provider.GetControllerNames(), Is.Empty);
        }

        private static ApplePluginControllerGlyphMap.MappedGlyphs South(InputType inputType, Texture2D glyph) =>
            new ApplePluginControllerGlyphMap.MappedGlyphs
            {
                InputType = inputType,
                GlyphMaps = new List<ApplePluginControllerGlyphMap.KeyGlyphMap>
                {
                    new ApplePluginControllerGlyphMap.KeyGlyphMap { Key = GCControllerInputName.ButtonSouth, Glyph = glyph },
                },
            };

        private static int Listeners(InputSystemControllerProvider provider) =>
            provider.OnControllerConnectionUpdate?.GetInvocationListCount() ?? 0;

        // A new instance with the default state Unity's serializer gives an authored asset: its serializable
        // processors and lists exist, empty.
        private static T Authored<T>(Rig rig) where T : ScriptableObject
        {
            T asset = Own(rig, ScriptableObject.CreateInstance<T>());
            EditorJsonUtility.FromJsonOverwrite(EditorJsonUtility.ToJson(asset), asset);
            return asset;
        }

        private static T Own<T>(Rig rig, T owned) where T : Object
        {
            rig.Created.Add(owned);
            return owned;
        }

        private static object ControllerProviderOf(SourceApplePluginControllerProvider source)
        {
            object controller = typeof(SourceApplePluginControllerProvider).GetField("m_sourceApplePluginController", AnyInstance).GetValue(source);
            return typeof(SourceApplePluginController).GetField("m_controllerProvider", AnyInstance).GetValue(controller);
        }

        private static ApplePluginControllerInputBinding CurrentAt(ActiveApplePluginControllerInputBindings bindings, int joystickIndex, List<Object> created)
        {
            ApplePluginControllerInputBinding found = null;
            foreach (ApplePluginControllerInputBinding current in bindings.CurrentInputBindings)
            {
                if (!created.Contains(current)) created.Add(current);
                if (current.JoystickIndex == joystickIndex) { Assert.That(found, Is.Null); found = current; }
            }
            Assert.That(found, Is.Not.Null, "binding at joystick index " + joystickIndex);
            return found;
        }

        private static void SetField(object owner, string name, object value)
        {
            for (System.Type type = owner.GetType(); type != null; type = type.BaseType)
            {
                FieldInfo field = type.GetField(name, AnyInstance | BindingFlags.DeclaredOnly);
                if (field == null) continue;
                field.SetValue(owner, value);
                return;
            }
            Assert.Fail("no field " + name);
        }
    }
}

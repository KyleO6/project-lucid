using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Hardlight;
using HardlightProject;
using NUnit.Framework;
using ProjectLucid.Offline;
using UnityEditor;
using UnityEngine;

namespace ProjectLucid.Tests
{
    // The Windows and Linux players take the shipped macOS player's platform branches (#503, hooks
    // H-070..H-076). Each test substitutes the player platform through OfflineDesktopPlatform's test seam;
    // the editor platform itself is never one of the substituted players.
    public sealed class OfflinePlatformHookTests
    {
        private const BindingFlags Declared = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public
            | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        private static readonly RuntimePlatform[] NonApplePlayers = { RuntimePlatform.WindowsPlayer, RuntimePlatform.LinuxPlayer };

        [Test]
        public void OnlyTheWindowsAndLinuxPlayersTakeTheMacPlayerBranch()
        {
            foreach (RuntimePlatform platform in Enum.GetValues(typeof(RuntimePlatform)))
                Assert.That(OfflineDesktopPlatform.TakesMacPlayerBranch(platform),
                    Is.EqualTo(platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.LinuxPlayer), platform.ToString());
            Assert.That(OfflineDesktopPlatform.IsMacPlayerEquivalent, Is.False, "the editor keeps its own branches");
            using (OfflineDesktopPlatform.OverridePlatform(RuntimePlatform.LinuxPlayer))
            {
                Assert.That(OfflineDesktopPlatform.IsMacPlayerEquivalent, Is.True);
                using (OfflineDesktopPlatform.OverridePlatform(RuntimePlatform.OSXPlayer))
                    Assert.That(OfflineDesktopPlatform.IsMacPlayerEquivalent, Is.False, "the macOS player runs the original check");
                Assert.That(OfflineDesktopPlatform.IsMacPlayerEquivalent, Is.True, "a nested override restores the outer one");
            }
            Assert.That(OfflineDesktopPlatform.IsMacPlayerEquivalent, Is.False);
        }

        // H-076: the shipped controller monitor lists iOS, macOS, tvOS and the macOS editor (no Windows or Linux),
        // and the touch monitor lists iOS, macOS, both editors and Windows (no Linux). Each non-Apple player is
        // active exactly where the list names the macOS player. The platform override substitutes only that player
        // test, so with adapters off, and for the editor itself, the original looks up the actual editor platform.
        [Test]
        public void InputMonitorsAreActiveOnWindowsAndLinuxWhereTheListNamesTheMacPlayer()
        {
            var lists = new (string Name, RuntimePlatform[] Platforms, bool Hooked)[]
            {
                ("controller", new[] { RuntimePlatform.IPhonePlayer, RuntimePlatform.OSXPlayer, RuntimePlatform.tvOS, RuntimePlatform.OSXEditor }, true),
                ("touch", new[] { RuntimePlatform.IPhonePlayer, RuntimePlatform.OSXPlayer, RuntimePlatform.OSXEditor,
                    RuntimePlatform.WindowsEditor, RuntimePlatform.WindowsPlayer }, true),
                ("list without the macOS player", new[] { RuntimePlatform.IPhonePlayer, RuntimePlatform.tvOS }, false),
            };
            foreach ((string name, RuntimePlatform[] platforms, bool hooked) in lists)
                WithMonitor(platforms, monitor =>
                {
                    bool original = platforms.Contains(Application.platform);
                    foreach (RuntimePlatform player in NonApplePlayers)
                        using (OfflineDesktopPlatform.OverridePlatform(player))
                        {
                            Assert.That(IsActive(monitor), Is.EqualTo(hooked), player + " " + name + " monitor");
                            using (OfflineAdapters.Override(false))
                                Assert.That(IsActive(monitor), Is.EqualTo(original), player + " " + name + " original: the actual " + Application.platform);
                        }
                    Assert.That(IsActive(monitor), Is.EqualTo(original), name + " monitor on the actual " + Application.platform);
                });
        }

        private sealed class NoControllers : BaseControllerProvider
        {
            public NoControllers() : base(1) { }
            public override IReadOnlyList<string> GetControllerNames() => Array.Empty<string>();
        }

        // An owned monitor taken through its real lifecycle: authored with the platform list, its own input
        // bindings and glyph maps, initialised as HLInputModule does, and destroyed before its dependencies, so
        // OnDestroy runs the original Shutdown. Initialise and Shutdown both read the actual editor platform, so
        // they agree on whether the control map is in use.
        private static void WithMonitor(RuntimePlatform[] platforms, Action<InputMonitor> body)
        {
            var owned = new List<UnityEngine.Object>();
            try
            {
                InputMonitor monitor = Authored<InputMonitor>(owned);
                var authored = new SerializedObject(monitor);
                SerializedProperty list = authored.FindProperty("m_activePlatforms");
                list.arraySize = platforms.Length;
                for (int i = 0; i < platforms.Length; i++) list.GetArrayElementAtIndex(i).intValue = (int)platforms[i];
                authored.FindProperty("m_inputBinding").objectReferenceValue = Authored<ActiveGameInputBindings>(owned);
                authored.FindProperty("m_glyphMapButtonSupplier").objectReferenceValue = Authored<KeyCodeGlyphMap>(owned);
                authored.FindProperty("m_glyphMapAxisSupplier").objectReferenceValue = Authored<StringGlyphMap>(owned);
                authored.FindProperty("m_glyphMapSwipeSupplier").objectReferenceValue = Authored<DirectionGlyphMap>(owned);
                authored.FindProperty("m_glyphMapFixedSupplier").objectReferenceValue = Authored<FixedBindingsGlyphMap>(owned);
                authored.FindProperty("m_glyphMapGameInputSupplier").objectReferenceValue = Authored<GameInputGlyphMap>(owned);
                authored.ApplyModifiedPropertiesWithoutUndo();
                Assert.That(Field(typeof(InputMonitor), "m_activePlatforms").GetValue(monitor), Is.EqualTo(platforms));
                monitor.Initialise(new NoControllers(), Authored<ControlMapping>(owned), new GlyphLookupSystem());
                body(monitor);
            }
            finally
            {
                foreach (UnityEngine.Object o in owned) UnityEngine.Object.DestroyImmediate(o);
            }
        }

        // A new instance with the default state Unity's serializer gives an authored asset: its serializable
        // processors and lists exist, empty.
        private static T Authored<T>(List<UnityEngine.Object> owned) where T : ScriptableObject
        {
            T asset = Own(owned, ScriptableObject.CreateInstance<T>());
            EditorJsonUtility.FromJsonOverwrite(EditorJsonUtility.ToJson(asset), asset);
            return asset;
        }

        // H-072: the device performance table holds the iOS, macOS and tvOS definitions. Windows and Linux use the
        // macOS definition: its first row whose generation equals the original MatchMacToDeviceGeneration result
        // for this machine's model (High for a model outside its table, whatever the operating system), and its
        // authored fallback only when no row matches. The original (and the editor) find no entry and return the
        // supplied default.
        [Test]
        public void DefaultPerformanceProfileUsesTheMacDefinitionOnWindowsAndLinux()
        {
            var owned = new List<UnityEngine.Object>();
            try
            {
                PerformanceProfile fallback = Own(owned, ScriptableObject.CreateInstance<PerformanceProfile>());
                PerformanceProfile defaultProfile = Own(owned, ScriptableObject.CreateInstance<PerformanceProfile>());
                DevicePerformance_macOS mac = Own(owned, ScriptableObject.CreateInstance<DevicePerformance_macOS>());
                Field(typeof(DevicePerformance), "m_fallback").SetValue(mac, fallback);
                var table = new Dictionary<RuntimePlatform, DevicePerformance> { { RuntimePlatform.OSXPlayer, mac } };
                void Expect(PerformanceProfile hooked, string label)
                {
                    Assert.That(VisualQualityManager_SDT.GetDefaultPerformanceProfile(table, defaultProfile), Is.SameAs(defaultProfile), label + ": editor");
                    foreach (RuntimePlatform player in NonApplePlayers)
                        using (OfflineDesktopPlatform.OverridePlatform(player))
                        {
                            Assert.That(VisualQualityManager_SDT.GetDefaultPerformanceProfile(table, defaultProfile), Is.SameAs(hooked), label + ": " + player);
                            using (OfflineAdapters.Override(false))
                                Assert.That(VisualQualityManager_SDT.GetDefaultPerformanceProfile(table, defaultProfile), Is.SameAs(defaultProfile), label + ": original");
                        }
                }

                // One row per generation, each with its own profile: the row the real matcher picks for this model.
                MacDeviceGeneration[] generations = Enum.GetValues(typeof(MacDeviceGeneration)).Cast<MacDeviceGeneration>().ToArray();
                List<DevicePerformanceMatch_macOS> rows = generations
                    .Select(g => new DevicePerformanceMatch_macOS(g, Own(owned, ScriptableObject.CreateInstance<PerformanceProfile>()))).ToList();
                MacDeviceGeneration machine = rows[0].MatchMacToDeviceGeneration();
                // The matcher branches on the device model, not the operating system: High is required only when this
                // model is absent from the original table (its string literals).
                HashSet<string> models = MacModelTable();
                Assert.That(models, Has.Count.GreaterThan(30).And.Contains("iMac13,2").And.Contains("MacBookPro18,4"));
                if (!models.Contains(SystemInfo.deviceModel))
                    Assert.That(machine, Is.EqualTo(MacDeviceGeneration.High), SystemInfo.deviceModel + " is outside the original model table");
                PerformanceProfile matched = rows.Single(r => r.MacDevice == machine).Profile;
                Field(typeof(DevicePerformance_macOS), "m_devicePerformanceMatches").SetValue(mac, rows);
                Expect(matched, machine + " row");

                // Rows that all miss this model, and no rows: the authored fallback.
                Field(typeof(DevicePerformance_macOS), "m_devicePerformanceMatches").SetValue(mac, rows.Where(r => r.MacDevice != machine).ToList());
                Expect(fallback, "no matching row");
                Field(typeof(DevicePerformance_macOS), "m_devicePerformanceMatches").SetValue(mac, new List<DevicePerformanceMatch_macOS>());
                Expect(fallback, "no rows");
            }
            finally
            {
                foreach (UnityEngine.Object o in owned) UnityEngine.Object.DestroyImmediate(o);
            }
        }

        // H-070: the Boot hook runs after the base enter and before the original platform branch, and its body is
        // the original macOS branch instruction for instruction (display read, full-screen mode, SetResolution with
        // the 1/60 refresh rate); the original branch follows unchanged.
        [Test]
        public void BootDisplaySetupHookRepeatsTheMacPlayerBranch()
        {
            MethodInfo enter = typeof(ApplicationStateBoot).GetMethod("DoOnEnter", Declared);
            List<OriginalILCallSequence.Call> calls = OriginalILCallSequence.Calls(enter);
            string[] names = OriginalILCallSequence.Names(calls);
            int at = Array.IndexOf(names, "FSMState.DoOnEnter");
            Assert.That(names.Skip(at + 1).Take(10), Is.EqualTo(new[]
            {
                "OfflineAdapters.get_Active", "OfflineDesktopPlatform.get_IsMacPlayerEquivalent",
                "Screen.get_mainWindowDisplayInfo", "Screen.get_fullScreenMode", "Screen.SetResolution",
                "Application.get_platform",
                "Screen.get_mainWindowDisplayInfo", "Screen.get_fullScreenMode", "Screen.SetResolution",
                "Application.set_targetFrameRate",
            }), string.Join(", ", names));
            int hookStart = calls[at + 3].Offset, hookEnd = calls[at + 5].Offset;
            int macStart = calls[at + 7].Offset, macEnd = calls[at + 9].Offset;
            string[] hookShape = Shape(enter, hookStart, hookEnd), macShape = Shape(enter, macStart, macEnd);
            Assert.That(hookShape, Is.EqualTo(macShape), string.Join(" | ", hookShape) + " vs " + string.Join(" | ", macShape));
            Assert.That(calls[at + 5].Target, Is.EqualTo(calls[at + 9].Target));
            Assert.That(HookPolarity(enter, calls[at + 1].Offset, calls[at + 3].Offset, calls[at + 7].Offset), Is.Null);
        }

        // H-071: the resolution setting checks the switch and the non-Apple player before the original OSXPlayer
        // return, and the hook's true path lands past that return, on the display read that starts the macOS body.
        [Test]
        public void ResolutionSettingHookSkipsTheOriginalPlatformReturn()
        {
            AssertFallThrough(typeof(ScreenResolutionSetting).GetMethod("Apply", Declared), "Screen.get_mainWindowDisplayInfo");
        }

        // H-073: likewise for the display-change handling of the quality manager.
        [Test]
        public void DisplayChangeHookSkipsTheOriginalPlatformReturn()
        {
            AssertFallThrough(typeof(VisualQualityManager_SDT).GetMethod("ValidateResolution", Declared), "Screen.get_mainWindowDisplayInfo");
        }

        // H-074: in the start screen's post-transition routine, the non-Apple player test sits before the original
        // OSXPlayer test inside the cursor condition, and its true path reaches the cursor body.
        [Test]
        public void StartScreenCursorHookReachesTheCursorBody()
        {
            Type routine = typeof(UIContainerStart).GetNestedTypes(BindingFlags.NonPublic)
                .Single(t => t.Name.StartsWith("<UpdateUIAfterTransition>", StringComparison.Ordinal));
            MethodInfo moveNext = routine.GetMethod("MoveNext", Declared);
            string[] names = OriginalILCallSequence.Names(OriginalILCallSequence.Calls(moveNext));
            int active = Array.IndexOf(names, "OfflineAdapters.get_Active");
            Assert.That(names.Skip(active).Take(4), Is.EqualTo(new[]
            {
                "OfflineAdapters.get_Active", "OfflineDesktopPlatform.get_IsMacPlayerEquivalent", "Application.get_platform", "EventSystem.get_current",
            }), string.Join(", ", names));
            Assert.That(names.Take(active).Count(n => n == "UINavigationManager.get_IsActive"), Is.EqualTo(1), "the original navigation test comes first");
            AssertFallThrough(moveNext, "EventSystem.get_current");
        }

        private static void AssertFallThrough(MethodInfo method, string bodyStart)
        {
            List<OriginalILCallSequence.Call> calls = OriginalILCallSequence.Calls(method);
            string[] names = OriginalILCallSequence.Names(calls);
            int active = Array.IndexOf(names, "OfflineAdapters.get_Active");
            Assert.That(active, Is.GreaterThanOrEqualTo(0), string.Join(", ", names));
            Assert.That(names[active + 1], Is.EqualTo("OfflineDesktopPlatform.get_IsMacPlayerEquivalent"));
            Assert.That(names[active + 2], Is.EqualTo("Application.get_platform"), "the original platform check follows the hook");
            int body = Array.IndexOf(names, bodyStart, active + 3);
            Assert.That(body, Is.GreaterThan(active + 2), string.Join(", ", names));
            Assert.That(HookPolarity(method, calls[active].Offset, calls[body].Offset, calls[body].Offset), Is.Null);
        }

        // The polarity check rejects the hook shape with either read inverted, and accepts it as written.
        [Test]
        public void HookPolarityCheckRejectsInvertedConditions()
        {
            string Check(string name)
            {
                MethodInfo method = typeof(OfflinePlatformHookTests).GetMethod(name, Declared);
                List<OriginalILCallSequence.Call> calls = OriginalILCallSequence.Calls(method);
                string[] names = OriginalILCallSequence.Names(calls);
                int active = Array.IndexOf(names, "OfflineAdapters.get_Active"), body = Array.IndexOf(names, "Screen.get_mainWindowDisplayInfo");
                Assert.That(active, Is.GreaterThanOrEqualTo(0), name);
                Assert.That(body, Is.GreaterThan(active), name);
                return HookPolarity(method, calls[active].Offset, calls[body].Offset, calls[body].Offset);
            }
            Assert.That(Check(nameof(HookControl)), Is.Null);
            Assert.That(Check(nameof(InvertedPlayerHookControl)), Is.Not.Null);
            Assert.That(Check(nameof(InvertedAdaptersHookControl)), Is.Not.Null);
        }

        private static void HookControl()
        {
            if (OfflineAdapters.Active && OfflineDesktopPlatform.IsMacPlayerEquivalent)
            {
            }
            else if (Application.platform != RuntimePlatform.OSXPlayer)
                return;
            _ = Screen.mainWindowDisplayInfo;
        }

        private static void InvertedPlayerHookControl()
        {
            if (OfflineAdapters.Active && !OfflineDesktopPlatform.IsMacPlayerEquivalent)
            {
            }
            else if (Application.platform != RuntimePlatform.OSXPlayer)
                return;
            _ = Screen.mainWindowDisplayInfo;
        }

        private static void InvertedAdaptersHookControl()
        {
            if (!OfflineAdapters.Active && OfflineDesktopPlatform.IsMacPlayerEquivalent)
            {
            }
            else if (Application.platform != RuntimePlatform.OSXPlayer)
                return;
            _ = Screen.mainWindowDisplayInfo;
        }

        // Null when the hook's true route (adapters on, a substituted Windows or Linux player) reaches the hook
        // body without reading the actual platform, and each false route (adapters off, or a player that is not
        // substituted) reads it and reaches the original macOS body for the macOS player only; otherwise each
        // route that differs.
        private static string HookPolarity(MethodInfo method, int activeRead, int hookBody, int macBody)
        {
            var problems = new List<string>();
            foreach (RuntimePlatform platform in new[] { RuntimePlatform.OSXPlayer, RuntimePlatform.WindowsPlayer })
            {
                (int call, bool read) = Follow(method, activeRead, true, true, platform);
                if (call != hookBody || read)
                    problems.Add($"hook true, {platform}: reached IL_{call:x4}, platform read {read}");
                foreach ((bool active, bool substituted) in new[] { (false, true), (false, false), (true, false) })
                {
                    (call, read) = Follow(method, activeRead, active, substituted, platform);
                    bool mac = platform == RuntimePlatform.OSXPlayer;
                    if (!read || (call == macBody) != mac || (!mac && call == hookBody))
                        problems.Add($"adapters {active}, substituted {substituted}, {platform}: reached IL_{call:x4}, platform read {read}");
                }
            }
            return problems.Count == 0 ? null : method.Name + ": " + string.Join("; ", problems);
        }

        // Runs the IL from the OfflineAdapters.Active read with that read, the substituted-player read and the
        // actual platform read fixed, through debug locals, comparisons and branches, to the first other call
        // (-1 for a return), and says whether the actual platform was read on the way.
        private static (int Call, bool ReadPlatform) Follow(MethodInfo method, int from, bool active, bool substituted, RuntimePlatform platform)
        {
            byte[] il = method.GetMethodBody().GetILAsByteArray();
            List<(int Offset, OpCode Code)> all = OriginalILCallSequence.Instructions(method);
            var index = new Dictionary<int, int>();
            for (int i = 0; i < all.Count; i++) index[all[i].Offset] = i;
            var stack = new Stack<int>();
            var locals = new Dictionary<int, int>();
            bool read = false;
            int at = index[from];
            for (int steps = 0; steps < 200; steps++)
            {
                (int offset, OpCode code) = all[at];
                int operand = offset + code.Size;
                int next = at + 1 < all.Count ? all[at + 1].Offset : il.Length;
                string name = code.Name.EndsWith(".s", StringComparison.Ordinal) ? code.Name.Substring(0, code.Name.Length - 2) : code.Name;
                if (code == OpCodes.Call || code == OpCodes.Callvirt)
                {
                    MethodBase target = method.Module.ResolveMethod(BitConverter.ToInt32(il, operand));
                    string call = target.DeclaringType.Name + "." + target.Name;
                    if (call == "OfflineAdapters.get_Active") stack.Push(active ? 1 : 0);
                    else if (call == "OfflineDesktopPlatform.get_IsMacPlayerEquivalent") stack.Push(substituted ? 1 : 0);
                    else if (call == "Application.get_platform") { stack.Push((int)platform); read = true; }
                    else return (offset, read);
                }
                else if (code == OpCodes.Ret) return (-1, read);
                else if (code == OpCodes.Nop) { }
                else if (code == OpCodes.Pop) stack.Pop();
                else if (code == OpCodes.Dup) stack.Push(stack.Peek());
                else if (name.StartsWith("ldc.i4", StringComparison.Ordinal))
                    stack.Push(code == OpCodes.Ldc_I4_M1 ? -1 : code == OpCodes.Ldc_I4_S ? (sbyte)il[operand]
                        : code == OpCodes.Ldc_I4 ? BitConverter.ToInt32(il, operand) : name[7] - '0');
                else if (name.StartsWith("stloc", StringComparison.Ordinal)) locals[Local(code, name, il, operand)] = stack.Pop();
                else if (name.StartsWith("ldloc", StringComparison.Ordinal) && !name.StartsWith("ldloca", StringComparison.Ordinal))
                {
                    int local = Local(code, name, il, operand);
                    Assert.That(locals.ContainsKey(local), Is.True, $"{method.Name} IL_{offset:x4} reads local {local} before the hook stores it");
                    stack.Push(locals[local]);
                }
                else if (code.OperandType == OperandType.InlineNone && Compare(name, out Func<int, int, bool> compare))
                {
                    int b = stack.Pop(), a = stack.Pop();
                    stack.Push(compare(a, b) ? 1 : 0);
                }
                else if (code.OperandType == OperandType.ShortInlineBrTarget || code.OperandType == OperandType.InlineBrTarget)
                {
                    int target = next + (code.OperandType == OperandType.ShortInlineBrTarget ? (sbyte)il[operand] : BitConverter.ToInt32(il, operand));
                    bool taken;
                    if (name == "br") taken = true;
                    else if (name == "brtrue") taken = stack.Pop() != 0;
                    else if (name == "brfalse") taken = stack.Pop() == 0;
                    else
                    {
                        int b = stack.Pop(), a = stack.Pop();
                        Assert.That(Compare(name.Substring(1), out Func<int, int, bool> branch), Is.True, $"{method.Name} IL_{offset:x4} {code.Name}");
                        taken = branch(a, b);
                    }
                    if (taken)
                    {
                        at = index[target];
                        continue;
                    }
                }
                else Assert.Fail($"{method.Name} IL_{offset:x4}: {code.Name} is outside the hook's comparison");
                at++;
            }
            Assert.Fail(method.Name + ": no call within 200 instructions of the hook");
            return (0, false);
        }

        private static int Local(OpCode code, string name, byte[] il, int operand) =>
            code.OperandType == OperandType.ShortInlineVar ? il[operand]
            : code.OperandType == OperandType.InlineVar ? BitConverter.ToUInt16(il, operand) : name[6] - '0';

        // ceq/cgt/clt, and/or on the boolean reads, and, without their leading b, the conditional branches beq/bne/bge/bgt/ble/blt; the enum
        // and boolean values compared here are small and non-negative, so the unsigned forms agree.
        private static bool Compare(string name, out Func<int, int, bool> compare)
        {
            if (name.EndsWith(".un", StringComparison.Ordinal)) name = name.Substring(0, name.Length - 3);
            switch (name)
            {
                case "ceq": case "eq": compare = (a, b) => a == b; return true;
                case "ne": compare = (a, b) => a != b; return true;
                case "cgt": case "gt": compare = (a, b) => a > b; return true;
                case "ge": compare = (a, b) => a >= b; return true;
                case "clt": case "lt": compare = (a, b) => a < b; return true;
                case "le": compare = (a, b) => a <= b; return true;
                case "and": compare = (a, b) => a != 0 && b != 0; return true;
                case "or": compare = (a, b) => a != 0 || b != 0; return true;
                default: compare = null; return false;
            }
        }

        // The device models MatchMacToDeviceGeneration compares against: every string literal in its body.
        private static HashSet<string> MacModelTable()
        {
            MethodInfo match = typeof(DevicePerformanceMatch_macOS).GetMethod("MatchMacToDeviceGeneration", BindingFlags.Instance | BindingFlags.Public);
            byte[] il = match.GetMethodBody().GetILAsByteArray();
            var models = new HashSet<string>(StringComparer.Ordinal);
            foreach ((int offset, OpCode code) in OriginalILCallSequence.Instructions(match))
                if (code == OpCodes.Ldstr)
                    models.Add(match.Module.ResolveString(BitConverter.ToInt32(il, offset + code.Size)));
            return models;
        }

        // Opcodes from start to end (inclusive of the instruction at end), with local slots normalised and every
        // other operand kept, so two scopes of the same statements compare equal.
        private static string[] Shape(MethodInfo method, int start, int end)
        {
            byte[] il = method.GetMethodBody().GetILAsByteArray();
            List<(int Offset, OpCode Code)> all = OriginalILCallSequence.Instructions(method);
            var shape = new List<string>();
            for (int i = 0; i < all.Count && all[i].Offset <= end; i++)
            {
                if (all[i].Offset < start) continue;
                int next = i + 1 < all.Count ? all[i + 1].Offset : il.Length;
                OpCode code = all[i].Code;
                string name = code.Name;
                if (name.StartsWith("ldloc.", StringComparison.Ordinal) || name.StartsWith("stloc.", StringComparison.Ordinal))
                    name = name.Substring(0, 5);
                bool local = code.OperandType == OperandType.ShortInlineVar || code.OperandType == OperandType.InlineVar;
                int operand = all[i].Offset + code.Size;
                if (local) shape.Add(name.Replace(".s", ""));
                else if (code.OperandType == OperandType.InlineMethod)
                {
                    MethodBase target = method.Module.ResolveMethod(BitConverter.ToInt32(il, operand));
                    shape.Add(name + " " + target.DeclaringType.Name + "." + target.Name);
                }
                else shape.Add((name + " " + BitConverter.ToString(il, operand, next - operand)).TrimEnd());
            }
            return shape.ToArray();
        }

        private static bool IsActive(InputMonitor monitor)
        {
            MethodInfo isActive = monitor.GetType().BaseType.GetMethod("IsActiveOnPlatform", BindingFlags.Instance | BindingFlags.NonPublic);
            return (bool)isActive.Invoke(monitor, null);
        }

        private static FieldInfo Field(Type type, string name)
        {
            for (Type t = type; t != null; t = t.BaseType)
            {
                FieldInfo field = t.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);
                if (field != null) return field;
            }
            throw new MissingFieldException(type.Name, name);
        }

        private static T Own<T>(List<UnityEngine.Object> owned, T value) where T : UnityEngine.Object
        {
            owned.Add(value);
            return value;
        }
    }
}

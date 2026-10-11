// Original Game.Runtime.dll HardlightProject.HLInputDebugMenu020005f7 and natural <>c020005f8.
// Preserve all14 authored/3 natural methods and10 authored/2 compiler fields; inferred locals/source-form held.
// The readonly lists and genuine App SystemRef initializer run before the MonoBehaviour base constructor.
// Same visible glyph type hides only its parent; raw-input polling remains independent of parent visibility.
// Pool growth adds only one item; real EventSystem/HLInputModule, prefabs, RTL/TMP and App faults remain.
// Native evidence identifies original methods; current CIL/emission, serialized assets and Engine behavior require separate gates.
// 0x020005f7 0x06002006 Awake ARM64[0x536384,0x536388) x86_64[0x55efb0,0x55efc0).
// 0x020005f7 0x06002007 GetAbilitySteeringAssistDesc ARM64[0x536388,0x536530) x86_64[0x55efc0,0x55f140).
// 0x020005f7 0x06002008 SetAbilitySteeringAssist ARM64[0x536530,0x53665c) x86_64[0x55f140,0x55f270).
// 0x020005f7 0x06002009 TogglePS4Glyphs ARM64[0x53665c,0x536668) x86_64[0x55f270,0x55f280).
// 0x020005f7 0x0600200a ToggleXboxGlyphs ARM64[0x536dc8,0x536dd4) x86_64[0x55f9e0,0x55f9f0).
// 0x020005f7 0x0600200b ToggleKeyboardGlyphs ARM64[0x536dd4,0x536de0) x86_64[0x55f9f0,0x55fa00).
// 0x020005f7 0x0600200c ToggleTouchGlyphs ARM64[0x536de0,0x536dec) x86_64[0x55fa00,0x55fa10).
// 0x020005f7 0x0600200d ToggleGlyphs ARM64[0x536668,0x536dc8) x86_64[0x55f280,0x55f9e0).
// 0x020005f7 0x0600200e HideAllChildren ARM64[0x536dec,0x53708c) x86_64[0x55fa10,0x55fcc0).
// 0x020005f7 0x0600200f ToggleDisplayRawInput ARM64[0x53708c,0x5370e0) x86_64[0x55fcc0,0x55fd10).
// 0x020005f7 0x06002010 Update ARM64[0x5370e0,0x5370f0) x86_64[0x55fd10,0x55fd30).
// 0x020005f7 0x06002011 LogAllInput ARM64[0x5370f0,0x53740c) x86_64[0x55fd30,0x55fff0).
// 0x020005f7 0x06002012 SetupTextPoolObject ARM64[0x53740c,0x53758c) x86_64[0x55fff0,0x560160).
// 0x020005f7 0x06002013 .ctor ARM64[0x53758c,0x5376cc) x86_64[0x560160,0x560280).
// 0x020005f8 0x06002014 .cctor ARM64[0x5376cc,0x537738) x86_64[0x560280,0x5602f0).
// 0x020005f8 0x06002015 .ctor ARM64[0x537738,0x537740) x86_64[0x5602f0,0x560300).
// 0x020005f8 0x06002016 <ToggleGlyphs>b__17_0 ARM64[0x537740,0x5377e0) x86_64[0x560300,0x560390).
using System;
using System.Collections.Generic;
using Hardlight;
using RTLTMPro;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace HardlightProject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HLInputDebugMenu : MonoBehaviour
    {
        private const int MinimumAxis = 1;
        private const int MaximumAxis = 10;
        [SerializeField] private GameObject m_debugParent;
        [SerializeField] private GlyphMappingDebugDisplay m_glyphDebugPrefab;
        [SerializeField] private RTLTextMeshPro m_inputLogPrefab;
        private InputType m_lastShownInputType;
        private bool m_logAllInput;
        private readonly List<RTLTextMeshPro> m_textLogPool = new List<RTLTextMeshPro>();
        private readonly List<GlyphMappingDebugDisplay> m_glyphMappingPool = new List<GlyphMappingDebugDisplay>();
        private readonly SystemRef<App> m_appRef = ProcessManager.GetSystemRef<App>();

        private void Awake() { }

        private string GetAbilitySteeringAssistDesc(bool steeringAssist)
        {
            bool disabled = m_appRef.Get().Storage.GetValue(AppFSMKeys.InputAbilitySteeringAssistDisabled, false, true);
            return steeringAssist ? (disabled ? "On" : "<< On >>") : (disabled ? "<< Off >>" : "Off");
        }

        private void SetAbilitySteeringAssist(bool steeringAssist)
        {
            m_appRef.Get().Storage.SetValue(AppFSMKeys.InputAbilitySteeringAssistDisabled, !steeringAssist);
        }

        private void TogglePS4Glyphs() { ToggleGlyphs(InputType.PS4Controller); }
        private void ToggleXboxGlyphs() { ToggleGlyphs(InputType.XboneController); }
        private void ToggleKeyboardGlyphs() { ToggleGlyphs(InputType.Keyboard); }
        private void ToggleTouchGlyphs() { ToggleGlyphs(InputType.Touch); }

        private void ToggleGlyphs(InputType requestedType)
        {
            if (m_debugParent.activeSelf && m_debugParent.activeInHierarchy && m_lastShownInputType == requestedType)
            {
                m_debugParent.SetActive(false);
                return;
            }
            m_lastShownInputType = requestedType;
            m_debugParent.SetActive(true);
            HideAllChildren();
            List<GameInput> inputs = new List<GameInput>();
            foreach (GameInput input in Enum.GetValues(typeof(GameInput))) inputs.Add(input);
            inputs.Sort((x, y) => string.CompareOrdinal(x.ToString(), y.ToString()));
            for (int i = 0; i < inputs.Count; ++i)
            {
                GameInput input = inputs[i];
                if (i >= m_glyphMappingPool.Count)
                    m_glyphMappingPool.Add(Object.Instantiate(m_glyphDebugPrefab, m_debugParent.transform));
                GlyphMappingDebugDisplay display = m_glyphMappingPool[i];
                Texture2D glyph = (EventSystem.current.currentInputModule as HLInputModule).GlyphLookupSystem.Debug_GetGlyphForGameInput(input, requestedType);
                display.SetupDisplay(input.ToString(), glyph);
                display.gameObject.SetActive(true);
            }
        }

        private void HideAllChildren()
        {
            foreach (Transform child in m_debugParent.transform) child.gameObject.SetActive(false);
        }

        private void ToggleDisplayRawInput()
        {
            m_logAllInput = !m_logAllInput;
            m_debugParent.SetActive(m_logAllInput);
            if (!m_logAllInput) HideAllChildren();
        }

        private void Update()
        {
            if (m_logAllInput) LogAllInput();
        }

        private void LogAllInput()
        {
            HideAllChildren();
            string[] joystickNames = Input.GetJoystickNames();
            int index = 0;
            for (int i = 0; i < joystickNames.Length; ++i)
            {
                SetupTextPoolObject(index, "Joystick Attached: " + joystickNames[i]);
                ++index;
            }
            for (int axis = MinimumAxis; axis <= MaximumAxis; ++axis)
            {
                SetupTextPoolObject(index, string.Format("Input Axis {0} value is {1}", axis, Input.GetAxis(string.Format("Joystick 1 Axis {0}", axis))));
                ++index;
            }
            foreach (KeyCode key in (KeyCode[])Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(key))
                {
                    SetupTextPoolObject(index, string.Format("Input Button held is: {0}", key));
                    ++index;
                }
            }
        }

        private void SetupTextPoolObject(int poolIndex, string infoDisplay)
        {
            if (m_textLogPool.Count <= poolIndex)
                m_textLogPool.Add(Object.Instantiate(m_inputLogPrefab, m_debugParent.transform));
            RTLTextMeshPro textObject = m_textLogPool[poolIndex];
            textObject.text = infoDisplay;
            textObject.gameObject.SetActive(true);
        }
    }
}

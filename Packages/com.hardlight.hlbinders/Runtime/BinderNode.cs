// Original HLBinders.Runtime.dll: Hardlight.UI.Binding.BinderNode 0x0200000d.
// Historical source preservation; source presence does not qualify lifecycle, bind references, assets or startup.
// Original true ShowInNodeEditor selects HideInInspector; partial listening is retained on failure.
// Reflection enumerator and as-cast null behavior are retained; runtime fault/source-form parity is a separate gate.
// 0x06000014 get_ShowInNodeEditor: arm64 [0x19f7b20,0x19f7b28); x86_64 [0x19f3b40,0x19f3b50).
// 0x06000015 Refresh: arm64 [0x19f7b28,0x19f7b2c); x86_64 [0x19f3b50,0x19f3b60).
// 0x06000016 Awake: arm64 [0x19f7b2c,0x19f7b78); x86_64 [0x19f3b60,0x19f3bb0).
// 0x06000017 SetUpBindReferenceFields: arm64 [0x19f7b78,0x19f7ee0); x86_64 [0x19f3bb0,0x19f3ee0).
// 0x06000018 OnEnable: arm64 [0x19f7ee0,0x19f8368); x86_64 [0x19f3ee0,0x19f4370).
// 0x06000019 OnDisable: arm64 [0x19f84a4,0x19f85b0); x86_64 [0x19f44b0,0x19f45b0).
// 0x0600001a SetInspectorVisibility: arm64 [0x19f77cc,0x19f77e0); x86_64 [0x19f3820,0x19f3840).
// 0x0600001b OnBeforeSerialize: arm64 [0x19f86d4,0x19f86d8); x86_64 [0x19f46e0,0x19f46f0).
// 0x0600001c OnAfterDeserialize: arm64 [0x19f86d8,0x19f86e4); x86_64 [0x19f46f0,0x19f4700).
// 0x0600001d .ctor: arm64 [0x19f7a88,0x19f7b20); x86_64 [0x19f3ac0,0x19f3b40).
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Hardlight.UI.Binding
{
    [ExecuteInEditMode]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class BinderNode : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private Vector2 nodePosition;
        [SerializeField] private Vector2 pan;
        private Vector2 dynamicNodePosition;
        private Vector2 dynamicPan;
        protected List<BindReference> bindReferences;
        private bool listening;
        private Type bindRefType = typeof(BindReference);

        public virtual bool ShowInNodeEditor { get { return true; } }
        public virtual void Refresh() { }

        protected virtual void Awake()
        {
            SetUpBindReferenceFields();
            hideFlags = ShowInNodeEditor ? HideFlags.HideInInspector : HideFlags.None;
        }

        protected virtual void SetUpBindReferenceFields()
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            bindReferences = new List<BindReference>(10);
            IEnumerator enumerator = fields.GetEnumerator();
            while (enumerator.MoveNext())
            {
                FieldInfo field = enumerator.Current as FieldInfo;
                if (field.FieldType == bindRefType)
                    bindReferences.Add(field.GetValue(this) as BindReference);
            }
        }

        protected virtual void OnEnable()
        {
            try
            {
                if (Application.isPlaying && !listening && bindReferences != null)
                {
                    for (int i = 0; i < bindReferences.Count; i++)
                    {
                        BindReference reference = bindReferences[i];
                        reference.StartListening();
                        reference.OnValueChanged = Refresh;
                    }
                    listening = true;
                }
                hideFlags = ShowInNodeEditor ? HideFlags.HideInInspector : HideFlags.None;
                Refresh();
            }
            catch (Exception exception)
            {
                Debug.LogError(string.Format("Error in binder {0} [{1}]", name, GetType()), gameObject);
                Debug.LogError(exception);
            }
        }

        protected virtual void OnDisable()
        {
            if (Application.isPlaying && listening && bindReferences != null)
            {
                for (int i = 0; i < bindReferences.Count; i++)
                {
                    BindReference reference = bindReferences[i];
                    reference.StopListening();
                    reference.OnValueChanged = null;
                }
                listening = false;
            }
        }

        protected void SetInspectorVisibility(bool visible)
        {
            hideFlags = visible ? HideFlags.None : HideFlags.HideInInspector;
        }

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize()
        {
            dynamicNodePosition = nodePosition;
            dynamicPan = pan;
        }
        protected BinderNode() { }
    }
}

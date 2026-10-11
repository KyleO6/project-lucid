// Original HLBinders.Runtime.dll: Hardlight.UI.Binding.Nodes.StringFormatter 0x02000076.
// Historical source preservation; source presence does not qualify lifecycle, bind references, assets or startup.
// All four argument getters run before the format getter; only a null format becomes empty.
// Genuine BindReference lookup/listening and formatting exceptions remain original provider obligations.
// 0x06000165 Refresh: arm64 [0x1a0d0a8,0x1a0d35c); x86_64 [0x1a08370,0x1a08630).
// 0x06000166 SetUpBindReferenceFields: arm64 [0x1a0d35c,0x1a0d5b0); x86_64 [0x1a08630,0x1a08870).
// 0x06000167 .ctor: arm64 [0x1a0d5b0,0x1a0d8e0); x86_64 [0x1a08870,0x1a08ba0).
using System.Collections.Generic;
using UnityEngine;

namespace Hardlight.UI.Binding.Nodes
{
    [AddComponentMenu("")]
    [AddNodeMenu("Text/String Formatter", int.MaxValue)]
    public class StringFormatter : BinderNode
    {
        [NodeInput] public BindReference String = new BindReference();
        [NodeInput] public BindReference Arg1 = new BindReference();
        [NodeInput] public BindReference Arg2 = new BindReference();
        [NodeInput] public BindReference Arg3 = new BindReference();
        [NodeInput] public BindReference Arg4 = new BindReference();
        [NodeOutput] public readonly Bindable<string> Output = new Bindable<string>();

        public override void Refresh()
        {
            List<object> arguments = new List<object>();
            arguments.Add(Arg1.GetValue());
            arguments.Add(Arg2.GetValue());
            arguments.Add(Arg3.GetValue());
            arguments.Add(Arg4.GetValue());
            Output.Value = string.Format(String.GetValue<string>() ?? string.Empty, arguments.ToArray());
        }

        protected override void SetUpBindReferenceFields()
        {
            List<BindReference> references = new List<BindReference>();
            references.Add(String);
            references.Add(Arg1);
            references.Add(Arg2);
            references.Add(Arg3);
            references.Add(Arg4);
            bindReferences = references;
        }
        public StringFormatter() { }
    }
}

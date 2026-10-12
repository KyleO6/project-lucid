using System.Collections.Generic;
using UnityEngine;

namespace Hardlight.UI.Binding
{
	// Original HLBinders.Runtime 0200000f; bodies translated from the 1.10.1 x86-64 binary.
	public class DictValueBinder<K, V> : MonoBehaviour
	{
		public Bindable<List<V>> Output;

		public Dictionary<K, V> Item
		{
			set
{ } 		}

		// 06000022 (1.10.1 RVA 0xF34B40)
		public DictValueBinder()
{ }
	}
}

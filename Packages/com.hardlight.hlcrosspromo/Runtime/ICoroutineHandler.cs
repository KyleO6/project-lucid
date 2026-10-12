using System.Collections;
using UnityEngine;

namespace Hardlight
{
	// Original HLCrossPromo.Runtime 02000023; bodies translated from the 1.10.1 x86-64 binary.
	public interface ICoroutineHandler
	{
		Coroutine StartCoroutine(IEnumerator downloadFile);
	}
}

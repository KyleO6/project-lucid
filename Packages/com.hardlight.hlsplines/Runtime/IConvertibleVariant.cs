using UnityEngine;
using System;

namespace Hardlight
{
	// Original HLSplines.Runtime 02000094; bodies translated from the 1.10.1 x86-64 binary.
	public interface IConvertibleVariant
	{
		MetadataGroups Metadata
		{
			get;
		}

		void CacheOriginal<T>(T original) where T : Component, IConvertibleBase;

		void DeleteOriginal();

		Component RestoreOriginal();
	}
}

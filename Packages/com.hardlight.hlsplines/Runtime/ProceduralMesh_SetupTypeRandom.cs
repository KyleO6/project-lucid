using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using System;

namespace Hardlight
{
	// Original HLSplines.Runtime 0200003d; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "MeshTypeRandomSetup", menuName = "Hardlight/Procedural/Mesh Type Random Setup", order = 0)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ProceduralMesh_SetupTypeRandom : ScriptableObject
	{
		[SerializeField]
		private Vector3 m_minRotationDeviation;

		[SerializeField]
		private Vector3 m_maxRotationDeviation;

		[SerializeField]
		private float m_minHeightDeviation;

		[SerializeField]
		private float m_maxHeightDeviation;

		[SerializeField]
		private float m_minScale;

		[SerializeField]
		private float m_maxScale;

		[SerializeField]
		private bool m_useParentScale;

		public Vector3 MinRotationDeviation
		{
			get
{ return m_minRotationDeviation; }
		}

		public Vector3 MaxRotationDeviation
		{
			get
{ return m_maxRotationDeviation; }
		}

		public Vector3 RotationDeviationVector
		{
			get
{ return m_maxRotationDeviation - m_minRotationDeviation; }
		}

		public float MinHeightDeviation
		{
			get
{ return m_minHeightDeviation; }
		}

		public float MaxHeightDeviation
		{
			get
{ return m_maxHeightDeviation; }
		}

		public float HeightDeviationRange
		{
			get
{ return m_maxHeightDeviation - m_minHeightDeviation; }
		}

		public float MinScale
		{
			get
{ return m_minScale; }
		}

		public float MaxScale
		{
			get
{ return m_maxScale; }
		}

		public float ScaleRange
		{
			get
{ return m_maxScale - m_minScale; }
		}

		public bool UseParentScale
		{
			get
{ return m_useParentScale; }
		}

		// 0600012a (1.10.1 RVA 0x1A61650)
		public ProceduralMesh_SetupTypeRandom()
		{
			m_minScale = 1f;
			m_maxScale = 1f;
		}
	}
}

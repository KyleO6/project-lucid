using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using System;

namespace Hardlight
{
	// Original HLSplines.Runtime 0200003c; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "MeshTypeGridSetup", menuName = "Hardlight/Procedural/Mesh Type Grid Setup", order = 0)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ProceduralMesh_SetupTypeGrid : ScriptableObject
	{
		[SerializeField]
		private Vector3 m_minRotationDeviation;

		[SerializeField]
		private Vector3 m_maxRotationDeviation;

		[SerializeField]
		private Vector3 m_minPositionDeviation;

		[SerializeField]
		private Vector3 m_maxPositionDeviation;

		[SerializeField]
		private float m_minScale = 1f;

		[SerializeField]
		private float m_maxScale = 1f;

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

		public Vector3 MinPositionDeviation
		{
			get
{ return m_minPositionDeviation; }
		}

		public Vector3 MaxPositionDeviation
		{
			get
{ return m_maxPositionDeviation; }
		}

		public Vector3 PositionDeviationVector
		{
			get
{ return m_maxPositionDeviation - m_minPositionDeviation; }
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

		// 0600011f (1.10.1 RVA 0x1A61580)
		public ProceduralMesh_SetupTypeGrid()
		{
		}
	}
}

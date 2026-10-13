using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000ac4; bodies translated from the 1.10.1 x86-64 binary.
	[SelectionBase]
	public class DreamTeckTrackHelpers : MonoBehaviour
	{
		[SerializeField]
		private Mesh floorMeshBase;

		[SerializeField]
		private int floorMaterialID;

		[SerializeField]
		private int channelCountOverride = -1;

		[SerializeField]
		private Mesh wallMeshBase;

		[SerializeField]
		private int channelCountOverrideWalls = -1;

		[SerializeField]
		private bool generateLeftWall = true;

		[SerializeField]
		private int leftWallLayerOverride = 14;

		[SerializeField]
		private float leftWallOffset = -6.0f;

		[SerializeField]
		private int leftWallMaterialId;

		[SerializeField]
		private bool generateRightWall = true;

		[SerializeField]
		private int rightWallLayerOverride = 14;

		[SerializeField]
		private float rightWallOffset = 6.0f;

		[SerializeField]
		private int rightWallMaterialId;

		[SerializeField]
		private bool generateLeftWallBarrier;

		[SerializeField]
		private float leftWallBarrierOffset = -6.0f;

		[SerializeField]
		private float leftWallBarrierScale = 6.0f;

		[SerializeField]
		private bool generateRightWallBarrier;

		[SerializeField]
		private float rightWallBarrierOffset = 6.0f;

		[SerializeField]
		private float rightWallBarrierScale = 6.0f;

		[SerializeField]
		private Mesh barrierMeshBase;

		[SerializeField]
		private Mesh floorMeshLodLow;

		[SerializeField]
		private Mesh wallMeshLodLow;

		[SerializeField]
		private bool autoGenerateLowestLod = true;

		[SerializeField]
		private int autoLodQuality = 1;

		[SerializeField]
		private float lodDistanceMultiplier = 1.0f;

		// 06003e1a (1.10.1 RVA 0x6E68A0)
		private void Start()
		{
		}

		// 06003e1b (1.10.1 RVA 0x6E68B0)
		private void Update()
		{
		}

		// 06003e1c (1.10.1 RVA 0x6E68C0)
		public DreamTeckTrackHelpers()
		{
		}
	}
}

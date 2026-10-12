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
		private int channelCountOverride;

		[SerializeField]
		private Mesh wallMeshBase;

		[SerializeField]
		private int channelCountOverrideWalls;

		[SerializeField]
		private bool generateLeftWall;

		[SerializeField]
		private int leftWallLayerOverride;

		[SerializeField]
		private float leftWallOffset;

		[SerializeField]
		private int leftWallMaterialId;

		[SerializeField]
		private bool generateRightWall;

		[SerializeField]
		private int rightWallLayerOverride;

		[SerializeField]
		private float rightWallOffset;

		[SerializeField]
		private int rightWallMaterialId;

		[SerializeField]
		private bool generateLeftWallBarrier;

		[SerializeField]
		private float leftWallBarrierOffset;

		[SerializeField]
		private float leftWallBarrierScale;

		[SerializeField]
		private bool generateRightWallBarrier;

		[SerializeField]
		private float rightWallBarrierOffset;

		[SerializeField]
		private float rightWallBarrierScale;

		[SerializeField]
		private Mesh barrierMeshBase;

		[SerializeField]
		private Mesh floorMeshLodLow;

		[SerializeField]
		private Mesh wallMeshLodLow;

		[SerializeField]
		private bool autoGenerateLowestLod;

		[SerializeField]
		private int autoLodQuality;

		[SerializeField]
		private float lodDistanceMultiplier;

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
			channelCountOverride = -1;
			channelCountOverrideWalls = -1;
			generateLeftWall = true;
			leftWallLayerOverride = 14;
			leftWallOffset = -6.0f;
			generateRightWall = true;
			rightWallLayerOverride = 14;
			rightWallOffset = 6.0f;
			leftWallBarrierOffset = -6.0f;
			leftWallBarrierScale = 6.0f;
			rightWallBarrierOffset = 6.0f;
			rightWallBarrierScale = 6.0f;
			autoGenerateLowestLod = true;
			autoLodQuality = 1;
			lodDistanceMultiplier = 1.0f;
		}
	}
}

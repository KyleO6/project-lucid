using Unity.IL2CPP.CompilerServices;

namespace HardlightProject
{
	// Original Game.Runtime 02000a08; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public static class FSMConstants
	{
		public const int StateHistoryMaxCount = 0;

		private const string CharacterMenuRoot = "Character";

		public const string CharacterMenuFormat = "Character/{0}";

		public const string CharacterTriggerMenuFormat = "Character/Trigger/{0}";

		private const string EnemyMenuRoot = "Enemy";

		public const string EnemyMovementMenuFormat = "Enemy/Movement/{0}";

		public const string EnemyBrainMenuFormat = "Enemy/Brain/{0}";

		private const string BossMenuRoot = "Boss";

		public const string BossMenuFormat = "Boss/{0}";

		public const string BossFourmenMenuFormat = "Boss/Fourmen/{0}";

		public const string BossCrabMenuFormat = "Boss/Crab/{0}";

		public const string BossGuardianMenuFormat = "Boss/Guardian/{0}";

		private const string ApplicationMenuRoot = "Application";

		public const string ApplicationMenuFormat = "Application/{0}";

		public const string ApplicationSystemManagementMenuFormat = "Application/Systems/{0}";

		public const string ApplicationMissionsMenuFormat = "Application/Missions/{0}";

		public const string ApplicationLevelsMenuFormat = "Application/Levels/{0}";

		public const string ApplicationLoadingMenuFormat = "Application/Loading/{0}";

		public const string ApplicationMetaGameMenuFormat = "Application/MetaGame/{0}";

		public const string ApplicationFTUEMenuFormat = "Application/FTUE/{0}";

		public const string ApplicationTheatreMenuFormat = "Application/Theatre/{0}";

		private const string ActorMenuRoot = "Actor";

		public const string ActorMenuFormat = "Actor/{0}";

		private const string GenericMenuRoot = "Generic";

		public const string GenericMenuFormat = "Generic/{0}";

		private const string ShaderVariantMenuRoot = "ShaderVariant";

		public const string ShaderVariantMenuFormat = "ShaderVariant/{0}";
	}
}

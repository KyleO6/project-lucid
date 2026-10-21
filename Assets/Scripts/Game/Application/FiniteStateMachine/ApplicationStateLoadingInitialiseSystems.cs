using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000144; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[GraphNodeMenuFormat("Application/Loading/{0}")]
	public class ApplicationStateLoadingInitialiseSystems : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityObject(typeof(ApplicationStateEvent))]
			public string EventId;

			// 060007ca (1.10.1 RVA 0x65AF20)
			public JSONCtorArgs()
			{;}
		}

		private readonly ApplicationStateEvent m_stateEvent;

		// The system reference is a field initializer, so it is acquired before base registration.
		private readonly SystemRef<SaveManager> m_saveManagerRef = ProcessManager.GetSystemRef<SaveManager>();

		// 060007c6 (1.10.1 RVA 0x65A8E0); the base registration precedes the event store.
		private ApplicationStateLoadingInitialiseSystems(FiniteStateMachine fsm, FSMIdentifier stateId, ApplicationStateEvent stateEvent) : base(fsm, stateId)
		{
			m_stateEvent = stateEvent;
		}

		// 060007c7 (1.10.1 RVA 0x65A9A0); the event is resolved by name before allocation.
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			ApplicationStateEvent stateEvent = ApplicationStateEvent.FindByName(args.EventId);
			return new ApplicationStateLoadingInitialiseSystems(fsm, stateId, stateEvent);
		}

		// 060007c8 (1.10.1 RVA 0x65AA40). The level's managed systems are added in this
		// order; the event is published only once the CharacterManager reports itself
		// initialised.
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			LevelManagerSystems systems = ProcessManager.GetSystem<LevelManager>().ManagedSystems;
			ProceduralMesh_PlacementManager placementManager = new ProceduralMesh_PlacementManager();
			systems.AddManagedSystem(placementManager);
			ProcessManager.UnregisterSystem<ProcMeshPlacementUpdater>();
			ProcessManager.RegisterSystem(new ProcMeshPlacementUpdater(placementManager));
			systems.AddManagedSystem<SimplifiedBodyManager>();
			systems.AddManagedSystem<CollectableManager>();
			systems.AddManagedSystem<EnemyManager>();
			systems.AddManagedSystem<MessageManager>();
			systems.AddManagedSystem<GameplayIslandManager>();
			systems.AddManagedSystem<EntityActivationManager>();
			systems.AddManagedSystem<LevelRespawnPoints>();
			systems.AddManagedSystem(new CharacterManager(() => PublishMessage(user)));
			if (m_saveManagerRef.TryGet(out SaveManager saveManager))
			{
				saveManager.GameIsPlaying = true;
			}
		}

		// 060007c9 (1.10.1 RVA 0x65AD80); publishes into the entering user's storage.
		private void PublishMessage(IGraphUser user)
		{
			user.Storage.GetValue<List<ApplicationStateEvent>>(AppFSMKeys.StateEvents, null, true).AddUnique(m_stateEvent);
		}
	}
}

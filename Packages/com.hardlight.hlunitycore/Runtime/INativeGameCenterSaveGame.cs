using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 02000138; bodies translated from the 1.10.1 x86-64 binary.
	public interface INativeGameCenterSaveGame
	{
		bool IsAvailable();

		void Initialise(string gameObjectName, string separator);

		void Deinitialise();

		void FetchSavedGames(int hashCode);

		void SaveGameData(int hashCode, string saveGameName, string saveGameData);

		void LoadGameData(int hashCode, string propertiesJoinToIdentifySavedGameToLoad);

		void DeleteSavedGamesWithName(int hashCode, string saveGameName);

		void ResolveConflictingSavedGames(int hashCode, string conflictingSavedGames, string saveGameData);
	}
}

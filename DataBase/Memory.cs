using The_Final_Battle.Characters;
using The_Final_Battle.Characters.CharacterAILogic;
using The_Final_Battle.Gameplay.Battle;

namespace The_Final_Battle.DataBase
{
	public static class GlobalVariables
	{
		public static readonly string BasePath = Path.Combine(AppContext.BaseDirectory, @"..\..\..");
	}

	public static class Memory
	{
		public static DataBase? DataBase { get; private set; }

		public static void Start() 
		{
			DataBase ??= new();

			
			DataBase.Start();
			
		}
	}


	public static class Game
	{
		public static void Start(int difficulty, int gameMode, CombatGroup heroes)
		{
			EnemiesWave combat;

			switch (difficulty)
			{
				case 1:
					combat = Memory.DataBase.Get<EnemiesWave>("Main Combat");

					break;

				case 2:
					combat = Memory.DataBase.Get<EnemiesWave>("Main Combat");

					break;


				case 3:
					combat = Memory.DataBase.Get<EnemiesWave>("Main Combat");

					break;

				default:
					throw new ArgumentOutOfRangeException($"Error: Chosed difficulty not support. " +
						$"(Game.Start({difficulty}))");
			}


			BattleMiniGame battleMiniGame = new();
			switch (gameMode)
			{
				case 1:
					battleMiniGame.Run(heroes, combat);

					break;


				case 2:
					foreach (CombatGroup monsters in combat.Enemies)
						InjectLogic(monsters);

					battleMiniGame.Run(heroes, combat);

					break;


				case 3:
					foreach (CombatGroup monsters in combat.Enemies)
						InjectLogic(monsters);
					InjectLogic(heroes);

					battleMiniGame.Run(heroes, combat);

					break;
			}



			void InjectLogic(CombatGroup characters)
			{
				for (int i = 0; i < characters.Count; i++)
				{
					CharacterAI monster = new(characters[i], AILogic.DefaultLogic);
					characters[i] = monster;
				}
			}
		}
	}
}

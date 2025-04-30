using The_Final_Battle.DataBase;
using The_Final_Battle.Miscellaneous;
using The_Final_Battle.Characters;
using The_Final_Battle.Gameplay.Battle;
using The_Final_Battle.Interfaces.Gameplay.Inventary;



Memory.Start();


string gameModeMessage = "Game mode:\n" +
				"\t1 - Player VS Player\n" +
				"\t2 - Player VS AI\n" +
				"\t3 - AI VS AI\n\n" +
"Choose a Game mode: ";

int gameMode = BaseMethods.AskForNumberInRange(gameModeMessage, 1, 4);
Console.Clear();


string difficultyLMessage = "Difficulty level:\n" +
				"\t1 - Easy\n" +
				"\t2 - Normal\n" +
				"\t3 - Hard\n\n" +
"Choose a difficulty: ";

int difficulty = BaseMethods.AskForNumberInRange(difficultyLMessage, 1, 4);
Console.Clear();


CombatGroup heroes = [
	Memory.DataBase.Get<Character>("Tog"),
	Memory.DataBase.Get<Character>("Vin Fletcher")
	];


heroes.Items.Add(Memory.DataBase.Get<IItem>("Healing Potion"));
heroes.Items.Add(Memory.DataBase.Get<IItem>("Sword"));
heroes.Items.Add(Memory.DataBase.Get<IItem>("Dagger"));

Game.Start(difficulty, gameMode, heroes);
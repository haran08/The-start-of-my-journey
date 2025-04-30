using The_Final_Battle.Characters;
using The_Final_Battle.Gameplay.Battle.Enum;
using The_Final_Battle.Interfaces.Characters;
using The_Final_Battle.Interfaces.Gameplay.Inventary;
using The_Final_Battle.UI;

namespace The_Final_Battle.Gameplay.Battle
{
	public class BattleMiniGame
	{
		public EnemiesWave?				MonstersWave { get; set; }
		public CombatGroup?				Heroes		 { get; set; }
		public RenderBattle				View		 { get; }	   = new();
		public (TurnTypes, ICharacter?) CurrectTurn  { get; set; }

		public event Action?			EndTurnEvent;



		/// <summary>
		/// Run the combat logic.
		/// </summary>
		public void Run(CombatGroup heroes, EnemiesWave enemies)
		{
			Heroes		 = heroes;
			MonstersWave = enemies;
			CombatGroup monsters = MonstersWave.Enemies.First();

			while (true)
			{
				View.ShowWave();
				while (true)
				{
					View.ShowRound();

					CurrectTurn = (TurnTypes.Heroes, null);
					RunTurnsLogic(Heroes!);
					RemoveDeadCharacters(monsters);

					if (TryEndWave(monsters, out bool _))
						break;

					CurrectTurn = (TurnTypes.Monsters, null);
					RunTurnsLogic(monsters);
					RemoveDeadCharacters(monsters);

					TryEndWave(monsters, out bool heroesDefeated);
					if (heroesDefeated)
						break;

					View.ShowRound(false);
				}

				if (CheckWin()) break;

				monsters = MonstersWave.Enemies.First();
			}
		}


		/// <summary>  
		/// Determines if the current faction has achieved victory conditions during their turn. 
		/// </summary>  
		/// <param name="turn">The current turn type, specifying whether it's the heroes' or monsters' turn.</param>  
		/// <returns>True if the victory conditions for the current faction are met; otherwise, false.</returns> 
		private bool CheckWin()
		{
			switch (CurrectTurn.Item1)
			{
				case TurnTypes.Heroes:
					if (MonstersWave.Enemies.Count == 0)
					{
						Console.WriteLine("The heroes won and the Uncoded One was defeated!");
						return true;
					}
					break;

				case TurnTypes.Monsters:
					if (Heroes.Count == 0)
					{
						Console.WriteLine("The heroes lost and the Uncoded One’s forces have prevailed!");
						return true;
					}
					break;
			}

			return false;
		}


		private bool TryEndWave(CombatGroup monsters, out bool heroesDefeated)
		{
			heroesDefeated	 = false;

			if (monsters.Count == 0)
			{
				StolenInventory(monsters, Heroes!);
				MonstersWave.Enemies.RemoveAt(0);

				View.Round = 0;
				RenderBattle.ShowWinningMessage_Wave();
				return true;
			}

			if (Heroes.Count == 0)
				heroesDefeated = true;

			return false;
		}

		/// <summary>
		/// Manages the execution of game actions for each character in the provided combat group,  
		/// triggers the end-of-turn event, and removes deceased characters from the current turn.  
		/// </summary>
		/// <param name="characters">The combat group whose characters will take their turns.</param>
		private void RunTurnsLogic(CombatGroup characters)
		{
			View.SetupInventoryTurnRender(characters.Items);

			for (int i = 0; i < characters.Count; i++)
			{
				CurrectTurn = (CurrectTurn.Item1, characters[i]);
				RenderBattle.CombatRender(this);

				switch (characters[i])
				{
					case Character ch:
						if (!View.HandlePLayerTurn(this, ch, out string usedActionName)) 
						{
							List<Delegate> events = EndTurnEvent.GetInvocationList().ToList();

							if (View.HasUsedInventory!.Last() is true) 
							{
								View.InventoryCopy.Add(characters.Items[View.ItemsChoosed!.Last()]);
								View.ItemsChoosed.RemoveAt(View.ItemsChoosed.Count - 1);
								View.HasUsedInventory.RemoveAt(View.HasUsedInventory.Count - 1);
							}

							events.RemoveAt(events.Count - 1);

							EndTurnEvent = null;
							events.ForEach(d => EndTurnEvent += (Action)d);

							i -= 2;
						}


						View.PrintChosedAction(ch, usedActionName);
						Thread.Sleep(475);
						Console.Clear();
						break;

					case CharacterAI ch:
						Thread.Sleep(1290);

						(Action, string) ActionAI = ch.Logic(this);

						EndTurnEvent += ActionAI.Item1;
						View.PrintChosedAction(ch, ActionAI.Item2);
						Thread.Sleep(1290);
						Console.Clear();

						break;
				}

			}


			EndTurnEvent?.Invoke();
			EndTurnEvent = null;
		}


		/// <summary>  
		/// Removes characters with zero or less health.  
		/// Notifies when a character is removed due to death.  
		/// </summary>  
		/// <param name="Monsters">The Currect Group of "Monsters" in combat</param> 
		private void RemoveDeadCharacters(CombatGroup monsters)
		{
			string heroesDeath = "";
			string monstersDeath = "";
			List<int> toRemove = [];

			for (int i = 0; i < monsters.Count; i++)
			{
				if (monsters[i].CurrectHealth <= 0)
				{
					monstersDeath += $"{monsters[i].Name} has been defeated!\n";

					monsters[i].Weapon?.Unequip(monsters.Items);
					toRemove.Add(i);
				}
			}
			RemoveDeads(monsters);

			for (int i = 0; i < Heroes.Count; i++)
			{
				if (Heroes[i].CurrectHealth <= 0)
				{
					heroesDeath += $"{Heroes[i].Name} has been defeated!\n";

					Heroes[i].Weapon?.Unequip(Heroes.Items);
					toRemove.Add(i);
				}
			}
			RemoveDeads(Heroes);

			if (monstersDeath != "") Console.WriteLine(monstersDeath);
			if (heroesDeath != "") Console.WriteLine(heroesDeath);


			void RemoveDeads(CombatGroup characters)
			{
				int _ = 0;
				foreach (int i in toRemove)
				{
					characters.RemoveAt(i - _++);
				}

				toRemove.Clear();
			}
		}

		
		private static void StolenInventory(CombatGroup stoled, CombatGroup receiver)
		{
			foreach (IItem item in stoled.Items)
			{
				Console.WriteLine($"{item.Name} taked from the dead bodies.");
				receiver.Items.Add(item);
			}

			stoled.Items.Clear();
		}
	}
}

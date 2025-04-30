using The_Final_Battle.Miscellaneous;
using The_Final_Battle.Characters;
using The_Final_Battle.Gameplay;
using The_Final_Battle.Gameplay.Battle;
using The_Final_Battle.Gameplay.Inventary;
using The_Final_Battle.GameSystems;
using The_Final_Battle.Interfaces.Characters;
using The_Final_Battle.Interfaces.Gameplay.Inventary;

namespace The_Final_Battle.UI
{
	public class RenderBattle
	{
		public List<IItem>?	InventoryCopy;
		public List<int>?	ItemsChoosed;
		public List<bool>?	HasUsedInventory;
		public int			Wave,Round;



		public static void CombatRender(BattleMiniGame combat)
		{
			string sprite1 = 
				"============================================= BATTLE ============================================";
			string sprite2 =
				"---------------------------------------------- VS -----------------------------------------------";


			Console.WriteLine(sprite1);
			RenderCharacters(combat.Heroes!, combat.CurrectTurn.Item2!);
			Console.WriteLine(sprite2);
			RenderCharacters(combat.MonstersWave.Enemies[0], combat.CurrectTurn.Item2!, true);
			Console.WriteLine(sprite1);


			void RenderCharacters(CombatGroup characters, ICharacter currectTurn, bool isMonsters = false)
			{
				foreach (ICharacter ch in characters)
				{
					if (ch == currectTurn)
					{
						Console.ForegroundColor = ConsoleColor.DarkYellow;
						PrintCharacter(ch);
						Console.ForegroundColor = ConsoleColor.White;
					}

					else
					{
						PrintCharacter(ch);
					}
				}

				void PrintCharacter(ICharacter ch)
				{
					if (isMonsters)
					{
						string _;
						if (ch.Weapon != null)
							_ = $"({ch.Weapon.Name}) - ({ch.CurrectHealth}/{ch.MaxHealth})     {ch.Name}";
						else
							_ = $"({ch.CurrectHealth}/{ch.MaxHealth})     {ch.Name}";
						int i = sprite1.Length - _.Length;
						Console.WriteLine($"{new string(' ', i)}{_}");
					}
					else
					{
						if (ch.Weapon != null)
							Console.WriteLine($"{ch.Name}     ({ch.CurrectHealth}/{ch.MaxHealth}) - ({ch.Weapon.Name})");
						else
							Console.WriteLine($"{ch.Name}     ({ch.CurrectHealth}/{ch.MaxHealth})");
					}
				}
			}
		}


		enum Menus : byte
		{
			Skills = 1,
			Inventory,
			Return
		}
		public bool HandlePLayerTurn(BattleMiniGame combat, Character ch, out string usedActionName)
		{
			while (true)
			{
				int choose = RenderOptions();

				switch ((Menus)choose)
				{
					case Menus.Skills:
						int skill = ActionMethods.ActionHandleMenuSelection(ch.Skills, combat, this);

						if (skill == 0)
						{
							Console.Clear();
							CombatRender(combat);
							continue;
						}

						HasUsedInventory.Add(false);
						usedActionName = ch.Skills[skill - 1].Name;
						return true;

					case Menus.Inventory:
						TargetSystem.SelectTargetGroup(combat, combat.CurrectTurn.Item1, out CombatGroup group);

						int item = InventoryMethods.ItemHandleMenuSelection(group.Items, combat, this);

						if (item == 0)
						{
							Console.Clear();
							CombatRender(combat);
							continue;
						}

						InventoryCopy.RemoveAt(item - 1);
						ItemsChoosed.Add(item - 1);
						HasUsedInventory.Add(true);
						usedActionName = group.Items[item - 1].Name;
						return true;

					case Menus.Return:
						usedActionName = "";
						return false;
				}

				Console.Clear();
				CombatRender(combat);
			}


			int RenderOptions()
			{
				if (ch == combat.Heroes[0] || ch == combat.MonstersWave.Enemies[0][0])
				{
					Console.WriteLine(
						$"{(int)Menus.Skills} - Skills\n" +
						$"{(int)Menus.Inventory} - Inventory\n"
					);

					return BaseMethods.AskForNumberInRange("What do you want to do? ", 1, 3, true);
				}

				else
				{
					Console.WriteLine(
						$"{(int)Menus.Skills} - Skills\n" +
						$"{(int)Menus.Inventory} - Inventory\n" +
						$"{(int)Menus.Return} - Return"
					);

					return BaseMethods.AskForNumberInRange("What do you want to do? ", 1, 4, true);
				}
			}
		}


		public int HandleMenuSelection<T>(List<T> options, Func<T, bool> endLogic,
			string prompt = "Select option: ", bool showCancel = true, bool isInventory = false)
		{
			while (true)
			{
				int startOption = showCancel ? 0 : 1;
				int selection;

				if (isInventory)
				{
					if (showCancel)
						Console.WriteLine(InventoryCopy.ToFormatedListString(o => o.ToString(), "Cancel"));

					else
						Console.WriteLine(InventoryCopy.ToFormatedListString(o => o.ToString()));

					selection = BaseMethods.AskForNumberInRange(prompt, startOption, InventoryCopy.Count + 1);
				}

				else
				{
					if (showCancel)
						Console.WriteLine(options.ToFormatedListString(o => o.ToString(), "Cancel"));

					else
						Console.WriteLine(options.ToFormatedListString(o => o.ToString()));

					selection = BaseMethods.AskForNumberInRange(prompt, startOption, options.Count + 1);
				}


				if (showCancel && selection == 0) return 0;


				bool logicResult = endLogic(options[selection - 1]);
				if (!logicResult) continue;


				return selection;
			}
		}


		public void PrintChosedAction(ICharacter ch, string action)
		{
			Console.WriteLine($"{ch.Name} used {action}");
		}


		/// <summary>
		/// Add one to Wave and print a string with him.
		/// </summary>
		public void ShowWave()
		{
			Console.WriteLine($"Wave {++Wave}");
		}


		/// <summary>
		/// Add one to Round and print a string with him. Start param choose if the message is about the start of 
		/// the round or his end.
		/// </summary>
		public void ShowRound(bool start = true)
		{
			if (start)
				Console.WriteLine($"Round {++Round} begins!");
			else
				Console.WriteLine($"Round {Round} ends!\n");
		}


		public static void ShowWinningMessage_Wave()
		{
			Console.WriteLine("The heroes won again this Wave!");
		}


		public static void ShowDefeatMessage()
		{
			Console.WriteLine("The Uncoded One Continue to be undefeated.");
		}


		public static void ShowEquipingMessage(ICharacter ch, Weapon gear)
		{
			Console.WriteLine($"{ch.Name} equiped {gear.Name}");
		}


		public void SetupInventoryTurnRender(List<IItem> inventory)
		{
			InventoryCopy = [];
			ItemsChoosed  = [];
			HasUsedInventory	  = [];
			inventory.ForEach(i => InventoryCopy.Add(i.Copy()));
		}


	}
}

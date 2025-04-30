using The_Final_Battle.Gameplay.Battle;
using The_Final_Battle.GameSystems;
using The_Final_Battle.Interfaces.Gameplay.Actions;
using The_Final_Battle.Interfaces.Gameplay.Inventary;
using The_Final_Battle.UI;

namespace The_Final_Battle.Gameplay.Inventary
{
	public class InventoryMethods
	{
		public static int ItemHandleMenuSelection(List<IItem> inventory, BattleMiniGame combat, RenderBattle render)
		{
			return render.HandleMenuSelection(
				inventory,
				ItemHandle,
				isInventory: true
			);


			bool ItemHandle(IItem item)
			{
				switch (item)
				{
					case IAction action:
						if (!TargetSystem.TryChoseTarget(combat, action)) return false;

						combat.EndTurnEvent += () => { 
							action.Execute();
							inventory.Remove(item);
						};
						return true;

					case Weapon gear:

						combat.EndTurnEvent += () => {
							combat.CurrectTurn.Item2.Weapon?.Unequip(inventory);
							gear.Equip(combat.CurrectTurn.Item2!);
							inventory.Remove(item);
							RenderBattle.ShowEquipingMessage(combat.CurrectTurn.Item2, gear);
						};
						return true;

					default:
						throw new NotImplementedException($"Error: unsuported item type '{item.GetType().Name}'. " +
							$"(InterfaceSystem.InventorySelectionAndHandle.ItemHandle({item.GetType().Name}))");
				}
			}
		}
	}
}

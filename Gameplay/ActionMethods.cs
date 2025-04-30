using The_Final_Battle.Gameplay.Battle;
using The_Final_Battle.GameSystems;
using The_Final_Battle.Interfaces.Gameplay.Actions;
using The_Final_Battle.UI;

namespace The_Final_Battle.Gameplay
{
	public static class ActionMethods
	{

		public static int ActionHandleMenuSelection<T>(List<T> skills, BattleMiniGame combat, RenderBattle render)
			where T : IAction
		{
			return render.HandleMenuSelection(
				skills,
				ActionHandle
			);


			bool ActionHandle(T skill)
			{
				if (!TargetSystem.TryChoseTarget(combat, skill)) return false;

				combat.EndTurnEvent += skill.Execute;
				return true;
			}
		}
		
	}
}

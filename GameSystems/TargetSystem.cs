using The_Final_Battle.Gameplay.Battle;
using The_Final_Battle.Gameplay.Battle.Enum;
using The_Final_Battle.Gameplay.Effects;
using The_Final_Battle.Gameplay.Skills.Enum;
using The_Final_Battle.Interfaces.Characters;
using The_Final_Battle.Interfaces.Gameplay.Actions;
using The_Final_Battle.Miscellaneous;
using The_Final_Battle.UI;


namespace The_Final_Battle.GameSystems
{
	public static class TargetSystem
	{
		public static bool TryChoseTarget(BattleMiniGame combat, IAction gameAction)
		{
			try
			{
				if (gameAction == null)
				{
					throw new NullReferenceException("ERROR: Combat or gameAction are null. (ManangeGameAction)");
				}

				switch (gameAction.Effect.Param)
				{

					case TargetParam param:
						ICharacter Target;

						if (gameAction.TargetTypeEnum == TargetTypeEnum.Self)
						{
							Target = combat.CurrectTurn.Item2 ??
								throw new NullReferenceException(
								"ERROR: Self Action can't receive a null target. (ManangeGameAction.ISingleTargetAction)"
							);
						}

						else
						{
							CombatGroup targetGroup = SelectTargetGroup(combat, gameAction.TargetTypeEnum);

							if (targetGroup.Count == 0) Console.WriteLine("No targets available.");

							Console.Write(targetGroup.ToListString("Cancel"));


							if (!TrySelectTarget(targetGroup, out ICharacter? target)) {
								Console.Clear();
								RenderBattle.CombatRender(combat);
								return false;
							}

							Target = target!;
						}

						param.Target = Target;

						return true;

					default:
						throw new NotImplementedException("Error: GameAction interface not implemented. " +
														  "(ManangeGameAction)");
				}
			}

			catch (NullReferenceException e)
			{
				Console.WriteLine(e.Message);
			}

			catch (NotImplementedException e)
			{
				Console.WriteLine(e.Message);
			}

			return false;
		}


		/// <summary>
		/// Retrieves the appropriate <see cref="CombatGroup"/> in a combat based on whether the
		/// gameAction targets allies or enemies and the cast group.
		/// </summary>
		/// <param name="combat">The current activeBattle instance.</param>
		/// <param name="targetAllies">A boolean indicating whether to target allied units.</param>
		/// <returns>The combat group to be targeted.</returns>
		/// <exception cref="NullReferenceException">Thrown when targeting allies but the
		/// hero group is null.</exception>
		private static CombatGroup SelectTargetGroup(BattleMiniGame combat, TargetTypeEnum targetAllies)
		{
			return targetAllies switch
			{
				TargetTypeEnum.Enemy => combat.CurrectTurn.Item1 == TurnTypes.Heroes ? combat.MonstersWave.Enemies[0] :
																						combat.Heroes!,

				TargetTypeEnum.Ally => combat.CurrectTurn.Item1 == TurnTypes.Heroes ? combat.Heroes! :
																						combat.MonstersWave.Enemies[0],

				_ => throw new NotImplementedException(
					"Error: TargetType not implemented (TargetMethods.SelectTargetGroup)"
				)
			};
		}

		public static CombatGroup SelectTargetGroup(BattleMiniGame combat, TurnTypes turn, out CombatGroup allies)
		{
			switch (turn)
			{
				case TurnTypes.Monsters:
					allies = combat.MonstersWave.Enemies.First();
					return combat.Heroes!;

				case TurnTypes.Heroes:
					allies = combat.Heroes!;
					return combat.MonstersWave.Enemies.First();

				default:
					throw new NotImplementedException(
						"Error: TurnType not implemented. " +
						"(TargetMethods.SelectTargetGroup.default)"
					);
			}
		}

		private static List<ICharacter> AskForTarget(CombatGroup targetGroup, int maxTargets)
		{
			List<int> targets = BaseMethods.AskForNumber("Select a target: ", maxTargets);
			List<ICharacter> targetCharacters = [];

			foreach (int t in targets)
			{
				targetCharacters.Add(targetGroup[t]);
			}

			return targetCharacters;
		}

		private static bool TrySelectTarget(CombatGroup targetGroup, out ICharacter? target)
		{
			int index = BaseMethods.AskForNumberInRange("Select a target: ", 0,
				targetGroup.Count + 1, true);

			if (index == 0) {
				target = null;
				return false;
			}

			target = targetGroup[index - 1];
			return true;
		}
	}
}
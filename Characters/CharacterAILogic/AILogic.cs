using The_Final_Battle.Interfaces.Characters;
using The_Final_Battle.Interfaces.Gameplay.Skills;
using The_Final_Battle.Gameplay.Battle;
using The_Final_Battle.Gameplay.Effects;
using The_Final_Battle.Gameplay.Skills.Enum;
using The_Final_Battle.GameSystems;
using The_Final_Battle.Interfaces.Gameplay.Actions;
using The_Final_Battle.Gameplay.Inventary;
using The_Final_Battle.Interfaces.Gameplay.Inventary;

namespace The_Final_Battle.Characters.CharacterAILogic
{
	public static class AILogic
	{
		public static (Action, string) DefaultLogic(BattleMiniGame combat)
		{
			CombatGroup Targets = TargetSystem.SelectTargetGroup(combat, combat.CurrectTurn.Item1, 
				out CombatGroup allies);
			CombatGroup Allies  = allies;
			ICharacter  self    = combat.CurrectTurn.Item2 ?? throw new NullReferenceException(
						"Error/Return: CurrectTurn has a null Character. (AILogic.DefaultLogic)"
						);
			float DamagePoint  = DamageScore(Targets, self.Skills, out (IAction, ICharacter) dAction);
			float HealingPoint = HealingScore(Allies, self.Skills, self, out (IAction, ICharacter) hAction);
			float GearPoint	   = GearScore(allies.Items, self, out Weapon? equip);
			(IAction, ICharacter) ToExecute = default;


			if (DamagePoint == 0 && HealingPoint == 0)
			{
				foreach (IAction action in self.Skills)
				{
					if (action.Effect.GetType() == typeof(SkipTurnEffect))
					{
						ToExecute = (action, self);
						break;
					}
				}
			}

			else
			{
				if (DamagePoint > HealingPoint && DamagePoint > GearPoint) {
					ToExecute = dAction;
				}

				else if (HealingPoint > DamagePoint && HealingPoint > GearPoint) {
					ToExecute = hAction;
				}

				else if (GearPoint > DamagePoint && GearPoint > HealingPoint)
				{
					Allies.Items.Remove(equip!);

					return ( () => equip.Equip(self) , equip.Name);
				}

				else
				{
					ToExecute = dAction;
				}
			}

			if (ToExecute.Item1 == null) {
				throw new InvalidOperationException("Error: No action to execute. (AILogic.DefaultLogic)");
			}


			string name = ToExecute.Item1 switch {
				ISkill s => s.Name,
				IItem  i => i.Name,
				_ => ""
			};

			switch (ToExecute.Item1.Effect.Param)
			{
				case TargetParam param:
					param.Target = ToExecute.Item2;
					break;

				default:
					throw new NotImplementedException("Error: TurnType not implemented. " +
						"(AILogic.DefaultLogic.default)");
			}



			if (ToExecute.Item1 is Consumable c)
				Allies.Items.Remove(c);

			return (ToExecute.Item1.Execute, name);
		}



		private static float DamageScore(CombatGroup targets, List<ISkill> gameActions,
			out (IAction, ICharacter) dAction)
		{
			float score		 = 0;
			int	  killsBonus = 0;
			bool  kills		 = false;
			dAction			 = default;

			if (gameActions.Count == 0 || gameActions == null)
				throw new InvalidOperationException("Error:No available game actions to execute. (AILogic.DamageScore)");

			foreach (IAction Action in gameActions)
			{
				if (!IsAttackAction(Action)) continue;

				switch (Action.Effect.Param)
				{
					case TargetParam param:
						foreach (ICharacter character in targets)
						{
							bool checkDeath = param.HealthParam > character.CurrectHealth;

							if (!kills)
							{
								if (checkDeath)
								{
									dAction.Item1 = Action;
									dAction.Item2 = character;
									score = character.CurrectHealth + 10;
									kills = true;
									killsBonus += 20;
									break;
								}

								else if (param.HealthParam > score)
								{
									dAction.Item1 = Action;
									dAction.Item2 = character;
									score = param.HealthParam;
								}	
							}

							else if (checkDeath)
							{
								if (character.CurrectHealth > score)
								{
									dAction.Item1 = Action;
									dAction.Item2 = character;
									score = character.CurrectHealth + 10;
								}
							}
						}
						break;

					default:
						throw new NotImplementedException("Error: GameActionType not implemented. (AILogic.DefaultLogic)");
				}


				static bool IsAttackAction(IAction action)
				{
					if (action.Effect.ActionTypeEnum != GameActionTypeEnum.Attack) return false;

					return true;
				}
			}

			return score + killsBonus;
		}


		private static float HealingScore(CombatGroup allies, List<ISkill> gameActions, ICharacter self,
			out (IAction, ICharacter) hAction)
		{
			float score   = 0;
			hAction		  = default;

			if (IsAlliesFullHealth()) return score;

			if (gameActions.Count == 0 || gameActions == null)
				throw new InvalidOperationException("Error:No available game actions to execute. " +
					"(AILogic.HealingScore)");


			foreach(IAction action in gameActions)
			{
				if (!IsHealingAction(action)) continue;

				switch(action.Effect)
				{
					case TargetParam tp:

						if (ScoreForTargetParam(action, tp, out (IAction, ICharacter) scoreAction))
							hAction = scoreAction;
						break;
				}
			}

			foreach (IItem item in allies.Items)
			{
				if (item is not Consumable consumable) continue;

				if (!IsHealingAction(consumable)) continue;

				switch (consumable.Effect.Param)
				{
					case TargetParam tp:

						if (ScoreForTargetParam(consumable, tp, out (IAction, ICharacter) scoreAction))
							hAction = scoreAction;
						break;
				}
			}

			return score;


			bool ScoreForTargetParam(IAction action, TargetParam tp, out (IAction, ICharacter) scoreAction)
			{
				int lowHealth = 0;
				scoreAction = default;
				if (tp.HealthParam <= score) return false;

				switch (action.TargetTypeEnum)
				{
					case TargetTypeEnum.Self:
						bool wastedHealth_Self = tp.HealthParam > self.CurrectHealth;

						if ((float)self.MaxHealth / 3 >= self.CurrectHealth)
						{
							if (new Random().NextDouble() <= 0.25)
							{
								lowHealth += 9;
								if (wastedHealth_Self) {
									score = tp.HealthParam - self.CurrectHealth + lowHealth;
									scoreAction.Item1 = action;
									scoreAction.Item2 = self;
									return true;
								}
								else {
									score = tp.HealthParam + lowHealth;
									scoreAction.Item1 = action;
									scoreAction.Item2 = self;
									return true;
								}
							}
						}
						break;

					case TargetTypeEnum.Ally:
						foreach(ICharacter character in allies)
						{
							int correctHParam = tp.HealthParam;
							bool wastedHealth = tp.HealthParam > character.CurrectHealth;
							if (wastedHealth) correctHParam = tp.HealthParam - character.CurrectHealth;

							if ((float)character.MaxHealth / 3 >= character.CurrectHealth &&
								lowHealth == 0)
									lowHealth += 9;

							if (wastedHealth && correctHParam + lowHealth > score)
							{
								score = character.CurrectHealth + lowHealth;
								scoreAction.Item1 = action;
								scoreAction.Item2 = self;
								return true;
							}
							else if (tp.HealthParam + lowHealth > score)
							{
								score = tp.HealthParam + lowHealth;
								scoreAction.Item1 = action;
								scoreAction.Item2 = self;
								return true;
							}
						}
						break;

					default:
						throw new NotImplementedException("Error: GameActionType not implemented. " +
							"(AILogic.DefaultLogic)");
				}

				return false;
			}

			bool IsHealingAction(IAction action)
			{
				if (action.Effect.ActionTypeEnum != GameActionTypeEnum.Heal) return false;

				return true;
			}


			bool IsAlliesFullHealth()
			{
				foreach (ICharacter character in allies)
					if (character.CurrectHealth != character.MaxHealth) return false;

				return true;
			}
		}


		private static float GearScore(List<IItem> inventory, ICharacter self, out Weapon? equip)
		{
			float score = 0;
			equip = default;

			foreach(IItem item in inventory)
			{
				if (item is Weapon gear)
				{
					if (self.Weapon is null)
					{
						int skillScore = GearSkillsScore(gear.Skills) + 6;
						if (score < skillScore)
						{
							score = skillScore;
							equip = gear;
						}
					}

					else
					{
						if (gear.Name == self.Weapon.Name) continue;

						int skillScore = GearSkillsScore(gear.Skills);
						if (score < skillScore)
						{
							score = skillScore;
							equip = gear;
						}
					}
				}
			}

			return score;



			int GearSkillsScore(List<ISkill> skills)
			{
				int _ = 0;
				foreach (ISkill skill in skills)
				{
					switch (skill.Effect.Param)
					{
						case TargetParam tp:
							if (tp.HealthParam > _)
								_ = tp.HealthParam;
							break;
					}
				}

				return _;
			}
		}
	}
}

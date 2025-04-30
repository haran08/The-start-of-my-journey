using System.Text.Json.Serialization;
using The_Final_Battle.Gameplay.Effects.Enum;
using The_Final_Battle.GameSystems;
using The_Final_Battle.Interfaces.Gameplay.Effects;

namespace The_Final_Battle.Gameplay.Effects
{
	// All Effects can be used by anything that implements IEffect.
	// The categories are here only to make it easier to find the ones created with
	// each mechanics in mind.

	// Skill's  Effects

	public class DefaultEffect : IEffect
	{
		public IEffectParam Param => throw new NotImplementedException();
		public GameActionTypeEnum ActionTypeEnum => throw new NotImplementedException();
		public void Execute() { throw new NotImplementedException(); }

		public IEffect Copy() => new DefaultEffect();
	}


	public class SkipTurnEffect : IEffect
	{
		public IEffectParam Param { get; }
		public GameActionTypeEnum ActionTypeEnum { get; } = GameActionTypeEnum.strategic;


		public SkipTurnEffect(IEffectParam param)
		{
			if (param is not TargetParam)
				throw new ArgumentException("Error: Effect inicialize with a invalid param. (SkipTurnEffect)",
					nameof(param) 
				);

			Param = param;
		}


		public void Execute()
		{
			if (Param is not TargetParam targetParam)
				throw new InvalidOperationException("Error: Effect has a invalid param. (SkipTurnEffect)");

			if (targetParam.Target is null)
				throw new ArgumentNullException(nameof(targetParam.Target),
					"Error: Effect execution received a null target. (SkipTurnEffect)"
				);


			Console.WriteLine($"{targetParam.Target.Name} did nothing.\n");
		}

		public IEffect Copy() => new SkipTurnEffect(Param.Copy());
	}


	public class DamageEffect : IEffect
	{
		public IEffectParam		  Param			  { get; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public DamagesType		  DamageType	  { get; init; }
		public GameActionTypeEnum ActionTypeEnum => GameActionTypeEnum.Attack;


		public DamageEffect(IEffectParam param, DamagesType damageType, float hitChance)
		{
			DamageType = damageType;

			param.ExtraParam = [];
			param.ExtraParam["float-HitChance".Dehumanize()] = hitChance;

			Param = param;
		}

		[JsonConstructor]
		protected DamageEffect(IEffectParam param, DamagesType damageType)
		{
			if (param is not TargetParam)
				throw new ArgumentException("Error: Effect inicialized with a invalid param. (DamageEffect)",
					nameof(param)
				);

			DamageType = damageType;
			Param = param;
		}


		public virtual void Execute()
		{
			if (Param is not TargetParam targetParam)
				throw new InvalidOperationException("Error: Effect has a invalid param. (DamageEffect)");

			if (targetParam.Target is null)
				throw new ArgumentNullException(nameof(targetParam.Target),
					"Error: Effect execution received a null target. (DamageEffect)"
				);


			DamageSystem.DealDamage(targetParam.HealthParam, targetParam.Target,
				(float)Param.ExtraParam["float-HitChance"], DamageType);

			targetParam.Target = null;
		}

		public virtual IEffect Copy() => new DamageEffect(Param.Copy(), DamageType);
	}


	public class DamageEffectRandom : DamageEffect
	{
		public DamageEffectRandom(IEffectParam param, DamagesType damageType, int minDamage) 
			: base(param, damageType)
		{
			param.ExtraParam = [];
			param.ExtraParam["int-MinDamage".Dehumanize()] = minDamage;
		}

		[JsonConstructor]
		private DamageEffectRandom(IEffectParam param, DamagesType damageType)
			: base(param, damageType) { }

		public override void Execute()
		{
			if (Param is not TargetParam targetParam)
				throw new InvalidOperationException("Error: Effect has a invalid param. (DamageEffectRandom)");

			if (targetParam.Target is null)
				throw new ArgumentNullException(nameof(targetParam.Target),
					"Error: Effect execution received a null target. (DamageEffectRandom)"
				);

			int minDamage = (int)Param.ExtraParam["int-MinDamage"];
			int damage = new Random().Next(minDamage, targetParam.HealthParam + 1);

			DamageSystem.DealDamage(damage, targetParam.Target,
				(float)Param.ExtraParam["float-HitChance"], DamageType);

			targetParam.Target = null;
		}

		public override IEffect Copy() => new DamageEffectRandom(Param.Copy(), DamageType);
	}

	
	public class HealingEffect : IEffect
	{
		public IEffectParam Param { get; }
		public GameActionTypeEnum ActionTypeEnum => GameActionTypeEnum.Heal;


		public HealingEffect(IEffectParam param)
		{
			if (param is not TargetParam)
				throw new ArgumentException("Error: Effect inicialized with a invalid param. (HealingEffect)",
					nameof(param)
				);

			Param = param;
		}


		public void Execute()
		{
			if (Param is not TargetParam targetParam)
				throw new ArgumentException("Error: Effect inicialized with a invalid param. (HealingEffect)",
					nameof(Param)
				);

			if (targetParam.Target is null)
				throw new ArgumentNullException(nameof(targetParam.Target),
					"Error: Effect execution received a null target. (DamageEffect)"
				);



			HealingSystem.Heal(targetParam.HealthParam, targetParam.Target);
			Console.WriteLine($"{targetParam.Target.Name} receive {targetParam.HealthParam} HP!");

			targetParam.Target = null;
		}

		public IEffect Copy() => new HealingEffect(Param.Copy());
	}


	// Gear Effects

}

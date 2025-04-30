using System.Text.Json.Serialization;
using The_Final_Battle.Gameplay.Effects;

namespace The_Final_Battle.Interfaces.Gameplay.Effects
{
	[JsonPolymorphic]
	[JsonDerivedType(typeof(DefaultEffect),		 typeDiscriminator: "DefaultEffect")]
	[JsonDerivedType(typeof(SkipTurnEffect),	 typeDiscriminator: "SkipTurnEffect")]
	[JsonDerivedType(typeof(DamageEffect),		 typeDiscriminator: "DamageEffect")]
	[JsonDerivedType(typeof(DamageEffectRandom), typeDiscriminator: "DamageEffectRandom")]
	[JsonDerivedType(typeof(HealingEffect),		 typeDiscriminator: "HealingEffect")]
	public interface IEffect
	{
		IEffectParam Param { get; }

		[JsonIgnore]
		GameActionTypeEnum ActionTypeEnum { get; }

		void Execute();

		IEffect Copy();
	}
}

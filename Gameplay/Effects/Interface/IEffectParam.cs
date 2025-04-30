using System.Text.Json.Serialization;
using The_Final_Battle.Gameplay.Effects;

namespace The_Final_Battle.Interfaces.Gameplay.Effects
{
	[JsonPolymorphic]
	[JsonDerivedType(typeof(TargetParam), typeDiscriminator: "TargetParam")]
	public interface IEffectParam
	{
		public Dictionary<string, object>? ExtraParam { get; set; }

		public IEffectParam Copy();
	}
}

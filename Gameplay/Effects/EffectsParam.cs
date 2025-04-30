using System.Text.Json.Serialization;
using The_Final_Battle.Interfaces.Characters;
using The_Final_Battle.Interfaces.Gameplay.Effects;

namespace The_Final_Battle.Gameplay.Effects
{
	public class TargetParam : IEffectParam
	{
		[JsonIgnore]
		public ICharacter?				   Target	   { get; set; }
		public int						   HealthParam { get; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Dictionary<string, object>? ExtraParam  { get; set; }


		public TargetParam(int healthParam)
		{
			HealthParam = healthParam;
		}

		private TargetParam(int healthParam, Dictionary<string, object>? extraParam)
		{
			HealthParam = healthParam;
			ExtraParam	= extraParam;
		}


		public IEffectParam Copy() => new TargetParam(HealthParam, ExtraParam);
	}
}

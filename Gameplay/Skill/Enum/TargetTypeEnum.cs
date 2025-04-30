using System.Text.Json.Serialization;

namespace The_Final_Battle.Gameplay.Skills.Enum
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum TargetTypeEnum : byte
	{
		Enemy,
		Ally,
		Self
	}
}

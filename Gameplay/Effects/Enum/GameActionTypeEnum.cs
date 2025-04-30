using System.Text.Json.Serialization;

namespace The_Final_Battle.Gameplay.Effects
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum GameActionTypeEnum : byte
	{
		Attack,
		Heal,
		strategic
	}
}

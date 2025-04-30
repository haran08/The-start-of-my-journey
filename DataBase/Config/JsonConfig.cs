using System.Text.Json;
using System.Text.Json.Serialization;
using The_Final_Battle.Characters;
using The_Final_Battle.Gameplay.Battle;
using The_Final_Battle.Gameplay.Inventary;
using The_Final_Battle.Gameplay.Inventary.Interface;
using The_Final_Battle.Interfaces.Characters;
using The_Final_Battle.Interfaces.Gameplay.Inventary;
using The_Final_Battle.Interfaces.Gameplay.Skills;

namespace The_Final_Battle.DataBase.Config
{

	public static class JsonConfig
	{
		public readonly static JsonSerializerOptions Options = new()
		{
			WriteIndented = true,

			Converters = {
				new CombatGroupConverter(),
			}
		};


		public static Dictionary<Type, string> JsonPaths = new() {

			{typeof(ISkill),		Path.Combine(GlobalVariables.BasePath, @"DataBase\Data\Skills.json")      },
			{typeof(Character),     Path.Combine(GlobalVariables.BasePath, @"DataBase\Data\Characters.json")  },
			{typeof(EnemiesWave),   Path.Combine(GlobalVariables.BasePath, @"DataBase\Data\Combats.json")     },
			{typeof(Consumable),    Path.Combine(GlobalVariables.BasePath, @"DataBase\Data\Consumables.json") },
			{typeof(IGear),         Path.Combine(GlobalVariables.BasePath, @"DataBase\Data\Gears.json")       },

		};
	}


	public class CombatGroupConverter : JsonConverter<CombatGroup>
	{
		public override CombatGroup? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			List<ICharacter> characters = [];
			List<IItem> items = [];

			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonTokenType.String:
						if (Memory.DataBase.TryGet(reader.GetString()!, out Character? ch))
						{
							characters.Add(ch!);
						}

						else if (Memory.DataBase.TryGet(reader.GetString()!, out IItem? item))
							items.Add(item!);

						break;
				}

				if (reader.TokenType == JsonTokenType.EndObject)
					break;
			}

			if (characters.Count == 0) return null;

			return new(characters, items);
		}
		public override void Write(Utf8JsonWriter writer, CombatGroup value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("Characters");
			writer.WriteStartArray();
			foreach (var ch in value)
			{
				writer.WriteStringValue(ch.Name);
			}
			writer.WriteEndArray();

			writer.WritePropertyName("Items");
			writer.WriteStartArray();
			foreach (var item in value.Items)
			{
				writer.WriteStringValue(item.Name);
			}
			writer.WriteEndArray();
			writer.WriteEndObject();
		}
	}


	public class SkillListConverter : JsonConverter<List<ISkill>>
	{
		public override List<ISkill>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{

			List<ISkill> _ = [];
			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonTokenType.String:
						if (Memory.DataBase.TryGet(reader.GetString()!, out ISkill? s))
							_.Add(s.Copy()!);
						break;
				}

				if (reader.TokenType == JsonTokenType.EndArray)
					break;
			}

			return _;
		}

		public override void Write(Utf8JsonWriter writer, List<ISkill> value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			value.ForEach(s => writer.WriteStringValue(s.Name));
			writer.WriteEndArray();
		}
	}


	public class ItemListConverter : JsonConverter<List<IItem>>
	{
		public override List<IItem>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			List<IItem> _ = [];
			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonTokenType.String:
						if (Memory.DataBase.TryGet(reader.GetString()!, out IItem? item))
							_.Add(item.Copy());
						break;
				}

				if (reader.TokenType == JsonTokenType.EndArray)
					break;
			}

			return _;
		}

		public override void Write(Utf8JsonWriter writer, List<IItem> value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			value.ForEach(i => writer.WriteStringValue(i.Name));
			writer.WriteEndArray();
		}
	}


	public class WeaponConverter : JsonConverter<Weapon>
	{
		public override Weapon? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch(reader.TokenType)
			{
				case JsonTokenType.String:
					if (Memory.DataBase.TryGet(reader.GetString()!, out IItem? gear))
						return (Weapon)gear.Copy();
					break;
			}
			
			return null;
		}
		public override void Write(Utf8JsonWriter writer, Weapon value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.Name);
		}
	}

	public class ArmorConverter : JsonConverter<Armor>
	{
		public override Armor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.String:
					if (Memory.DataBase.TryGet(reader.GetString()!, out IItem? gear))
						return (Armor)gear.Copy();
					break;
			}

			return null;
		}
		public override void Write(Utf8JsonWriter writer, Armor value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.Name);
		}
	}
}

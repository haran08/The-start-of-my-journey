using System.Text.Json;
using The_Final_Battle.Characters;
using The_Final_Battle.DataBase.Config;
using The_Final_Battle.Gameplay.Battle;
using The_Final_Battle.Gameplay.Inventary;
using The_Final_Battle.Gameplay.Inventary.Interface;
using The_Final_Battle.Gameplay.Skills;
using The_Final_Battle.Interfaces.Gameplay.Inventary;
using The_Final_Battle.Interfaces.Gameplay.Skills;

namespace The_Final_Battle.DataBase
{
	
	public class DataBase
	{
		public Dictionary<Type, object> _tables;

		

		public DataBase()
		{
			_tables = new() {

				{ typeof(ISkill),	   new SkillTable()		},
				{ typeof(Character),   new CharacterTable()	},
				{ typeof(EnemiesWave), new CombatTable()	},
				{ typeof(IItem),       new ItemTable()      },

			};
		}


		private void GetTable<T>(Type key, out T table)
		{
			table = (T)GetTable(key);
		}

		private object GetTable(Type key)
		{
			return _tables[key];
		}


		public void Start()
		{
			// --- Get Tables from _tables dict ---

			GetTable(typeof(ISkill),	out SkillTable		_skills);
			GetTable(typeof(IItem),		out ItemTable		_items);
			GetTable(typeof(Character),	out CharacterTable	_characters);
			GetTable(typeof(EnemiesWave), out CombatTable	_combats);

			try
			{
				// --- Read JSON data from files into strings ---
				string SkillsJson	   = File.ReadAllText(JsonConfig.JsonPaths[typeof(ISkill)]);
				string ConsumablesJson = File.ReadAllText(JsonConfig.JsonPaths[typeof(Consumable)]);
				string GearsJson	   = File.ReadAllText(JsonConfig.JsonPaths[typeof(IGear)]);
				string CharactersJson  = File.ReadAllText(JsonConfig.JsonPaths[typeof(Character)]);
				string CombatsJson	   = File.ReadAllText(JsonConfig.JsonPaths[typeof(EnemiesWave)]);


				// --- Deserialize Skills data from JSON and initialize _Skills dictionary ---
				TryDeserialize(SkillsJson, out List<Skill>? Skills);

				if (Skills != null)
				{
					foreach (ISkill skill in Skills)
					{
						_skills.Add(new(skill.Name.Dehumanize()), skill);
					}
				}


				// --- Deserialize Items data from JSON and initialize _Items dictionary ---
				TryDeserialize(ConsumablesJson, out List<IItem>? Consumables);
				TryDeserialize(GearsJson, out List<IItem>? Gears);

				if (Consumables != null)
				{
					foreach (IItem item in Consumables)
					{
						_items.Add(new(item.Name.Dehumanize()), item);
					}
				}
				if (Gears != null)
				{
					foreach (IItem item in Gears)
					{
						_items.Add(new(item.Name.Dehumanize()), item);
					}
				}


				// --- Deserialize Character data (Heroes and Enemies) and initialize _characters dictionary ---
				TryDeserialize(CharactersJson,  out List<Character>? charactes);

				if (charactes != null)
				{
					foreach (Character character in charactes)
					{
						_characters.Add(new(character.Name.Dehumanize()), character);
					}
				}


				// --- Deserialize Combats data from JSON and initialize _Combats dictionary ---
				TryDeserialize(CombatsJson, out List<EnemiesWave>? combats);

				if (combats != null)
				{
					foreach (EnemiesWave combat in combats)
					{
						_combats.Add(new(combat.Name.Dehumanize()), combat);
					}
				}

				Console.WriteLine("DataBase Initialized");


				/// <summary>
				/// Attempts to deserialize a JSON string into a List of type T.
				/// Returns true if deserialization is successful; otherwise, returns false and sets values to null.
				/// </summary>
				static bool TryDeserialize<T>(string json, out List<T>? values)
				{
					if (json == "{\r\n}" || json == "") 
					{
						values = null;
						return false;
					}

					values = JsonSerializer.Deserialize<List<T>>(json, JsonConfig.Options);
					
					if (values is List<Skill> skills)
					{
						foreach (Skill skill in skills)
						{
							Dictionary<string, object>? extraParam = skill.Effect.Param.ExtraParam;

							if (extraParam == null) continue;

							foreach (KeyValuePair<string, object> item in extraParam)
							{
								if (item.Value is JsonElement j)
								{
									var _ = JsonConvertElement(j);
									extraParam[item.Key] = _!;
								}
							}
						}	
					}

					return true;


					static object? JsonConvertElement(JsonElement element)
					{
						switch (element.ValueKind)
						{
							case JsonValueKind.Number:
								if (element.TryGetInt32(out int i)) return i;
								if (element.TryGetSingle(out float n)) return n;
								break;
						}

						return null;
					}
				}
			}

			catch (FileNotFoundException)
			{
				Console.WriteLine("Error: One or more files not found. (DataBase.Start)");
			}

			catch (JsonException e)
			{
				Console.WriteLine($"Error: JSON Deserialization: {e.Message} {e.StackTrace} (DataBase.Start)");
			}

			catch (Exception e)
			{
				Console.WriteLine($"Error: An unexpected error occurred: {e.Message} ({e.StackTrace}) (DataBase.Start)");
			}
		}


		/// <summary>
		/// Adds a key-value pair to the database with polymorphic type support.
		/// For polymorphism scenarios, specify the base table type as the generic type parameter.
		/// </summary>
		/// <typeparam name="T">Type key for retrieving the corresponding base table from the database</typeparam>
		/// <param name="key">Unique identifier</param>
		/// <param name="value">Entry value, which can include related polymorphic object types</param>
		public void Add<T>(string key, T value)
		{
			dynamic table = GetTable(typeof(T));

			table.Add(key.Dehumanize(), value);

			JsonUpdate<T>();
		}

		public T Get<T>(string name) where T : class
		{
			dynamic table = GetTable(typeof(T));

			return table.Get(name.Dehumanize()).Copy();
		}

		public bool TryGet<T>(string name, out T? instance) where T : class
		{
			dynamic table = GetTable(typeof(T));

			if (table.TryGetValue(name.Dehumanize(), out T value))
			{
				// If any error, First check if Value has the method .Copy()
				dynamic _ = value;
				instance  = _.Copy();
				return true;
			}

			instance = default;
			return false;
		}

		public List<T> GetTableValues<T>(List<string> names)
		{
			dynamic table   = GetTable(typeof(T));

			return (List<T>)(object)names.Select(name => table.Get(name.Dehumanize()).Copy()).ToList();
		}

		public void Remove<T>(string name)
		{
			dynamic table	= GetTable(typeof(T));

			table.Remove(name.Dehumanize());
			JsonUpdate<T>();
			return;
		}

		public void Clear()
		{
			_tables.Clear();
		}


		public void JsonUpdate<T>()
		{
			dynamic table = GetTable(typeof(T));

			Dictionary<string, T> _ = table._tableData;
			var tableValuesToArray = _.Select(o => o.Value).ToArray();

			string serializedCombat = JsonSerializer.Serialize(tableValuesToArray, JsonConfig.Options);
			File.WriteAllText( JsonConfig.JsonPaths[typeof(T)], serializedCombat	);
			return;

		}
	}
}

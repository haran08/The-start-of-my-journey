using System.Text.Json.Serialization;
using The_Final_Battle.DataBase.Config;
using The_Final_Battle.Gameplay.Inventary;
using The_Final_Battle.Interfaces.Characters;
using The_Final_Battle.Interfaces.Gameplay.Skills;
using The_Final_Battle.Miscellaneous;

namespace The_Final_Battle.Characters
{
	public class Character : ICharacter
	{
		[JsonConverter(typeof(SkillListConverter))]
		public List<ISkill>     Skills	       { get; set; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonConverter(typeof(WeaponConverter))]
		public Weapon?			Weapon		   { get; set; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonConverter(typeof(ArmorConverter))]
		public Armor?			Armor		   { get; set; }
		public string			ID			   { get; }		 = Guid.NewGuid().ToString();
		public string			Name		   { get; set; }
		[JsonIgnore]
		public int				CurrectHealth { get; set; }
		public int				MaxHealth	   { get; set; }

		public Character(string name, int health, List<ISkill> skills, Weapon? weapon = null,
			Armor? armor = null)
		{
			Skills		  = skills;
			Weapon		  = weapon;
			Armor		  = armor;
			Name		  = name;
			MaxHealth	  = health;
			CurrectHealth = MaxHealth;
		}

		public Character() : this("default", 100, []) { }


		public string ActionsToListString()
		{
			return Skills.ToFormatedListString(a => a.Name);
		}

		public ICharacter Copy()
		{
			List<ISkill> _	= Skills.Select(s => s.Copy()).ToList();
			Weapon? gear	= Weapon?.Copy() as Weapon;
			Armor? armor	= Armor?.Copy() as Armor;

			Character character = new(Name, MaxHealth, _);
			gear?.Equip(character);
			armor?.Equip(character);


			return character;
		}
	}	
}
using System.Text.Json.Serialization;
using The_Final_Battle.Gameplay.Effects.Enum;
using The_Final_Battle.Gameplay.Inventary.Interface;
using The_Final_Battle.Interfaces.Characters;
using The_Final_Battle.Interfaces.Gameplay.Inventary;

namespace The_Final_Battle.Gameplay.Inventary
{
	public class Armor : IGear, IItem
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ICharacter?	Using  { get; private set; }
		public string		ID	   { get; }		= new Random().Next(0, 100).ToString();
		public string		Name   { get; set; }
		public int DefenseModifier { get; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public DamagesType	ResistenceType { get; }


		public Armor(string name, int defenseModifier, DamagesType resistenceType)
		{
			Name = name;
			DefenseModifier = defenseModifier;
			ResistenceType	= resistenceType;
		}


		public IItem Copy() => new Armor(Name, DefenseModifier, ResistenceType);

		public void Equip(ICharacter character)
		{
			character.Armor = this;
			Using			= character;
		}

		public void Unequip(List<IItem> inventory)
		{
			Using.Armor = null;
			Using		= null;

			inventory.Add(this);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}

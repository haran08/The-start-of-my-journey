using System.Text.Json.Serialization;
using The_Final_Battle.Gameplay.Inventary;

namespace The_Final_Battle.Interfaces.Gameplay.Inventary
{
	[JsonPolymorphic]
	[JsonDerivedType(typeof(Consumable), "Consumable")]
	[JsonDerivedType(typeof(Weapon), "Weapon")]
	[JsonDerivedType(typeof(Armor), "Armor")]
	public interface IItem
	{
		public string ID { get; }
		public string Name { get; }


		public IItem Copy();

		public string ToString();
	}
}

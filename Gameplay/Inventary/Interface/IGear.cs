using The_Final_Battle.Interfaces.Characters;
using The_Final_Battle.Interfaces.Gameplay.Inventary;

namespace The_Final_Battle.Gameplay.Inventary.Interface
{
	public interface IGear
	{
		ICharacter?  Using { get; }


		public void Equip(ICharacter character);

		public void Unequip(List<IItem> inventory);
	}
}

using The_Final_Battle.Gameplay.Inventary;
using The_Final_Battle.Interfaces.Gameplay.Skills;

namespace The_Final_Battle.Interfaces.Characters
{
	public interface ICharacter
	{
		public List<ISkill> Skills			{ get; set; }
		public Weapon?		Weapon			{ get; set; }
		public Armor?		Armor			{ get; set; }
		public string		ID				{ get; }
		public string		Name			{ get; set; }
		public int			CurrectHealth	{ get; set; }
		public int			MaxHealth		{ get; }



		public ICharacter Copy();
	}
}

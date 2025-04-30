using The_Final_Battle.Characters.CharacterAILogic;
using The_Final_Battle.Gameplay.Battle;
using The_Final_Battle.Gameplay.Inventary;
using The_Final_Battle.Interfaces.Characters;
using The_Final_Battle.Interfaces.Gameplay.Skills;

namespace The_Final_Battle.Characters
{
	public class CharacterAI : ICharacter
	{
		public List<ISkill>	Skills			{ get; set; }
		public Func<BattleMiniGame, (Action, string)>	Logic;
		public Weapon?		Weapon			{ get; set; }
		public Armor?		Armor			{ get; set; }
		public string		ID				{ get; }		= Guid.NewGuid().ToString();
		public string		Name			{ get; set; }
		public int			CurrectHealth	{ get; set; }
		public int			MaxHealth		{ get; set; }

		public CharacterAI(string name, int health, List<ISkill> skills,
			Func<BattleMiniGame, (Action, string)> logic,Weapon? weapon = null, 
			Armor? armor = null)
		{
			Skills		  = skills;
			Weapon		  = weapon;
			Armor		  = armor;
			Name		  = name;
			MaxHealth	  = health;
			CurrectHealth = health;
			Logic		  = logic;
		}


		public CharacterAI(ICharacter character, Func<BattleMiniGame, (Action, string)> logic)
		{
			if (character is not Character)
				throw new ArgumentException("Error: The constructor of CharacterAI only accept Character type. " +
					"(CharacterAI(ICharacter, logic))", character.GetType().Name);

			Skills = character.Skills;
			Name = character.Name;
			MaxHealth = character.MaxHealth;
			CurrectHealth = character.CurrectHealth;
			Logic = logic;

			character.Weapon?.Equip(this);
			character.Armor?.Equip(this);
		}


		public CharacterAI() : this("default", 100, [], AILogic.DefaultLogic) { }


		public ICharacter Copy()
		{
			List<ISkill> skills = Skills.Select(s => s.Copy()).ToList();
			Weapon? gear  = Weapon?.Copy() as Weapon;
			Armor?	armor = Armor?.Copy() as Armor;

			CharacterAI character = new(Name, MaxHealth, skills, Logic);
			gear?.Equip(character);
			armor?.Equip(character);


			return character;
		}
	}
}

using System.Text.Json.Serialization;
using The_Final_Battle.DataBase.Config;
using The_Final_Battle.Gameplay.Inventary.Interface;
using The_Final_Battle.Interfaces.Characters;
using The_Final_Battle.Interfaces.Gameplay.Inventary;
using The_Final_Battle.Interfaces.Gameplay.Skills;

namespace The_Final_Battle.Gameplay.Inventary
{
	public class Weapon : IGear, IItem
	{
		[JsonConverter(typeof(SkillListConverter))]
		public List<ISkill> Skills	{ get; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ICharacter?	Using	{ get; private set; }
		public string		ID		{ get; } = new Random().Next(0, 100).ToString();
		public string		Name	{ get; set; }
		

		public Weapon(string name, List<ISkill> skills)
		{
			Name	= name;
			Skills	= skills;
		}


		public IItem Copy()
		{
			List<ISkill> copy = Skills.Select(s => s.Copy()).ToList();

			return new Weapon(Name, copy);
		}

		public void Equip(ICharacter character)
		{
			character.Weapon = this;
			Using = character;

			Skills.ForEach(s => character.Skills.Add(s));
			character.Skills = character.Skills.Distinct().ToList();
		}

		public void Unequip(List<IItem> inventory)
		{
			foreach (ISkill skill in Skills)
			{
				if (Using.Skills.Contains(skill)) 
					Using.Skills.Remove(skill);
			}

			Using.Weapon = null;
			Using = null;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}

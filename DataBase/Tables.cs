using The_Final_Battle.Characters;
using The_Final_Battle.Gameplay.Battle;
using The_Final_Battle.Bases.DataBase;
using The_Final_Battle.Interfaces.Gameplay.Inventary;
using The_Final_Battle.Interfaces.Gameplay.Skills;

namespace The_Final_Battle.DataBase
{
	public class SkillTable		: BaseTable<string, ISkill>;

	public class CharacterTable : BaseTable<string, Character>;

	public class CombatTable	: BaseTable<string, EnemiesWave>;

	public class ItemTable		: BaseTable<string, IItem>;

}

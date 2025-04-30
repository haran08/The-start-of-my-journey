using System.Text.Json.Serialization;
using The_Final_Battle.Interfaces.Gameplay.Actions;
using The_Final_Battle.Gameplay.Skills;

namespace The_Final_Battle.Interfaces.Gameplay.Skills
{
	[JsonPolymorphic]
	[JsonDerivedType(typeof(Skill), typeDiscriminator: "skill")]
	public interface ISkill : IAction
	{
		string ID	{ get; }
		string Name { get; }

		public ISkill Copy();
	}
}

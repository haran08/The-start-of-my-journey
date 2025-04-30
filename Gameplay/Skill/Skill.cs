using The_Final_Battle.Gameplay.Skills.Enum;
using The_Final_Battle.Interfaces.Gameplay.Skills;
using The_Final_Battle.Interfaces.Gameplay.Effects;

namespace The_Final_Battle.Gameplay.Skills
{

	public class Skill : ISkill
	{
		public string		  ID			 { get; } = Guid.NewGuid().ToString();
		public string		  Name			 { get; }
		public IEffect		  Effect		 { get; }
		public TargetTypeEnum TargetTypeEnum { get; }


		public Skill(string name, IEffect effect, TargetTypeEnum targetTypeEnum)
		{
			Name		   = name;
			Effect		   = effect;
			TargetTypeEnum = targetTypeEnum;
		}


		public void Execute()
		{
			Effect.Execute();
		}

		public bool Equals(Skill? other) => other is not null && Name == other.Name;

		public override bool Equals(object? obj) => Equals(obj as Skill);

		public override int GetHashCode() => base.GetHashCode();

		public ISkill Copy()
		{
			return new Skill(Name, Effect.Copy(), TargetTypeEnum);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}

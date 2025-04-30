using System.Text.Json.Serialization;
using The_Final_Battle.Gameplay.Effects;
using The_Final_Battle.Gameplay.Skills.Enum;
using The_Final_Battle.Interfaces.Gameplay.Actions;
using The_Final_Battle.Interfaces.Gameplay.Effects;
using The_Final_Battle.Interfaces.Gameplay.Inventary;

namespace The_Final_Battle.Gameplay.Inventary
{
	public class Consumable : IAction, IItem
	{
		public IEffect		  Effect		 { get; }
		public string		  ID			 { get; } = new Random().Next(0, 100).ToString();
		public string		  Name			 { get; }
		public TargetTypeEnum TargetTypeEnum { get; }

		[JsonConstructor]
		public Consumable(string name, IEffect effect, TargetTypeEnum targetTypeEnum)
		{
			Name		   = name;
			Effect		   = effect;
			TargetTypeEnum = targetTypeEnum;
		}

		public Consumable()
		{
			Name = "";
			Effect = new DefaultEffect();
			TargetTypeEnum = TargetTypeEnum.Self;
		}

		public IItem Copy() => new Consumable(Name, Effect.Copy(), TargetTypeEnum);

		public void Execute()
		{
			Effect.Execute();
		}

		public override string ToString()
		{
			return Name;
		}
	}
}

using The_Final_Battle.Gameplay.Skills.Enum;
using The_Final_Battle.Interfaces.Gameplay.Effects;

namespace The_Final_Battle.Interfaces.Gameplay.Actions
{
	public interface IAction
	{
		public IEffect		  Effect	 { get; }
		public TargetTypeEnum TargetTypeEnum { get; }

		public void    Execute();
	}
}
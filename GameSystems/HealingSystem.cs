using The_Final_Battle.Interfaces.Characters;

namespace The_Final_Battle.GameSystems
{
	public static class HealingSystem
	{
		public static void Heal(int heal, ICharacter target)
		{
			if (target.CurrectHealth + heal >= target.MaxHealth)
				target.CurrectHealth = target.MaxHealth;

			else
				target.CurrectHealth += heal;
		}
	}
}

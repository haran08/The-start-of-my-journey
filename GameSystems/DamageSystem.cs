using The_Final_Battle.Gameplay.Effects.Enum;
using The_Final_Battle.Interfaces.Characters;

namespace The_Final_Battle.GameSystems
{
	public static class DamageSystem
	{
		public static void DealDamage(int damage, ICharacter target, float hitChance,
			DamagesType damageType)
		{
			if(new Random().NextSingle() > hitChance)
			{
				Console.WriteLine("Miss");
				return;
			}


			if (target.Armor is not null && damageType == target.Armor.ResistenceType)
				damage -= target.Armor.DefenseModifier;


			if (target.CurrectHealth - damage <= 0) {
				target.CurrectHealth = 0;
			}

			else
				target.CurrectHealth -= damage;

			Console.WriteLine($"{target.Name} takes {damage} damage!");
		}
	}
}

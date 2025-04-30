namespace The_Final_Battle.Gameplay.Battle
{
	/// <summary>
	/// Manages a combat encounter between two CombatGroups, handling turns and rounds.
	/// </summary>
	public class EnemiesWave(string name, List<CombatGroup> enemies)
	{
		public List<CombatGroup> Enemies { get; } = enemies;
		public string			 Name	 { get; } = name;

		public EnemiesWave Copy()
		{
			List<CombatGroup> _ = Enemies.Select(e => e.Copy()).ToList();

			return new(Name, _);
		}
	}
}
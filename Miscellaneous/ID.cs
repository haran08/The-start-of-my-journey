namespace The_Final_Battle.Miscellaneous
{
	internal static class IDManager
	{
		/// <summary>
		/// Generates a unique ID for a given type.
		/// The "type" param indicates the object that will receive the generated ID.
		/// Example: GenerateID(GetType());
		/// </summary>
		/// <param name="type">The <see cref="Type"/> of the class that will have
		/// its ID property populated.</param>
		/// <returns></returns>
		public static string GenerateID(Type type)
		{
			string seed = Guid.NewGuid().ToString(); // Generate a random number with 8 digits
			string name = type.Name;

			return name.ToLower() + "CoreEffectBase" + seed;
		}
	}
}
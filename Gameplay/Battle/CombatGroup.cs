using The_Final_Battle.Miscellaneous;
using System.Collections;
using System.Runtime.CompilerServices;
using The_Final_Battle.Interfaces.Characters;
using The_Final_Battle.Interfaces.Gameplay.Inventary;



namespace The_Final_Battle.Gameplay.Battle
{
	/// <summary>
	/// Represents a group of max 4 characters participating in combat.
	/// </summary>
	[CollectionBuilder(typeof(CombatGroupBuilder), "Create")]
	public class CombatGroup : IList<ICharacter>
	{
		private		  List<ICharacter> _characters { get; set; }
		private const byte             MaxSize					 = 4;
		public		  int			   Count					 => _characters.Count;
		public		  bool			   IsReadOnly  { get; }
		public		  List<IItem>	   Items	   { get; init; }

		public CombatGroup()
		{
			_characters = [];
			Items		= [];
			IsReadOnly  = false;
		}

		public CombatGroup(ReadOnlySpan<ICharacter> characters)
		{
			if (characters.Length > MaxSize)
			{
				throw new ArgumentOutOfRangeException(
					paramName: nameof(characters),
					message: $"The number of characters cannot exceed {MaxSize}. " + 
                     $"Actual value: {characters.Length}."
				);
			}

			_characters = characters.ToArray().ToList();
			Items       = [];
			IsReadOnly  = false;
		}

		public CombatGroup(List<ICharacter> characters, List<IItem>? items = null)
		{
			if (characters.Count > MaxSize)
			{
				throw new ArgumentOutOfRangeException(
					paramName: nameof(characters),
					message: $"The number of characters cannot exceed {MaxSize}. " +
					 $"Actual value: {characters.Count}."
				);
			}

			_characters = characters;
			Items		= items ?? [];
			IsReadOnly  = false;
		}

		public ICharacter this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
				{
					throw new ArgumentOutOfRangeException(nameof(index), "Index was outside the bounds of the array.");
				}

				return _characters[index];
			}
			set
			{
				if (index < 0 || index >= Count)
				{
					throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
				}

				_characters[index] = value;
			}
		}

		/// <summary>
		/// Adds a character to the end of the CombatGroup.  
		/// Throws an exception if the maximum size of the CombatGroup is exceeded.
		/// </summary>
		/// <param name="character">The character to add to the CombatGroup.</param>
		/// <exception cref="InvalidOperationException">
		/// Thrown when attempting to add more characters than the maximum allowed size.
		/// </exception>
		public void Add(ICharacter character)
		{
			if (Count >= MaxSize) 
				throw new InvalidOperationException("Cannot ADD more than 4 characters to a CombatGroup class.");

			if (IsReadOnly)
				throw new InvalidOperationException("Cannot ADD characters to a read-only CombatGroup class.");

			_characters.Add(character);
		}

		/// <summary>
		/// Removes the character at the specified index from the CombatGroup.  
		/// Shifts the remaining characters to fill the gap created by the removed character.
		/// </summary>
		/// <param name="index">The zero-based index of the character to remove.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Thrown when the specified index is less than 0 or greater than or equal to the length of the CombatGroup.
		/// </exception>
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= Count)
				throw new ArgumentOutOfRangeException(nameof(index), "Error: Index is out of range. (CombatGroup)");

			if (IsReadOnly)
				throw new InvalidOperationException("Cannot REMOVE characters from a read-only CombatGroup class.");

			_characters.RemoveAt(index);
		}

		/// <summary>
		/// Copy the CombatGroup to a new CombatGroup object, with copies of all
		/// characters and items.
		/// </summary>
		/// <returns></returns>
		public CombatGroup Copy()
		{
			List<ICharacter> characters = _characters.Select(c => c.Copy()).ToList();
			List<IItem>		 items		= Items.Select(i => i.Copy()).ToList();
			return new(characters, items);
		}

		/// <summary>
		/// Removes all characters from the CombatGroup array.
		/// </summary>
		public void Clear() => _characters.Clear();

		/// <summary>
		/// Formats the CombatGroup into a numbered string list, displaying each character's name in a numeric order.
		/// </summary>
		public string ToListString(string? plusElement = null) => 
			_characters.ToFormatedListString(c => c.Name, plusElement!);

		public int IndexOf(ICharacter item) => _characters.IndexOf(item);

		public void Insert(int index, ICharacter item) => _characters.Insert(index, item);

		public bool Contains(ICharacter item) => _characters.Contains(item);

		public void CopyTo(ICharacter[] array, int arrayIndex) => 
			_characters.CopyTo(array, arrayIndex);

		public bool Remove(ICharacter item) => _characters.Remove(item);

		public IEnumerator<ICharacter> GetEnumerator() => 
			_characters.AsEnumerable<ICharacter>().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}


	public static class CombatGroupBuilder
	{
		public static CombatGroup Create(ReadOnlySpan<ICharacter> characters) => new(characters);
	}
}
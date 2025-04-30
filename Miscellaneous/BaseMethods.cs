using System.Text;

namespace The_Final_Battle.Miscellaneous
{
	public static class BaseMethods
	{
		public static int AskForNumber(string message)
		{
			Console.WriteLine(message);
			int result;
			while (!int.TryParse(Console.ReadLine(), out result))
			{
				Console.WriteLine("Invalid input. Please enter a number.");
			}

			return result;
		}

		public static List<int> AskForNumber(string message, int maxNumbers)
		{
			List<int> list = new List<int>();
			Console.WriteLine(message);
			int num = 0;
			while (true)
			{
				if (!int.TryParse(Console.ReadLine(), out var result))
				{
					Console.WriteLine("Invalid input. Please enter a number.");
				}
				else if (num > maxNumbers)
				{
					break;
				}

				list.Add(result);
				num++;
			}

			return list;
		}

		public static int AskForNumberInRange(string message, int min, int max, bool endL = false)
		{
			if (max == 0)
			{
				return 0;
			}

			Console.Write(message);
			int result;
			while (true)
			{
				if (!int.TryParse(Console.ReadLine(), out result))
				{
					Console.WriteLine("Invalid input. Please enter a number.");
					continue;
				}

				if (result >= min && result <= max - 1)
				{
					break;
				}

				Console.WriteLine($"Invalid input. Please enter a number in the range [{min} - {max - 1}].");
			}

			if (endL)
			{
				Console.WriteLine();
			}

			return result;
		}

		public static string ToFormatedNumberedListString<T>(this IEnumerable<T> collection, Func<T, string> valueSelector)
		{
			if (collection == null)
			{
				return string.Empty;
			}

			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (T item in collection)
			{
				stringBuilder.Append($"{num} - {valueSelector(item)}\n");
				num++;
			}

			return stringBuilder.ToString();
		}

		public static string ToFormatedListString<T>(this List<T> collection, Func<T, string> valueSelector,
			string? plusElement = null)
		{
			if (collection == null)
			{
				return string.Empty;
			}

			StringBuilder stringBuilder = new StringBuilder();
			if (plusElement != null)
			{
				stringBuilder.AppendLine($"{0} - {plusElement}");
			}

			for (int i = 0; i < collection.Count; i++)
			{
				if (plusElement != null)
				{
					stringBuilder.AppendLine($"{i + 1} - {valueSelector(collection[i])}");
				}
				else
				{
					stringBuilder.AppendLine($"{i} - {valueSelector(collection[i])}");
				}
			}

			return stringBuilder.ToString();
		}
	}
}

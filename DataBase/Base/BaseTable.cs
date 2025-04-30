namespace The_Final_Battle.Bases.DataBase
{
	public abstract class BaseTable<TKey, TValue> where TKey : notnull
	{
		public readonly Dictionary<TKey, TValue> _tableData = [];


		public void Add(TKey key, TValue value)
		{
			_tableData.Add(key, value);
		}

		public TValue Get(TKey key)
		{
			return _tableData[key];
		}

		public bool Remove(TKey key)
		{
			if (_tableData.Remove(key)) return true;

			return false;
		}

		public bool TryGetValue(TKey key, out TValue? value)
		{
			if (_tableData.TryGetValue(key, out TValue? result))
			{
				value = result;
				return true;
			}

			value = default;
			return false;
		}
	}
}

<Query Kind="Program">
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

void Main()
{
	throw new NotSupportedException("Wut?!");
}

public static class CollectionsExtensions
{
	public static IReadOnlyCollection<TItem> ToReadOnlyCollection<TItem>(this IEnumerable<TItem> collection)
	{
		return collection switch
		{
			List<TItem> list => list,
			IReadOnlyList<TItem> readOnlyList => readOnlyList,
			IReadOnlyCollection<TItem> readOnlyCollection => readOnlyCollection,
			IList<TItem> => throw new NotSupportedException("IList<TItem>"),
			ICollection<TItem> => throw new NotSupportedException("ICollection<TItem>"),
			_ => collection.ToList()
		};
	}
}

<Query Kind="Program">
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

void Main()
{
	throw new NotSupportedException("Wut?!");
}

public static class GenericExtensions
{
	public static bool In<T>(this T @this, params T[] values)
	{
		return values.Contains(@this);
	}
	
	public static bool In<T>(this T @this, IReadOnlyCollection<T> values)
	{
		return values.Contains(@this);
	}
	
	public static TOut Transform<TIn, TOut>(this TIn @this, Func<TIn, TOut> f) => f(@this);
}

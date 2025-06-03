<Query Kind="Program">
</Query>

void Main()
{
	
}

public static class DumpExtensions
{
	public static T Dump0<T>(this T @this, string description)
	{
		return @this.Dump(description, collapseTo: 0);
	}
}
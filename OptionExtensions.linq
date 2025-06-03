<Query Kind="Program">
  <NuGetReference>LanguageExt.Core</NuGetReference>
  <Namespace>LanguageExt</Namespace>
</Query>

void Main()
{
}

public static class OptionExtensions
{
	public static T ValueOrThrow<T>(this Option<T> @this) => @this.Match(some => some, () => throw new InvalidOperationException("Option was indeed None."));
}

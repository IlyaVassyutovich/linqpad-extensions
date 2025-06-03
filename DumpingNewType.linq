<Query Kind="Program">
  <NuGetReference>LanguageExt.Core</NuGetReference>
  <Namespace>LanguageExt</Namespace>
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

void Main()
{
	throw new NotSupportedException("Wut?!");
}

public abstract class DumpingNewType<TNewType, TWrappedType> : NewType<TNewType, TWrappedType>
	where TNewType : NewType<TNewType, TWrappedType>
	where TWrappedType : notnull
{
	protected DumpingNewType(TWrappedType value) : base(value)
	{
	}
	
	public object ToDump() => Value;
}

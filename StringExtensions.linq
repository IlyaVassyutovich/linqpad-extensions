<Query Kind="Program">
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

void Main()
{
	
}

public static class StringExtensions
{
	public static int Count(this string source, string substring)
	{
		ArgumentNullException.ThrowIfNullOrWhiteSpace(source);
		ArgumentNullException.ThrowIfNullOrWhiteSpace(substring);

		int count = 0;
		int position = 0;

		while ((position = source.IndexOf(substring, position)) != -1)
		{
			count++;
			position += substring.Length; // Move past the current occurrence
		}

		return count;
	}
}
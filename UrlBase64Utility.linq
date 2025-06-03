<Query Kind="Program">
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

void Main()
{
	throw new NotSupportedException("Go Away!");
}

public static class UrlBase64Utility
{
	private static readonly char _character62 = '+';
	private const char Character62Replacement = '-';
	private static readonly char _character63 = '/';
	private static readonly char _character63Replacement = '_';

	private const char PaddingCharacter = '=';

	public static string ConvertToString(byte[] bytes)
	{
		var charLength = 4 * (int)Math.Ceiling(bytes.Length / 3.0);
		var chars = new char[charLength];

		Convert.ToBase64CharArray(bytes, 0, bytes.Length, chars, 0);

		var modifiedChars = chars
			.Where(c => c != PaddingCharacter)
			.Select(c =>
			{
				if (c == _character62)
					return Character62Replacement;
				if (c == _character63)
					return _character63Replacement;

				return c;
			})
			.ToArray();

		return new string(modifiedChars);
	}

	public static byte[] ConvertToBytes(string urlBase64String)
	{
		var charLength = 4 * (int)Math.Ceiling(urlBase64String.Length / 4.0);
		int padLength = charLength - urlBase64String.Length;

		var chars = urlBase64String
			.ToCharArray()
			.Select(c =>
			{
				if (c == Character62Replacement)
					return _character62;
				if (c == _character63Replacement)
					return _character63;

				return c;
			})
			.Concat(Enumerable
				.Range(1, padLength)
				.Select(i => PaddingCharacter))
			.ToArray();

		var bytes = Convert.FromBase64CharArray(chars, 0, chars.Length);
		return bytes;
	}
}
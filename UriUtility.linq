<Query Kind="Program">
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

#load "e\GenericExtensions"

void Main()
{
	new[]
	{
		@"https://example.com/some/path?foo=bar&baz=spam#fragment",
		@"https://example.com/some/path?foo=bar&baz#fragment",
	}
		.Select(str => new Uri(str))
		.Select(
			uri => new
			{
				Uri = uri,
				Paramters = uri.GetQueryParameters()
			})
		.Dump();
}

public static class UriUtility
{
	public static IReadOnlyDictionary<string, string?> GetQueryParameters(this Uri uri)
	{
		var query = uri.Query.TrimStart('?');
		
		return query
			.Split('&')
			.Select(parameter => parameter.Split('='))
			.Select(
				kvpCandidate =>
				{
					if (kvpCandidate.Length == 2)
						return new KeyValuePair<string, string?>(kvpCandidate[0], kvpCandidate[1]);
					else if (kvpCandidate.Length == 1)
						return new KeyValuePair<string, string?>(kvpCandidate[0], null);
					else
						throw new ArgumentException(
							$"Unable to parse query parameters for uri \"{uri}\".");
				})
			.Select(kvp => Decode(kvp))
			.Transform(kvps => new Dictionary<string, string?>(kvps));
			
		
		static KeyValuePair<string, string?> Decode(KeyValuePair<string, string?> input)
		{
			return new KeyValuePair<string, string?>(
				System.Web.HttpUtility.UrlDecode(input.Key),
				System.Web.HttpUtility.UrlDecode(input.Value));
		}
	}
}
<Query Kind="Program">
  <NuGetReference>LanguageExt.Core</NuGetReference>
  <Namespace>LanguageExt</Namespace>
</Query>

void Main()
{
	
}

namespace StoredPasswords
{
	public static class Passwords
	{
		public static Option<StoredPasswordValue> Get(StoredPasswordKey key)
		{
			return Prelude
				.Optional(Util.GetPassword(key.Value, noDefaultSave: true))
				.Map(str => new StoredPasswordValue(str));
		}
		
		public static Option<StoredPasswordValue> Get(Func<WellKnownPasswords, StoredPasswordKey> keySelector) =>
			Get(keySelector(WellKnownPasswords.Instance));
	}

	public sealed class StoredPasswordKey(string value) : NewType<StoredPasswordKey, string>(value);
	public sealed class StoredPasswordValue(string value) : NewType<StoredPasswordValue, string>(value);
	
	public sealed class WellKnownPasswords
	{
		public static WellKnownPasswords Instance { get; } = new();
		
		private WellKnownPasswords()
		{
		}
		
		public StoredPasswordKey TodoistApi { get; } = new("TodoistApi");
	}
}

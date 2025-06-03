<Query Kind="Program">
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

void Main()
{
	throw new NotSupportedException("Go Away!");
}

public static class UnixDateTimeExtensions
{
	public static DateTime FromUnixMilliseconds(this long unixMillisecondsSinceEpoch)
	{
		return DateTimeOffset
			.FromUnixTimeMilliseconds(unixMillisecondsSinceEpoch)
			.UtcDateTime;
	}

	public static long ToUnixMilliseconds(this DateTime dateTimeUtc)
	{
		return new DateTimeOffset(dateTimeUtc).ToUnixTimeMilliseconds();
	}

	public static long ToUnixSeconds(this DateTime dateTimeUtc)
	{
		return new DateTimeOffset(dateTimeUtc).ToUnixTimeSeconds();
	}
}
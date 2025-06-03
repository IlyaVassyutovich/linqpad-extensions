<Query Kind="Program">
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
  <Namespace>System.Text.Json.Nodes</Namespace>
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

#load "e\DumpingNewType"
#load "e\ExceptionExtensions"
#load "e\FileSystem"
#load "e\GenericExtensions"

void Main()
{
	
}

public static class Json
{
	public static T Deserialize<T>(string json)
	{
		return JsonSerializer.Deserialize<T>(json)
			?? throw new InvalidOperationException("Failed to deserialze JSON.");
	}

	public static T Deserialize<T>(FileSystemFilePath file)
	{
		return File
			.ReadAllText(file)
			.Transform(text => Deserialize<T>(text));
	}
	
	public static T Deserialize<T>(JsonObject jo)
	{
		try
		{
			return jo.Deserialize<T>()
				?? throw new JsonException($"Failed to deserialize JsonObject into type \"{typeof(T)}\".");
		}
		catch (JsonException je)
		{
			throw new JsonException(
					$"Failed to deserialize JsonObject into type \"{typeof(T)}\".",
					je)
				.WithData("JsonObject", jo.ToString());
		}
	}
}
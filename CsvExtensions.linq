<Query Kind="Program">
  <NuGetReference>CsvHelper</NuGetReference>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>CsvHelper.Configuration.Attributes</Namespace>
  <Namespace>System.Globalization</Namespace>
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

void Main()
{
	throw new NotSupportedException("Wut?!");
}

public static class Csv
{
	public static CsvConfiguration Default { get; } = new(CultureInfo.CurrentCulture)
	{
		HasHeaderRecord = true
	};

	public static void WriteRecords<T>(this IEnumerable<T> records, string destinationFile)
	{
		using var fileStream = new FileStream(
			destinationFile,
			FileMode.CreateNew,
			FileAccess.ReadWrite);
		using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
		using var csvWriter = new CsvWriter(streamWriter, Default);

		csvWriter.WriteRecords(records);
	}

	public static IReadOnlyCollection<T> ReadRecords<T>(string sourceFile)
	{
		using var fileStream = File.OpenRead(sourceFile);
		using var streamReader = new StreamReader(fileStream, Encoding.UTF8);
		using var csvWriter = new CsvReader(streamReader, Default);

		return csvWriter.GetRecords<T>().ToList();
	}
}
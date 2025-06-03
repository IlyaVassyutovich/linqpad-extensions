<Query Kind="Program">
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

void Main()
{	
	try
	{
		Try();
	}
	catch (Exception e)
	{
		e.RenderAsString().Dump(nameof(ExceptionExtensions.RenderAsString));
		
		e.RenderAsStrings().Dump(nameof(ExceptionExtensions.RenderAsStrings));
	}
	
	void Try()
	{
		try
		{
			TryCore();
		}
		catch (ArgumentOutOfRangeException aore)
		{
			throw new InvalidOperationException("Failed to try.", aore);
		}
		
		void TryCore()
		{
			new DateTime(-1, -1, -1);
		}
	}
}

public static class ExceptionExtensions
{
	public static string RenderAsString(this Exception exception)
	{
		var resultBuilder = new StringBuilder();

		RenderCore(exception);

		return resultBuilder.ToString();

		void RenderCore(Exception exception)
		{
			resultBuilder.AppendLine($"[{exception.GetType().FullName}]: {exception.Message}");
			resultBuilder.AppendLine(exception.StackTrace);
			if (exception.InnerException != null)
			{
				resultBuilder.AppendLine();
				RenderCore(exception.InnerException);
			}
		}
	}

	public static IReadOnlyCollection<string> RenderAsStrings(this Exception exception, bool separateNestedWithEmptyString = true)
	{
		var result = new List<string>();

		var atFirstLevel = true;
		for (var renderedException = exception; renderedException != null; renderedException = renderedException.InnerException, atFirstLevel = false)
		{
			if (!atFirstLevel && separateNestedWithEmptyString)
				result.Add(string.Empty);

			result.Add($"[{renderedException.GetType().FullName}]: {renderedException.Message}");
			result.AddRange(renderedException.StackTrace.Split(Environment.NewLine));
		}

		return result;
	}

	public static TException WithData<TException>(this TException @this, string key, object value) where TException : Exception
	{
		@this.AddData(key, value);
		return @this;
	}

	public static void AddData(this Exception @this, string key, object value)
	{
		switch (@this)
		{
			case AggregateException agg:
				agg.AddData(key, value);
				break;
			default:
				AddDataCore(@this, key, value);
				break;
		}
	}

	public static void AddData(this AggregateException @this, string key, object value)
	{
		switch (@this.InnerExceptions.ToList())
		{
			case [var single]:
				single.AddData(key, value);
				break;
			case []:
				AddDataCore(@this, key, value);
				break;
			case [.. var multiple]:
				foreach (var inner in multiple)
				{
					inner.AddData(key, value);
				}
				break;
		}
	}

	private static void AddDataCore(Exception exception, string key, object value)
	{
		exception.Data.Add(key, value);
	}
}

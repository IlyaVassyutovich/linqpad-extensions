<Query Kind="Program">
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

void Main()
{
	throw new NotSupportedException("Wut?!");
}

public static class TypeExtensions
{
	public static bool IsDirectImplementationOfGenericType(
				this Type type, Type genericTypeDefinition)
	{
		if (type == null)
			throw new ArgumentNullException(nameof(type));
		if (genericTypeDefinition == null)
			throw new ArgumentNullException(nameof(genericTypeDefinition));

		return type.IsGenericType && !type.IsGenericTypeDefinition &&
			(type.GetGenericTypeDefinition() == genericTypeDefinition);
	}

	public static bool IsImplementationOfGenericType(
		this Type type, Type genericTypeDefinition)
	{
		if (type == null)
			throw new ArgumentNullException(nameof(type));
		if (genericTypeDefinition == null)
			throw new ArgumentNullException(nameof(genericTypeDefinition));

		return type.IsAnyTypeInHierarchy(t => t.IsDirectImplementationOfGenericType(genericTypeDefinition));
	}

	public static bool IsAnyTypeInHierarchy(this Type type, Predicate<Type> predicate)
	{
		if (type == null)
			throw new ArgumentNullException(nameof(type));

		var currentType = type;
		while (currentType != null)
		{
			if (predicate(currentType))
			{
				return true;
			}
			currentType = currentType.BaseType;
		}
		return false;
	}
}

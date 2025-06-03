<Query Kind="Program">
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

void Main()
{
	throw new NotSupportedException("Go away!");
}

public static class HierarchyExtensions
{
	public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
		this ICollection<TEntity> allItems,
		Func<TEntity, TProperty> idProperty,
		Func<TEntity, TProperty> parentIdProperty)
		where TEntity : class
	{
		return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, 0);
	}


	private static IEnumerable<HierarchyNode<TEntity>> CreateHierarchy<TEntity, TProperty>(
		ICollection<TEntity> allItems,
		TEntity parentItem,
		Func<TEntity, TProperty> idProperty,
		Func<TEntity, TProperty> parentIdProperty,
		int depth)
		where TEntity : class
	{
		var children = parentItem == null
			? allItems.Where(entity => Equals(parentIdProperty(entity), default(TProperty)))
			: allItems.Where(entity => Equals(parentIdProperty(entity), idProperty(parentItem)));
		var childDepth = depth + 1;
		return children.Select(
			child => new HierarchyNode<TEntity>
			{
				Entity = child,
				ChildNodes = CreateHierarchy(allItems, child, idProperty, parentIdProperty, childDepth),
				Depth = childDepth
			});
	}
}

public sealed record HierarchyNode<T> where T : class
{
	public T Entity { get; init; }
	public IEnumerable<HierarchyNode<T>> ChildNodes { get; init; }
	public int Depth { get; init; }
}
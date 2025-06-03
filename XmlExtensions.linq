<Query Kind="Program">
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

void Main()
{
	var xdocument = new XDocument(
		new XElement(
			"root",
			new XElement(
				"child",
				new XAttribute("attribute", "value"))));
	
	xdocument.Dump(nameof(xdocument));
	
	var childElement = xdocument.GetSingleElementByXPath("/root/child");
	childElement.Dump(nameof(childElement));
}

public static class XmlExtensions
{
	public static XElement GetSingleElementByXPath(this XNode node, string xpathExpression)
	{
		return node.XPathSelectElement(xpathExpression)
			?? throw new InvalidOperationException($"Element not found by xpath \"{xpathExpression}\".");
	}

	public static XElement GetExistingElement(this XContainer container, XName elementName)
	{
		var result = container.Element(elementName);
		if (result == null)
			throw new InvalidOperationException($"Element '{elementName}' not found");
		return result;
	}
	
	public static XAttribute GetExistingAttribute(this XElement element, XName attributeName)
	{
		var result = element.Attribute(attributeName);
		if (result == null)
			throw new InvalidOperationException($"Element does not have attribute '{attributeName}'.");
		return result;
	}
}
namespace Boilerplate.Records;

public record Product(Guid ProductId, string ProductName, List<string> Features, decimal RetailPrice)
{
    public override string ToString() => ProductName;
};
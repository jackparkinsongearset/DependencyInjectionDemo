namespace Boilerplate.Records;

public record Sale(
    Guid SaleId,
    Customer Customer,
    Dictionary<Product, int> ProductsBoughtWithQuantity,
    DateTime TransactionDate)
{
    public override string ToString() => $"{SaleId}__{Customer}";
    public decimal TotalCost => ProductsBoughtWithQuantity.Sum(
        product => product.Key.RetailPrice * product.Value);
};
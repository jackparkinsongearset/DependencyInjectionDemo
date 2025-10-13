using Boilerplate.Interfaces;
using Boilerplate.Records;

namespace Boilerplate.Classes.stages;

class SalesProcessorV2: ISalesProcessor
{
    private readonly ISalesDatabase _salesDatabase;
    private readonly IProductDatabase _productDatabase;

    public SalesProcessorV2(ISalesDatabase salesDatabase, IProductDatabase productDatabase)
    {
        _salesDatabase = salesDatabase;
        _productDatabase = productDatabase;
    }

    public IDictionary<Sale, bool> ProcessSales(IEnumerable<Sale> sales)
    {
        IDictionary<Sale, bool> results = new Dictionary<Sale, bool>();
        foreach (var sale in sales)
        {
            var canReserveStock = _productDatabase.TryToReserveStockForSale(sale);
            if (canReserveStock) {
                _salesDatabase.SaveSale(sale);
            }
            results.Add(sale, canReserveStock);
        }
        return results;
    }
}
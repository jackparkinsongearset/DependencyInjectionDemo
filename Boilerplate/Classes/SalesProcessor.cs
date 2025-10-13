using Boilerplate.Interfaces;
using Boilerplate.Records;

namespace Boilerplate.Classes;

public class SalesProcessor: ISalesProcessor
{
    private readonly ISalesDatabase _salesDatabase;
    private readonly IProductDatabase _productDatabase;

    public SalesProcessor()
    {
        // Tightly coupled dependencies - this causes issues for testing and flexibility
        // since the implementations are hardcoded.
        // We can use Dependency Injection to improve flexibility through Inversion of Control.
        _salesDatabase = new SalesDatabase(); 
        _productDatabase = new ProductDatabase();
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
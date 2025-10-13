using Boilerplate.Interfaces;
using Boilerplate.Records;

namespace Boilerplate.Classes.Mock;

public class TestProductDatabase: IProductDatabase
{
    public bool TryToReserveStockForSale(Sale sale)
    {
        Console.WriteLine($"Pretending to reserve stock for sale {sale}...");
        return true;
    }
}
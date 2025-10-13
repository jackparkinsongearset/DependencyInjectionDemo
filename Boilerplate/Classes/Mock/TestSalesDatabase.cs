using Boilerplate.Interfaces;
using Boilerplate.Records;

namespace Boilerplate.Classes.Mock;

public class TestSalesDatabase: ISalesDatabase
{
    public void SaveSale(Sale sale)
    {
        Console.WriteLine($"Pretending to save sale {sale}...");
    }
}
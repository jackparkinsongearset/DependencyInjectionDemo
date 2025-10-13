using Boilerplate.Interfaces;
using Boilerplate.Records;

namespace Boilerplate.Classes;

public class SalesDatabase: ISalesDatabase
{
    public void SaveSale(Sale sale)
    {
        Console.WriteLine($"Saving sale {sale} in the prod database, this is an expensive operation...");
    }
}
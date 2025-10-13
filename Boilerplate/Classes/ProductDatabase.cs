using Boilerplate.Interfaces;
using Boilerplate.Records;

namespace Boilerplate.Classes;

public class ProductDatabase: IProductDatabase
{
    public bool TryToReserveStockForSale(Sale sale)
    {
        Console.WriteLine($"Try to reserve stock for sale {sale} - this is an expensive operation...");
        
        // If buying three or more of any item pretend we don't have enough stock
        var productsOutOfStock = sale.ProductsBoughtWithQuantity
            .Where(pair => pair.Value > 2)
            .Select(pair => pair.Key)
            .ToList();
          
        if (productsOutOfStock.Count != 0) {
            Console.WriteLine($"Not enough stock for items {string.Join(", ", productsOutOfStock)}.");
            return false;
        }
          
        Console.WriteLine($"Reserving stock for products bought in sale {sale}.");
        return true;
    }
}
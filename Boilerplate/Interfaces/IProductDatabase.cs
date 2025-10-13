using Boilerplate.Records;

namespace Boilerplate.Interfaces;

public interface IProductDatabase { 
    bool TryToReserveStockForSale(Sale sale);
}
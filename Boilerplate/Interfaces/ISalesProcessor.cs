using Boilerplate.Records;

namespace Boilerplate.Interfaces;

public interface ISalesProcessor { 
    IDictionary<Sale, bool> ProcessSales(IEnumerable<Sale> sales); 
} 
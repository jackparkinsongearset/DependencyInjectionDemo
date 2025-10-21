using Boilerplate.Interfaces;
using Boilerplate.Records;

namespace Core;

public static class MainWithIOC
{
    public static void Main(string[] args)
    {
        var container = ContainerRegistrationService.CreateDIContainer();

        var mainApp = container.Resolve<ISalesProcessor>();

        var exampleCustomer = new Customer(Guid.NewGuid(), FirstName: "Foo", LastName: "Bar", Email: "foo.bar@email.com");
        var exampleProduct = new Product(Guid.NewGuid(), ProductName: "Example Product", Features: [], RetailPrice: 19.99m);
        var numberOfProductBought = new Dictionary<Product, int> { { exampleProduct, 2 } };
        var exampleSale = new Sale(Guid.NewGuid(), exampleCustomer, numberOfProductBought, DateTime.UtcNow);
        
        var result = mainApp.ProcessSales([exampleSale]);
        
        Console.WriteLine($"Sale {exampleSale} was processed successfully: {result[exampleSale]}");
    }
}
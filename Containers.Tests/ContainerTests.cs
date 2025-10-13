using Boilerplate.Classes;
using Boilerplate.Interfaces;
using Boilerplate.Records;

namespace Containers.Tests;

[TestFixture]
public class ContainerTests
{
    // Container tests to show that our class can enable Inversion of Control (IOC)
 
    // Tests 1: Can resolve a simple type with a parameterless constructor (i.e. SalesDatabase)
    [Test]
    public void Container_CanResolveConcreteTypeWithParameterlessConstructor()
    {
        var container = new Container();
        var db = container.Resolve<SalesDatabase>();
        Assert.That(db, Is.Not.Null);
    }
    
    [Test]
    public void Container_CanResolveInterfaceTypeUsingProvidedConcreteTypeWithParameterlessConstructor()
    {
        var container = new Container();
        container.Register<ISalesDatabase, SalesDatabase>();
        var db = container.Resolve<ISalesDatabase>();
        Assert.That(db, Is.Not.Null);
    }
    
    [Test]
    public void Container_CanResolveInterfaceTypeUsingProvidedConcreteTypeWithDependency()
    {
        var container = new Container();
        container.Register<ISalesDatabase, SalesDatabase>();
        container.Register<IProductDatabase, ProductDatabase>();
        var processor = container.Resolve<SalesProcessor>();
        Assert.That(processor, Is.Not.Null);
    }

    // Test 3: Ensure Container can correctly instantiate a working SalesProcessor object
    [Test]
    public void SalesProcessor_ProcessesSalesCorrectly()
    {
        var container = new Container();
        container.Register<ISalesDatabase, SalesDatabase>();
        container.Register<IProductDatabase, ProductDatabase>();
        var salesProcessor = container.Resolve<SalesProcessor>();
            
        var productA = new Product(Guid.NewGuid(), "Laptop", ["Fast", "Lightweight"], 1500m); 
        var productB = new Product(Guid.NewGuid(), "Mouse", ["Ergonomic"], 50m); 
        var productC = new Product(Guid.NewGuid(), "Keyboard", ["Mechanical"], 120m); 
            
        var customer1 = new Customer(Guid.NewGuid(), "Jane", "Doe", "jane.doe@example.com");

        var sale1 = new Sale(Guid.NewGuid(), customer1,
            new Dictionary<Product, int> { { productA, 1 }, { productB, 2 } }, DateTime.Now);
        var sale2 = new Sale( Guid.NewGuid(), customer1, new Dictionary<Product, int> { { productC, 3 } }, DateTime.Now);

        List<Sale> sales = [sale1, sale2];
            
        var result = salesProcessor.ProcessSales(sales); 

        Assert.That(result[sale1], Is.True);
        Assert.That(result[sale2], Is.False);
    }
}
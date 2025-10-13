using Boilerplate.Classes;
using Boilerplate.Classes.Mock;
using Boilerplate.Interfaces;
using Boilerplate.Records;

namespace Containers.Tests;

[TestFixture]
public class TestContainerTests
{
    // TestContainer tests to show that our class can create random test data programmatically using recursion and reflection.
    // Test 1: Can create common primitives with unique values
    [Theory]
    [TestCase(typeof(string))]
    [TestCase(typeof(int))]
    [TestCase(typeof(decimal))]
    [TestCase(typeof(bool))]
    [TestCase(typeof(char))]
    public void TestContainer_CreatesPrimitiveTypeWithRandomData(Type type)
    {
        // Verify that the container can create an object of a given type and that its properties are not default.
        var fixture = new TestContainer();
        var instance1 = fixture.Create(type);
        var instance2 = fixture.Create(type);

        Assert.Multiple(() =>
        {
            Assert.That(instance1, Is.Not.Null);
            Assert.That(instance1, Is.TypeOf(type));
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(instance2, Is.Not.Null);
            Assert.That(instance2, Is.TypeOf(type));
        });

        // Ensure uniqueness (flaky test, not guaranteeing uniqueness here)
        // bool can only be true or false, so is skipped
        if (type != typeof(bool))
        {
            Assert.That(instance1, Is.Not.EqualTo(instance2));
        }

    }

    // Test 2: Can create basic structs with unique values
    [Theory]
    [TestCase(typeof(Guid))]
    [TestCase(typeof(DateTime))]
    public void TestContainer_CreatesTypeWithRandomData(Type type)
    {
        var fixture = new TestContainer();
        var instance1 = fixture.Create(type);
        var instance2 = fixture.Create(type);

        Assert.Multiple(() =>
        {
            Assert.That(instance1, Is.Not.Null);
            Assert.That(instance1, Is.TypeOf(type));
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(instance2, Is.Not.Null);
            Assert.That(instance2, Is.TypeOf(type));
        });

        // Ensure uniqueness (flaky test, not guaranteeing uniqueness here)
        Assert.That(instance1, Is.Not.EqualTo(instance2));
    }

    // Test 3: Can create user-defined classes with unique values
    [Test]
    public void TestContainer_CreateUserDefinedClass_Customer_WithRandomData() 
    {
        var fixture = new TestContainer();
        var instance1 = fixture.Create<Customer>();
        var instance2 = fixture.Create<Customer>();

        Assert.Multiple(() =>
        {
            Assert.That(instance1, Is.Not.Null);
            Assert.That(instance1, Is.TypeOf<Customer>());
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(instance2, Is.Not.Null);
            Assert.That(instance2, Is.TypeOf<Customer>());
        });

        // Ensure uniqueness (flaky test, not guaranteeing uniqueness here)
        Assert.That(instance1, Is.Not.EqualTo(instance2));

        Assert.Multiple(() =>
        {
            // Ensure all values are correctly populated
            Assert.That(instance1.CustomerId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(instance1.FirstName, Is.Not.Empty);
            Assert.That(instance1.LastName, Is.Not.Empty);
            Assert.That(instance1.Email, Is.Not.Empty);
        });
            
        Assert.Multiple(() =>
        {
            Assert.That(instance2.CustomerId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(instance2.FirstName, Is.Not.Empty);
            Assert.That(instance2.LastName, Is.Not.Empty);
            Assert.That(instance2.Email, Is.Not.Empty);
        });
    }
    
    // Test 4: Can create objects with nullable fields and enumerable fields uniquely
    [Test]
    public void TestContainer_CreatesClassWithNullableAndEnumerableProperties_Product_WithRandomData()
    {
        var fixture = new TestContainer();
        var instance1 = fixture.Create<Product>();
        var instance2 = fixture.Create<Product>();

        Assert.Multiple(() =>
        {
            Assert.That(instance1, Is.Not.Null);
            Assert.That(instance1, Is.TypeOf<Product>());
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(instance2, Is.Not.Null);
            Assert.That(instance2, Is.TypeOf<Product>());
        });

        // Ensure uniqueness (flaky test, not guaranteeing uniqueness here)
        Assert.That(instance1, Is.Not.EqualTo(instance2));

        // Ensure all values are correctly populated
        Assert.Multiple(() =>
        {
            Assert.That(instance1.ProductId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(instance1.ProductName, Is.Not.Empty);
            Assert.That(instance1.Features, Is.Not.Empty);
            Assert.That(instance1.RetailPrice, Is.Not.EqualTo(0));
        });

        Assert.Multiple(() =>
        {
            Assert.That(instance2.ProductId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(instance2.ProductName, Is.Not.Empty);
            Assert.That(instance2.Features, Is.Not.Empty);
            Assert.That(instance2.RetailPrice, Is.Not.EqualTo(0));
        });
    }

    // Test 5: Can create objects containing other user-defined objects as properties (+ dictionary field) uniquely
    [Test]
    public void TestContainer_CreatesClassWithNullableAndEnumerableProperties_Sale_WithRandomData()
    {
        var fixture = new TestContainer();
        var instance1 = fixture.Create<Sale>();
        var instance2 = fixture.Create<Sale>();

        Assert.Multiple(() =>
        {
            Assert.That(instance1, Is.Not.Null);
            Assert.That(instance1, Is.TypeOf<Sale>());
        });

        Assert.Multiple(() =>
        {
            Assert.That(instance2, Is.Not.Null);
            Assert.That(instance2, Is.TypeOf<Sale>());
        });
        
        // Ensure uniqueness (flaky test, not guaranteeing uniqueness here)
        Assert.That(instance1, Is.Not.EqualTo(instance2));

        // Ensure all values are correctly populated
        Assert.Multiple(() =>
        {
            Assert.That(instance1.SaleId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(instance1.Customer, Is.Not.Null);
            Assert.That(instance1.ProductsBoughtWithQuantity, Is.Not.Empty);
        });

        Assert.Multiple(() =>
        {
            Assert.That(instance2.SaleId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(instance2.Customer, Is.Not.Null);
            Assert.That(instance2.ProductsBoughtWithQuantity, Is.Not.Empty);
        });
    }

    // Test 6: CreateMany creates correct number of objects
    [Test]
    public void TestContainer_CreateMany_CreatesCorrectCountOfUniqueObjects()
    {
        // Verify that calling CreateMany multiple times results in a list of unique objects.
        var fixture = new TestContainer();
        var sales = fixture.CreateMany<Sale>(7);

        Assert.Multiple(() =>
        {
            Assert.That(sales.Count, Is.EqualTo(7));
            Assert.That(sales.Select(s => s.SaleId).Distinct().Count(), Is.EqualTo(7));
        });
    }
    
    // Test 7: Freezing an instance ensures the same instance is returned on subsequent calls to Create
    [Test]
    public void TestContainer_Freeze_EnsuresSameInstanceIsReturned()
    {
        var testContainer = new TestContainer();
        var frozenCustomer = testContainer.Create<Customer>();
        testContainer.Freeze(frozenCustomer);
        
        var createdCustomer1 = testContainer.Create<Customer>();
        var createdCustomer2 = testContainer.Create<Customer>();

        Assert.Multiple(() =>
        {
            Assert.That(createdCustomer1, Is.SameAs(frozenCustomer));
            Assert.That(createdCustomer2, Is.SameAs(frozenCustomer));
        });
    }
    
    // Copied container tests to show that our class can enable Inversion of Control (IOC) and be used in place of the original Container class
 
    // Test 8: Can resolve a simple type with a parameterless constructor (i.e. SalesDatabase)
    [Test]
    public void Container_CanResolveConcreteTypeWithParameterlessConstructor()
    {
        var container = new Container();
        var db = container.Resolve<TestSalesDatabase>();
        Assert.That(db, Is.Not.Null);
    }
    
    [Test]
    public void Container_CanResolveInterfaceTypeUsingProvidedConcreteTypeWithParameterlessConstructor()
    {
        var container = new Container();
        container.Register<ISalesDatabase, TestSalesDatabase>();
        var db = container.Resolve<ISalesDatabase>();
        Assert.That(db, Is.Not.Null);
    }

    // Tests 9: Can resolve a type with a dependency (i.e. SalesProcessor)
    [Test]
    public void Container_CanResolveConcreteTypeWithDependency()
    {
        var container = new Container();
        container.Register<ISalesDatabase, TestSalesDatabase>();
        container.Register<IProductDatabase, TestProductDatabase>();
        var processor = container.Resolve<SalesProcessor>();
        Assert.That(processor, Is.Not.Null);
    }
    
    [Test]
    public void Container_CanResolveInterfaceTypeUsingProvidedConcreteTypeWithDependency()
    {
        var container = new Container();
        container.Register<ISalesDatabase, TestSalesDatabase>();
        container.Register<IProductDatabase, TestProductDatabase>();
        container.Register<ISalesProcessor, SalesProcessor>();
        var processor = container.Resolve<SalesProcessor>();
        Assert.That(processor, Is.Not.Null);
    }

    // Test 10: Ensure Container can correctly instantiate a working SalesProcessor object
    [Test]
    public void SalesProcessor_ProcessesSalesCorrectly()
    {
        var testContainer = new TestContainer();
        testContainer.Register<ISalesDatabase, TestSalesDatabase>();
        testContainer.Register<IProductDatabase, TestProductDatabase>();
        var salesProcessor = testContainer.Resolve<SalesProcessor>();
            
        var productA = new Product(Guid.NewGuid(), "Laptop", ["Fast", "Lightweight"], 1500m); 
        var productB = new Product(Guid.NewGuid(), "Mouse", ["Ergonomic"], 50m); 
        var productC = new Product(Guid.NewGuid(), "Keyboard", ["Mechanical"], 120m); 
            
        var customer1 = new Customer(Guid.NewGuid(), "Jane", "Doe", "jane.doe@example.com");

        var sale1 = new Sale(Guid.NewGuid(), customer1,
            new Dictionary<Product, int> { { productA, 1 }, { productB, 2 } }, DateTime.Now);
        var sale2 = new Sale( Guid.NewGuid(), customer1, new Dictionary<Product, int> { { productC, 3 } }, DateTime.Now);

        List<Sale> sales = [sale1, sale2];
            
        var result = salesProcessor.ProcessSales(sales);

        Assert.Multiple(() =>
        {
            Assert.That(result[sale1], Is.True);
            Assert.That(result[sale2], Is.True); // Mocked database always has stock
        });
    }
    
    // Test 11: Show that TestContainer can correctly instantiate a working SalesProcessor object as well as instantiate the test data needed for this test
    [Test]
    public void ProductDatabase_AllowsTestingStockLogic_WithGeneratedTestData()
    {
        var testContainer = new TestContainer();
        testContainer.Register<ISalesDatabase, TestSalesDatabase>();
        testContainer.Register<IProductDatabase, TestProductDatabase>();
        var salesProcessor = testContainer.Resolve<SalesProcessor>();
        
        var sales = testContainer.CreateMany<Sale>(2);
            
        var result = salesProcessor.ProcessSales(sales); 

        Assert.That(result[sales[0]], Is.True);
        Assert.That(result[sales[1]], Is.True);
    }
    
    // Test 12: Show that product logic can be tested with generated test data
    [Test]
    public void ProductDatabase_ProductsWithValidStockQuantities_SaleVerified_WithGeneratedTestData()
    {
        var testContainer = new TestContainer();
        var productDatabase = testContainer.Resolve<ProductDatabase>();

        // Create list of products with quantities less than 3
        var soldProductsForSale = ((int[])[1, 2, 2]).ToDictionary(i => testContainer.Create<Product>());
        
        var sale = testContainer.Create<Sale>() with { ProductsBoughtWithQuantity = soldProductsForSale };
            
        var result = productDatabase.TryToReserveStockForSale(sale); 

        Assert.That(result, Is.True);
    }
    
    [Test]
    public void ProductDatabase_ProductsWithInvalidStockQuantities_SaleNotVerified_WithGeneratedTestData()
    {
        var testContainer = new TestContainer();
        var productDatabase = testContainer.Resolve<ProductDatabase>();

        // Create list of products with quantities less than 3
        var soldProductsForSale = ((int[])[1, 2, 4]).ToDictionary(i => testContainer.Create<Product>());
        
        var sale = testContainer.Create<Sale>() with { ProductsBoughtWithQuantity = soldProductsForSale };
            
        var result = productDatabase.TryToReserveStockForSale(sale); 

        Assert.That(result, Is.False);
    }
}
using Boilerplate.Classes;
using Boilerplate.Interfaces;
using Containers;

namespace Core;

public class ContainerRegistrationService
{
    public static Container CreateDIContainer()
    {
        var container = new Container();
        
        // Register services
        container.Register<ISalesProcessor, SalesProcessor>();
        
        // Register databases
        container.Register<ISalesDatabase, SalesDatabase>();
        container.Register<IProductDatabase, ProductDatabase>();
        
        return container;
    }
}
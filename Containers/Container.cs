using System.Diagnostics.CodeAnalysis;

namespace Containers;

public class Container
{
    private readonly Dictionary<Type, Type> _registrations = new();

    public void Register<TInterface, TConcrete>() where TConcrete : TInterface
    {
        throw new NotImplementedException();
    }

    public T Resolve<T>()
    {
        throw new NotImplementedException();
    }

    private static bool IsConcrete(Type type) => type is { IsClass: true, IsAbstract: false, IsInterface: false };
    
    private bool CanImplementType(Type type, [MaybeNullWhen(false)] out Type implementationType)
    {
        throw new NotImplementedException();
    }

    // Virtual ensures that recursive calls will use the overridden method in derived classes
    protected virtual object Resolve(Type type)
    {
        throw new NotImplementedException();
    }
}
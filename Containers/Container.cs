using System.Diagnostics.CodeAnalysis;

namespace Containers;

public class Container
{
    private readonly Dictionary<Type, Type> _registrations = new();

    public void Register<TInterface, TConcrete>() where TConcrete : TInterface
    {
        _registrations[typeof(TInterface)] = typeof(TConcrete);
    }

    public T Resolve<T>()
    {
        return (T)Resolve(typeof(T));
    }

    private static bool IsConcrete(Type type) => type is { IsClass: true, IsAbstract: false, IsInterface: false };
    
    private bool CanImplementType(Type type, [MaybeNullWhen(false)] out Type implementationType)
    {
        if (_registrations.TryGetValue(type, out implementationType!))
        {
            return true;
        }

        // If the type is concrete, we can use it directly
        if (IsConcrete(type))
        {
            implementationType = type;
            return true;
        }

        implementationType = null;
        return false;
    }

    // Virtual ensures that recursive calls will use the overridden method in derived classes
    protected virtual object Resolve(Type type)
    {
        if (!CanImplementType(type, out var implementationType))
        {
            throw new InvalidOperationException($"Type {type.Name} not registered.");
        }
        
        var ctor = implementationType.GetConstructors().Single();
        var parameters = ctor.GetParameters()
            .Select(p => Resolve(p.ParameterType))
            .ToArray();

        return Activator.CreateInstance(implementationType, parameters);
    }
}
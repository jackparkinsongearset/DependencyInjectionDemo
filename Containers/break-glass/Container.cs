using System.Diagnostics.CodeAnalysis;

namespace Containers.break_glass;

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
        if (IsConcrete(type))
        {
            implementationType = type;
            return true;
        }
        
        if (_registrations.TryGetValue(type, out var registeredType))
        {
            implementationType = registeredType;
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
            throw new InvalidOperationException($"Type {type.FullName} is not registered and cannot be instantiated.");
        }

        var constructor = implementationType.GetConstructors().OrderBy(c => c.GetParameters().Length).FirstOrDefault();
        if (constructor == null)
        {
            throw new InvalidOperationException($"No public constructors found for type {implementationType.FullName}.");
        }

        var parameters = constructor.GetParameters();
        var parameterInstances = parameters
            .Select(p => Resolve(p.ParameterType))
            .ToArray();
        return Activator.CreateInstance(implementationType, parameterInstances)!;
    }
}
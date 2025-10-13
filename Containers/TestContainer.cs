using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Containers;

public class TestContainer : Container
{
    private readonly Dictionary<Type, object> _mockInstances = new();
    private readonly Random _random = new();

    public void Freeze<T>(T instance) where T: notnull
    {
        _mockInstances[typeof(T)] = instance;
    }

    public T Create<T>()
    {
        return (T)Resolve(typeof(T));
    }

    // For testing this class only
    public object Create(Type type)
    {
        return Resolve(type);
    }

    public List<T> CreateMany<T>(int count)
    {
        return (List<T>)ResolveCollection(typeof(List<T>), count)!;
    }

    protected override object Resolve(Type type)
    {
        if (_mockInstances.TryGetValue(type, out var mockedInstance)) return mockedInstance;

        // Check for specific generic types
        if (type.IsGenericType)
        {
            var genericTypeDefinition = type.GetGenericTypeDefinition();

            // Handle nullable types
            if (genericTypeDefinition == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type)!;
            }

            // Handle collections (List and Dictionary)
            var resolvedCollection = ResolveCollection(type, _random.Next(2, 6));
            if (resolvedCollection != null) return resolvedCollection;
        }
        
        // Handle interfaces and abstract classes using base implementation
        if (type.IsInterface || type.IsAbstract)
        {
            return base.Resolve(type);
        }

        // Handle primitive types and common structs with one-line returns
        if (ResolvePrimitivesAndStructs(type, out var concreteInstance)) return concreteInstance;

        // Handle classes and records - note only handles one constructor classes/records at present
        var ctor = type.GetConstructors().Single();
        var parameters = ctor.GetParameters().Select(p => Resolve(p.ParameterType)).ToArray();
        return Activator.CreateInstance(type, parameters)!;
    }

    private bool ResolvePrimitivesAndStructs(Type type, [MaybeNullWhen(false)] out object instance)
    {
        instance = type switch
        {
            _ when type == typeof(Guid) => Guid.NewGuid(),
            _ when type == typeof(string) => Guid.NewGuid().ToString("N"),
            _ when type == typeof(int) => _random.Next(int.MinValue, int.MaxValue),
            _ when type == typeof(byte) => (byte)_random.Next(byte.MinValue, byte.MaxValue + 1),
            _ when type == typeof(decimal) => (decimal)_random.NextDouble() * 1_000_000,
            _ when type == typeof(bool) => _random.Next(2) == 0,
            _ when type == typeof(char) => (char)_random.Next(33, 126),
            _ when type == typeof(DateTime) => new DateTime(DateTime.UtcNow.Ticks + _random.Next(-1_000_000, 1_000_000)),
            _ => null
        };
    
        return instance != null;
    }

    private object? ResolveCollection(Type type, int count)
    {
        var genericTypeDefinition = type.GetGenericTypeDefinition();
        var genericArguments = type.GetGenericArguments();
        
        // 1. Handle List<T>
        if (genericTypeDefinition == typeof(List<>) && genericArguments.Length == 1)
        {
            var elementType = genericArguments.Single();
            var list = (IList)Activator.CreateInstance(type)!;

            // Populate the list
            for (var i = 0; i < count; i++)
            {
                list.Add(Resolve(elementType));
            }
            return list;
        }
    
        // 2. Handle Dictionary<TKey, TValue>
        if (genericTypeDefinition == typeof(Dictionary<,>) && genericArguments.Length == 2)
        {
            var keyType = genericArguments[0];
            var valueType = genericArguments[1];
        
            // Create the specific Dictionary<TKey, TValue> instance and cast it to the non-generic IDictionary
            var dictionary = (IDictionary)Activator.CreateInstance(type)!;

            // Populate the dictionary
            for (var i = 0; i < count; i++)
            {
                dictionary.Add(Resolve(keyType), Resolve(valueType));
            }
            return dictionary;
        }

        return null;
    }
}
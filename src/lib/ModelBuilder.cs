using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace modeling;

public class ModelBuilder
{

    public static Schema Create(Type serviceType)
    {
        var builder = new ModelBuilder();
        return builder.Build(serviceType);
    }

    public Schema Build(Type serviceType) //  params Type[] types)
    {
        var dependencies = serviceType.GetProperties().Select(p => p.PropertyType.Flatten());
        var elements = ProcessTypesAndDependencies(dependencies);

        Service service = CreateService(serviceType);
        return new Schema(service, new SchemaElementCollection(elements));
    }


    private IEnumerable<ISchemaElement> ProcessTypesAndDependencies(IEnumerable<Type> types)
    {
        var queue = new ProcessingQueue<Type>(types);
        while (queue.TryDequeue(out var type))
        {
            if (type.IsEnum)
            {
                yield return CreateEnum(type);
            }
            else
            {
                foreach (var prop in type.GetProperties())
                {
                    var pt = prop.PropertyType.Flatten();
                    if (pt != null && !BuildInTypes.Contains(pt) && !(pt.Namespace?.StartsWith("System") ?? false))
                    {
                        queue.Enqueue(pt);
                    }
                }
                yield return CreateType(type);
            }
        }
    }

    private Service CreateService(Type serviceType)
    {
        var props = serviceType.GetProperties().Select(p => CreateProperty(p));
        return new Service(new Properties(props));
    }

    private StructuredType CreateType(Type type)
    {
        var props = new Properties(type.GetProperties().Select(p => CreateProperty(p)));
        var model = new StructuredType(type.Name, props);

        if (type.IsClass && !model.IsEntity)
        {
            Console.Error.WriteLine("ERROR: entity type {0} has no key(s) defined", type.Name);
        }
        else if (!type.IsClass && model.IsEntity)
        {
            Console.Error.WriteLine("ERROR: complex type {0} has key(s) defined", type.Name);
        }

        return model;
    }

    private EnumType CreateEnum(Type type)
    {
        var array = (int[])System.Enum.GetValues(type);
        var members = new Member[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            members[i] = new Member(System.Enum.GetName(type, array[i])!);
        }
        return new EnumType(type.Name, members);
    }

    private Property CreateProperty(System.Reflection.PropertyInfo prop)
    {
        var isKey = prop.GetCustomAttribute<KeyAttribute>() != null;

        if (prop.PropertyType.IsCollection(out var elementType))
        {
            if (elementType.IsGenericType)
            {
                throw new NotImplementedException("generic element type");
            }

            return new Property(prop.Name, elementType.Name) { IsMultiValue = true, IsKey = isKey };
        }
        else if (prop.PropertyType.IsGenericType)
        {
            throw new NotImplementedException("generic type");
        }
        else
        {
            return new Property(prop.Name, prop.PropertyType.Name) { IsKey = isKey };
        }
    }

}

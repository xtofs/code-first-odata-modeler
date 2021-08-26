using System.Collections;
using Microsoft.OData.Edm;

namespace modeling;

public class CsdlModelTransformer
{
    private readonly Dictionary<string, IEdmSchemaType> environment = new Dictionary<string, IEdmSchemaType>();

    public IEdmModel TransformSchema(Schema schema)
    {
        environment.Clear();
        var model = new EdmModel();

        foreach (var type in schema.Elements)
        {
            var edm = CreateSchemaElement(schema, type);
            environment.Add(type.Name, edm);
            model.AddElement(edm);
        }
        foreach (var type in schema.Elements)
        {
            if (type is StructuredType structured)
            {
                AddProperties(structured);
            }
        }

        CreateContainer(model, schema);

        return model;
    }

    private void AddProperties(StructuredType type)
    {
        foreach (var property in type.Properties)
        {
            if (environment.TryGetValue(type.Name, out var edmDefiningType))
            {
                if (edmDefiningType is EdmStructuredType edmDefiningStructuredType)
                {
                    AddProperty(property, edmDefiningStructuredType);
                }
                // TODO: ERROR
            }
            else
            {
                throw new NotSupportedException($"internal error, can't find type {type.Name}");
            }
        }
    }

    private void AddProperty(Property property, EdmStructuredType edmDefiningType)
    {
        switch (ResolvePropertyType(property))
        {
            case IEdmEntityType edmEntityPropertyType:
                var info = new EdmNavigationPropertyInfo
                {
                    Name = property.Name,
                    Target = edmEntityPropertyType,
                    TargetMultiplicity = property.IsMultiValue ? EdmMultiplicity.Many : EdmMultiplicity.One,
                    ContainsTarget = true // TODO
                };
                edmDefiningType.AddUnidirectionalNavigation(info);
                break;

            case IEdmComplexType edmComplexPropertyType:
                edmDefiningType.AddStructuralProperty(property.Name, new EdmComplexTypeReference(edmComplexPropertyType, false));
                break;

            case IEdmPrimitiveType edmPrimitivePropertyType:
                edmDefiningType.AddStructuralProperty(property.Name, new EdmPrimitiveTypeReference(edmPrimitivePropertyType, false));
                break;

            case IEdmEnumType edmEnumPropertyType:
                edmDefiningType.AddStructuralProperty(property.Name, new EdmEnumTypeReference(edmEnumPropertyType, false));
                break;
        }
    }

    private IEdmType ResolvePropertyType(Property property)
    {
        if (environment.TryGetValue(property.Type, out var edmPropertyType))
        {
            return edmPropertyType;
        }

        var kind = property.Type switch
        {
            "String" => EdmPrimitiveTypeKind.String,
            "Int32" => EdmPrimitiveTypeKind.Int32,
            "DateOnly" => EdmPrimitiveTypeKind.Date,
            "Decimal" => EdmPrimitiveTypeKind.Decimal,

            _ => throw new NotSupportedException($"not supported {property.Type}")
        };
        return EdmCoreModel.Instance.GetPrimitiveType(kind);
    }

    // if (environment.TryGetValue(property.Type, out var edmPropertyType))
    // {
    //     if (edmPropertyType is IEdmEntityType edmEntityPropertyType)
    //     {
    //         
    //     }
    //     else if (edmPropertyType is IEdmComplexType edmComplexPropertyType)
    //     {
    //         edmDefiningStructuredType.AddStructuralProperty(property.Name, new EdmComplexTypeReference(edmComplexPropertyType, false));
    //     }
    // }


    public IEdmSchemaType CreateSchemaElement(Schema schema, ISchemaElement element)
    {
        switch (element)
        {
            case EnumType @enum:
                return NewEnumType(schema, @enum);
            case StructuredType type:
                return (IEdmSchemaType)NewStructuredType(schema, type);
            default:
                throw new NotImplementedException();
        }
    }

    private EdmEnumType NewEnumType(Schema schema, EnumType @enum)
    {
        var edm = new EdmEnumType(schema.Namespace, @enum.Name);
        foreach (var (i, member) in @enum.Members.Enumerate())
        {
            edm.AddMember(member.Name, new EdmEnumMemberValue(i));
        }
        return edm;
    }

    private EdmStructuredType NewStructuredType(Schema schema, StructuredType type)
    {
        if (type.IsEntity)
        {
            var edm = new EdmEntityType(schema.Namespace, type.Name, null, false, type.IsOpen);
            return edm;
        }
        else
        {
            var edm = new EdmComplexType(schema.Namespace, type.Name, null, false, type.IsOpen);
            return edm;
        }
    }

    private EdmEntityContainer CreateContainer(EdmModel model, Schema schema)
    {
        var container = model.AddEntityContainer(schema.Namespace, schema.Service.Name);
        foreach (var property in schema.Service.Properties)
        {
            if (property.IsMultiValue)
            {
                // TODO check if it is an IEdmEntityType
                container.AddEntitySet(property.Name, environment[property.Type] as IEdmEntityType);
            }
            else
            {
                container.AddSingleton(property.Name, environment[property.Type] as IEdmEntityType);
            }
        }
        return container;
    }
}

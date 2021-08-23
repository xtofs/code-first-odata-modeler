using System.Collections;
using System.Xml;

namespace modeling;

public class CsdlXmlModelWriter : IModelWriter
{
    private bool disposedValue;

    private readonly XmlWriter writer;

    public CsdlXmlModelWriter(XmlWriter writer) => (this.writer) = (writer);

    public void WriteSchema(Schema schema)
    {
        //<edmx:Edmx xmlns:edmx="http://docs.oasis-open.org/odata/ns/edmx"
        //            Version="4.01">
        //   <edmx:DataServices>
        //     …
        //   </edmx:DataServices>
        // </edmx:Edmx>
        WriteElement("Schema", () =>
        {
            WriteSchemaElementCollection(schema.Elements);
            WriteService(schema.Service);
        });
    }

    public void WriteService(Service service)
    {
        WriteElement("EntityContainer", new Attributes { }, () =>
       {
           var first = true;
           foreach (var property in service.Properties)
           {
               if (first) { first = false; } else { }

               var tag = property.IsMultiValue ? "EntitySet" : "Singleton";
               WriteElement(tag, new Attributes { ["Name"] = property.Name, ["EntityType"] = property.Type });
           }
       });
    }

    public void WriteSchemaElement(ISchemaElement element)
    {
        switch (element)
        {
            case StructuredType type: WriteType(type); break;
            case EnumType @enum: WriteEnum(@enum); break;
            default: throw new NotImplementedException();
        }
    }

    public void WriteType(StructuredType type)
    {
        var tag = type.IsEntity ? "EntityType" : "ComplexType";
        WriteElement(tag, new Attributes { ["Name"] = type.Name }, () =>
        {
            if (type.IsEntity)
            {
                WriteElement("Keys", Attributes.Empty, () =>
                {
                    foreach (var key in type.Keys)
                    {
                        WriteElement("Key", new Attributes { ["Name"] = key });
                    }
                });
            }

            foreach (var prop in type.Properties)
            {
                WriteProperty(prop);
            }
        });
    }

    public void WriteSchemaElementCollection(SchemaElementCollection elements)
    {
        var first = true;
        foreach (var element in elements)
        {
            if (first) { first = false; } else { }
            WriteSchemaElement(element);
        }
    }

    public void WriteEnum(EnumType @enum)
    {
        WriteElement("Enum", new Attributes { ["Name"] = @enum.Name }, () =>
        {
            foreach (var member in @enum.Members)
            {
                WriteMember(member);
            }
        });
    }

    public void WriteMember(Member member)
    {
        WriteElement("Member", new Attributes { ["Name"] = member.Name });
    }

    public void WriteProperty(Property property)
    {
        var @ref = property.IsMultiValue ? $"Collection({property.Type})" : property.Type;
        var tag = property.isNavigation ? "NavigationProperty" : "Property";
        WriteElement(tag, new Attributes { ["Name"] = property.Name, ["Type"] = @ref });
    }

    // ##############################

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                writer.Dispose();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    // ##############################

    private void WriteElement(string name, Attributes attributes)
    {
        WriteElement(name, attributes, () => { });
    }

    private void WriteElement(string name, Action action)
    {
        WriteElement(name, Attributes.Empty, action);
    }

    private void WriteElement(string name, Attributes attributes, Action action)
    {
        writer.WriteStartElement(name);
        foreach (var attribute in attributes)
        {
            writer.WriteAttributeString(attribute.Key, attribute.Value.ToString());
        }
        action();
        writer.WriteEndElement();
    }

    class Attributes : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public void Add(string key, string value) => dictionary.Add(key, value);

        public object this[string key] { get => dictionary[key]; set => dictionary.Add(key, value); }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => dictionary.GetEnumerator();

        public static Attributes Empty => new Attributes();
    }
}
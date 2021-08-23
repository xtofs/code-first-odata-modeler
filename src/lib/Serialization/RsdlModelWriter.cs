namespace modeling;

public class RsdlModelWriter : IModelWriter
{
    private bool disposedValue;

    private readonly System.CodeDom.Compiler.IndentedTextWriter writer;

    public RsdlModelWriter(TextWriter writer) => (this.writer) = (new System.CodeDom.Compiler.IndentedTextWriter(writer));

    public void WriteSchema(Schema schema)
    {
        var first = true;
        foreach (var element in schema.Elements)
        {
            if (first) { first = false; } else { writer.WriteLine(); }
            WriteSchemaElement(element);
        }

        if (!first) { writer.WriteLine(); }
        WriteService(schema.Service);

        writer.Flush();
    }

    public void WriteService(Service service)
    {
        writer.Write("service");
        writer.Block(() =>
        {
            var first = true;
            foreach (var prop in service.Properties)
            {
                if (first) { first = false; } else { }
                WriteProperty(prop);
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
        writer.Write("type {0}", type.Name);
        writer.Block(() =>
        {
            foreach (var prop in type.Properties)
            {

                WriteProperty(prop);
            }
        });
    }

    public void WriteEnum(EnumType @enum)
    {
        writer.Write("enum {0}", @enum.Name);
        writer.Block(() =>
        {
            foreach (var member in @enum.Members)
            {
                WriteMember(member);
            }
        });
    }

    public void WriteMember(Member member)
    {
        writer.WriteLine("{0}", member.Name);
    }

    public void WriteProperty(Property property)
    {
        var @ref = property.IsMultiValue ? $"[{property.Type}]" : property.Type;
        writer.WriteLine("{0}: {1}", property.Name, @ref);
    }


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
}

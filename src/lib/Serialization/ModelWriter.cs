using System.CodeDom.Compiler;
using System.Xml;

namespace modeling;

public interface IModelWriter : IDisposable
{
    void WriteSchema(Schema schema);
    void WriteService(Service service);
    void WriteSchemaElement(ISchemaElement element);
    void WriteType(StructuredType type);
    void WriteEnum(EnumType @enum);
    void WriteMember(Member member);
    void WriteProperty(Property property);
}

public enum ModelFormat { RSDL, CSDL_XML, CSDL_JSON };

public static class ModelWriter
{
    public static IModelWriter Create(ModelFormat format, Stream stream)
    {
        switch (format)
        {
            case ModelFormat.RSDL: return new RsdlModelWriter(new StreamWriter(stream));
            case ModelFormat.CSDL_XML: return new CsdlXmlModelWriter(XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }));
            default: throw new NotImplementedException();
        }
    }
}

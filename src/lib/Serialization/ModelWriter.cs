using System.CodeDom.Compiler;
using System.Xml;

namespace modeling;

public interface IModelWriter : IDisposable
{
    void WriteSchema(Schema schema);
}

public enum ModelFormat { RSDL, CSDL_XML, CSDL_JSON };

public static class ModelWriter
{
    public static IModelWriter Create(ModelFormat format, Stream stream)
    {
        switch (format)
        {
            case ModelFormat.RSDL: return new RsdlModelWriter(new StreamWriter(stream));
            // case ModelFormat.CSDL_XML: return new CsdlXmlModelWriter(XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }));
            case ModelFormat.CSDL_XML: return new CsdlXmlModelWriter(XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }), new CsdlModelTransformer());
            default: throw new NotImplementedException();
        }
    }
}

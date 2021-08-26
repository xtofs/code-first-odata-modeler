using System.Xml;
using Microsoft.OData.Edm.Csdl;

namespace modeling;

public class CsdlXmlModelWriter : IModelWriter
{
    private bool disposedValue;

    private readonly XmlWriter writer;

    private readonly CsdlModelTransformer transformer;

    public CsdlXmlModelWriter(XmlWriter writer, CsdlModelTransformer transformer) => (this.writer, this.transformer) = (writer, transformer);

    public void WriteSchema(Schema schema)
    {
        var edm = transformer.TransformSchema(schema);

        CsdlWriter.TryWriteCsdl(edm, writer, CsdlTarget.OData, out var errors);
    }

    #region Dispose

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

    #endregion
}
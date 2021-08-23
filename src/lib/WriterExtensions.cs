namespace modeling;

internal static class WriterExtensions
{
    public static void Block(this System.CodeDom.Compiler.IndentedTextWriter writer, Action action)
    {
        try
        {
            writer.WriteLine(" {");
            writer.Indent += 1;
            action();
        }
        finally
        {
            writer.Indent -= 1;
            writer.WriteLine("}");
        }
    }
}

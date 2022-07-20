using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

var withPdf = args.Length != 0 && args[0].Equals("-pdf");

var withPdfText = withPdf ? "Yes" : "No (To enable PDF generation, pass the -pdf parameter)";

Console.WriteLine($"Pdf generate: {withPdfText}");

var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

var rootPath = Path.GetFullPath(Path.Combine(location!, "xsl"));

if (Directory.Exists(rootPath))
{
    Console.WriteLine($"Directory xsl found '{rootPath}'");

    var files = Directory.GetFiles(rootPath, "*.xsl", SearchOption.AllDirectories);

    var count = files.Length;

    var width = count.ToString().Length;

    Console.WriteLine($"Found {count} files by mask '*.xsl'");

    for (int i = 0; i < files.Length; i++)
    {
        var file = files[i];

        var relativePath = Path.GetRelativePath(rootPath, file);

        var relativeDir = Path.GetDirectoryName(relativePath);

        var path = String.IsNullOrEmpty(relativeDir) ? Path.GetFileNameWithoutExtension(relativePath) 
                           : Path.Combine(relativeDir, Path.GetFileNameWithoutExtension(relativePath));

        var no = (i + 1).ToString().PadRight(width);

        Console.WriteLine();
        Console.WriteLine($"[{no}] {relativePath}");

        try
        {
            Byte[]? view = null;

            if (Exists(rootPath, path, "html"))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("view html exist");
                Console.ResetColor();
            }
            else
            {
                var xsl = LoadText(rootPath, path, "xsl");

                var xslDoc = LoadXml(xsl);

                var compiled = Compile(xslDoc);

                var xml = LoadText(rootPath, path, "xml");

                LoadXml(xml);

                view = Transform(compiled, xml);

                SaveView(rootPath, path, "html", view);
            }

            if (withPdf)
            {
                if (Exists(rootPath, path, "pdf"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("view pdf exist");
                    Console.ResetColor();
                }
                else
                {
                    var pdf = ToPdf(view ?? LoadBytes(rootPath, path, "html"));

                    SaveView(rootPath, path, "pdf", pdf);
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            try
            {
                Console.Write($"{ex.Message}");

                var inner = ex.InnerException;

                if (inner != null)
                    Console.Write($" -> {inner.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }
        Console.WriteLine();
    }
}
else
{
    Console.WriteLine($"Directory xsl not found '{rootPath}'");
}

Pdf.Dispose();

Console.Write("Press any key to exit...");
Console.ReadKey();

static Byte[] LoadBytes(String rootPath, String path, String ext) 
    => File.ReadAllBytes(Path.Combine(rootPath, path + "." + ext));

static String LoadText(String rootPath, String path, String ext)
{
    Console.Write($"Load {ext}: ");

    path += "." + ext;

    var localPath = Path.Combine(rootPath, path);

    if (!File.Exists(localPath)) throw new InvalidOperationException($"File '{path}' not found");

    var data = File.ReadAllText(localPath);

    Console.Write("Ok");
    Console.WriteLine();

    return data;
}

static XmlDocument LoadXml(String xml, [CallerArgumentExpression("xml")] String? paramName = null)
{
    Console.Write($"Validate {paramName}: ");
    try
    {
        var doc = new XmlDocument();

        doc.PreserveWhitespace = true;

        doc.LoadXml(xml);

        Console.Write("Ok");
        Console.WriteLine();

        return doc;
    }
    catch (XmlException ex)
    {
        throw new InvalidOperationException($"{paramName} is not valid document", ex);
    }
}

static XslCompiledTransform Compile(XmlDocument xsl)
{
    Console.Write("Compile xsl: ");

    var compiled = new XslCompiledTransform();

    try
    {
        compiled.Load(xsl);
        Console.Write("Ok");
        Console.WriteLine();
    }
    catch (XsltException ex)
    {
        throw new InvalidOperationException("xsl compilation error", ex);
    } 

    return compiled;
}

static Byte[] Transform(XslCompiledTransform compiled, String xml)
{
    Console.Write("Transform: ");

    using var stream = new MemoryStream();

    using var xmlReader = new XmlTextReader(new StringReader(xml));

    compiled.Transform(xmlReader, null, stream);

    var transformed = stream.ToArray();

    Console.Write("Ok");
    Console.WriteLine();

    return transformed;
}

static Boolean Exists(String rootPath, String path, String ext) => File.Exists(Path.Combine(rootPath, path + "." + ext));

static void SaveView(String rootPath, String path, String ext, Byte[] view)
{
    Console.Write($"Save view {ext}: ");
    path += "." + ext;
    var localPath = Path.Combine(rootPath, path);
    try
    {
        File.WriteAllBytes(localPath, view);
        Console.Write("Ok");
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException($"error saving file '{path}' to disk", ex);
    }
}

static Byte[] ToPdf(Byte[] view)
{
    Console.WriteLine($"Convert view to pdf: ");

    try
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            return Pdf.Convert(Encoding.UTF8.GetString(view));
        }
        finally
        {
            Console.ResetColor();
            Console.WriteLine("Ok");
        }
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException("error convert to pdf", ex);
    }
}

class Pdf
{
    private static IT.Pdf.DinkToPdf.PdfConverter _converter = new IT.Pdf.DinkToPdf.BasicPdfConverter();

    public static Byte[] Convert(String data) => _converter.Convert(data);

    public static void Dispose() => _converter.Dispose();
}
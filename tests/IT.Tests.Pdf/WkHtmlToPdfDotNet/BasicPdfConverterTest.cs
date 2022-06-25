using IT.Pdf.WkHtmlToPdfDotNet;

namespace IT.Tests.Pdf.WkHtmlToPdfDotNet;

public class BasicPdfConverterTest : PdfConverterTest
{
    public BasicPdfConverterTest() : base(new BasicPdfConverter())
    {
    }
}
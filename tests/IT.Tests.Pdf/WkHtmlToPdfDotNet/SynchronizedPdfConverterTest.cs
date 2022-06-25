using IT.Pdf.WkHtmlToPdfDotNet;

namespace IT.Tests.Pdf.WkHtmlToPdfDotNet;

public class SynchronizedPdfConverterTest : PdfConverterTest
{
    public SynchronizedPdfConverterTest() : base(new SynchronizedPdfConverter())
    {
    }
}
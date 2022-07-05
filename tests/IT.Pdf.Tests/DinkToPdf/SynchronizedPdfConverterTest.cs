using IT.Pdf.DinkToPdf;

namespace IT.Pdf.Tests.DinkToPdf;

public class SynchronizedPdfConverterTest : PdfConverterTest
{
    public SynchronizedPdfConverterTest() : base(new SynchronizedPdfConverter())
    {
    }
}
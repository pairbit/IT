using IT.Pdf.DinkToPdf;

namespace IT.Tests.Pdf.DinkToPdf;

public class SynchronizedPdfConverterTest : PdfConverterTest
{
    public SynchronizedPdfConverterTest() : base(new SynchronizedPdfConverter())
    {
    }
}
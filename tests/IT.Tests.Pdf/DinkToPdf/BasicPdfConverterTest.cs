using IT.Pdf.DinkToPdf;
using WkHtmlToPdfDotNet;

namespace IT.Tests.Pdf.DinkToPdf;

public class BasicPdfConverterTest : PdfConverterTest
{
    public BasicPdfConverterTest()
        : base(new BasicPdfConverter(null, GetGlobalSettings, GetLocalSettings))
    {
    }

    private static GlobalSettings GetGlobalSettings()
    {
        return new GlobalSettings() { Margins = new MarginSettings { } };
    }

    private static LocalSettings GetLocalSettings()
    {
        return new LocalSettings() { WebSettings = new WebSettings { UserStyleSheet = "WkHtmlToPdf.css" } };
    }
}
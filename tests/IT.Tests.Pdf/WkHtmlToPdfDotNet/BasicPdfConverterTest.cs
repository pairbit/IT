using IT.Pdf.WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet;

namespace IT.Tests.Pdf.WkHtmlToPdfDotNet;

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
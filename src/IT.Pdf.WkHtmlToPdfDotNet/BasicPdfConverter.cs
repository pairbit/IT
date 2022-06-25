using Microsoft.Extensions.Logging;
using System;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace IT.Pdf.WkHtmlToPdfDotNet;

public class BasicPdfConverter : PdfConverter
{
    public BasicPdfConverter(
        ITools? tools = null,
        Func<GlobalSettings>? getGlobalSettings = null,
        Func<LocalSettings>? getLocalSettings = null,
        ILogger<PdfConverter>? logger = null) :

        base(new BasicConverter(tools ?? new PdfTools()),
            getGlobalSettings,
            getLocalSettings,
            logger)
    { }
}
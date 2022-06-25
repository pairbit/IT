using Microsoft.Extensions.Logging;
using System;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace IT.Pdf.WkHtmlToPdfDotNet;

public class SynchronizedPdfConverter : PdfConverter
{
    public SynchronizedPdfConverter(
        ITools? tools = null,
        Func<GlobalSettings>? getGlobalSettings = null,
        Func<LocalSettings>? getLocalSettings = null,
        ILogger<PdfConverter>? logger = null) :

        base(new SynchronizedConverter(tools ?? new PdfTools()),
            getGlobalSettings,
            getLocalSettings,
            logger)
    { }
}
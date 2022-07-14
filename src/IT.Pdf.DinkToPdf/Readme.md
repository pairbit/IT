# IT.Pdf.DinkToPdf
[![NuGet version (IT.Pdf.DinkToPdf)](https://img.shields.io/nuget/v/IT.Pdf.DinkToPdf.svg)](https://www.nuget.org/packages/IT.Pdf.DinkToPdf)
[![NuGet pre version (IT.Pdf.DinkToPdf)](https://img.shields.io/nuget/vpre/IT.Pdf.DinkToPdf.svg)](https://www.nuget.org/packages/IT.Pdf.DinkToPdf)

Implementation of conversion html to pdf via DinkToPdf from Haukcode

## add PdfConverter to IServiceCollection

```csharp
    private static WkHtmlToPdfDotNet.GlobalSettings GetGlobalSettings()
    {
        return new WkHtmlToPdfDotNet.GlobalSettings() { Margins = new WkHtmlToPdfDotNet.MarginSettings { } };
    }

    private static LocalSettings GetLocalSettings()
    {
        return new IT.Pdf.DinkToPdf.LocalSettings() { WebSettings = new WkHtmlToPdfDotNet.WebSettings { UserStyleSheet = "WkHtmlToPdf.css" } };
    }

    public static IServiceCollection TryAddPdfConverter(this IServiceCollection services)
    {
        static IT.Pdf.DinkToPdf.PdfConverter? Factory(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<IT.Pdf.DinkToPdf.PdfConverter>>();
            try
            {
                return new IT.Pdf.DinkToPdf.SynchronizedPdfConverter(null, GetGlobalSettings, GetLocalSettings, logger);
            }
            catch (Exception ex)
            {
                if (logger.IsEnabled(LogLevel.Warning))
                    logger.LogWarning(ex, "[Pdf]");

                return null;
            }
        }
        services.AddSingleton<IT.Pdf.IPdfConverter>(Factory);
        return services;
    }
```
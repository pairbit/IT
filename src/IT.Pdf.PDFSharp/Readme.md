# IT.Pdf.PDFSharp
[![NuGet version (IT.Pdf.PDFSharp)](https://img.shields.io/nuget/v/IT.Pdf.PDFSharp.svg)](https://www.nuget.org/packages/IT.Pdf.PDFSharp)
[![NuGet pre version (IT.Pdf.PDFSharp)](https://img.shields.io/nuget/vpre/IT.Pdf.PDFSharp.svg)](https://www.nuget.org/packages/IT.Pdf.PDFSharp)

Implementation of reading pdf via PDFSharp

## add PdfReader to IServiceCollection

```csharp
    public static IServiceCollection TryAddPdfReader(this IServiceCollection services)
    {
        services.AddSingleton<IT.Pdf.IPdfReader, IT.Pdf.PDFSharp.PdfReader>();
        return services;
    }
```
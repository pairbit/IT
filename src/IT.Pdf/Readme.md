# IT.Pdf
[![NuGet version (IT.Pdf)](https://img.shields.io/nuget/v/IT.Pdf.svg)](https://www.nuget.org/packages/IT.Pdf)
[![NuGet pre version (IT.Pdf)](https://img.shields.io/nuget/vpre/IT.Pdf.svg)](https://www.nuget.org/packages/IT.Pdf)

Interfaces of reading/converting pdf

## IPdfConverter

```csharp
    private void Convert(IPdfConverter pdfConverter)
    {
        var html = File.ReadAllText("/var/view.html");

        var bytes = pdfConverter.Convert(html);

        File.WriteAllBytes("/var/view.pdf", bytes);
    }
```

## IPdfReader

```csharp
    public void ReadToStream(IPdfReader pdfReader)
    {
        using var pdf = File.OpenRead("/var/view.pdf");

        var countPages = pdfReader.GetCountPages(pdf);

        for (int i = 0; i < countPages; i++)
        {
            using var page = File.OpenWrite($"/var/view.{i}.pdf");

            _pdfReader.ReadPage(pdf, i, page);

            page.Close();
        }
    }

    public void ReadToBytes(IPdfReader pdfReader)
    {
        using var pdf = File.OpenRead("/var/view.pdf");

        var countPages = pdfReader.GetCountPages(pdf);

        for (int i = 0; i < countPages; i++)
        {
            var page = _pdfReader.ReadPage(pdf, i);

            File.WriteAllBytes($"/var/view.{i}.pdf", page);
        }
    }
```
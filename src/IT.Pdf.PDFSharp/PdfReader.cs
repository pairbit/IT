using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;
using System.Text;

namespace IT.Pdf.PDFSharp;

public class PdfReader : IPdfReader
{
    private readonly ILogger? _logger;

    public PdfReader(ILogger<PdfReader>? logger = null)
    {
        _logger = logger;
    }

    public virtual Int32 GetCountPages(Stream pdf)
    {
        using var doc = PdfSharp.Pdf.IO.PdfReader.Open(pdf, PdfDocumentOpenMode.Import);
        return doc.PageCount;
    }

    public virtual void ReadPage(Stream pdf, Int32 number, Stream page)
    {
        using var doc = PdfSharp.Pdf.IO.PdfReader.Open(pdf, PdfDocumentOpenMode.Import);

        var count = doc.PageCount;

        if (number < 0 || number > count - 1) throw new ArgumentOutOfRangeException(nameof(number), $"{count - 1} >= {number} >= 0");

        var newDoc = new PdfDocument();

        newDoc.Version = doc.Version;

        var info = doc.Info;

        var title = info.Title;

        newDoc.Info.Title = String.IsNullOrWhiteSpace(title) ? $"Page {number + 1} of {count}" : $"Page {number + 1} of {count} from {title}";

        newDoc.Info.Creator = info.Creator;

        newDoc.AddPage(doc.Pages[number]);

        try
        {
            newDoc.Save(page);
        }
        catch (NotSupportedException ex)
        {
            if (_logger is not null && _logger.IsEnabled(LogLevel.Warning))
                _logger.LogWarning(ex, "Try RegisterProvider");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (_logger is not null && _logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)");

            newDoc.Save(page);
        }
    }

    public virtual Byte[] ReadPage(Stream pdf, Int32 number)
    {
        using var page = new MemoryStream();

        ReadPage(pdf, number, page);

        return page.ToArray();
    }
}
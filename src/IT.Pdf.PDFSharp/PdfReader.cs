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

    public virtual Int32 GetCountPages(Stream file)
    {
        using var pdf = PdfSharp.Pdf.IO.PdfReader.Open(file, PdfDocumentOpenMode.Import);
        return pdf.PageCount;
    }

    public virtual Byte[] ReadPage(Stream file, Int32 page)
    {
        using var pdf = PdfSharp.Pdf.IO.PdfReader.Open(file, PdfDocumentOpenMode.Import);

        if (page < 0 || page > pdf.PageCount - 1) throw new ArgumentOutOfRangeException();

        var newDoc = new PdfDocument();

        newDoc.Version = pdf.Version;

        newDoc.Info.Title = $"Page {page + 1} of {pdf.PageCount} from {pdf.Info.Title}";

        newDoc.Info.Creator = pdf.Info.Creator;

        newDoc.AddPage(pdf.Pages[page]);

        using var outStream = new MemoryStream();

        try
        {
            newDoc.Save(outStream);
        }
        catch (NotSupportedException ex)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            newDoc.Save(outStream);
        }

        return outStream.ToArray();
    }
}
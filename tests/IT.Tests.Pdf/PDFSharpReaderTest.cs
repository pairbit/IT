namespace IT.Tests.Pdf;

public class PDFSharpReaderTest : PdfReaderTest
{
    public PDFSharpReaderTest() : base(new IT.Pdf.PDFSharp.PdfReader()) { }
}
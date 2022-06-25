using System;
using System.IO;

namespace IT.Pdf;

public interface IPdfReader
{
    Int32 GetCountPages(Stream pdf);

    Byte[] ReadPage(Stream pdf, Int32 page);
}
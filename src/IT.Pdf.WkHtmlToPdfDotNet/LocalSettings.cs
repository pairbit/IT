using WkHtmlToPdfDotNet;

namespace IT.Pdf.WkHtmlToPdfDotNet;

public class LocalSettings
{
    public string? Page { get; set; }

    public bool? UseExternalLinks { get; set; }

    public bool? UseLocalLinks { get; set; }

    public bool? ProduceForms { get; set; }

    public bool? IncludeInOutline { get; set; }

    public bool? PagesCount { get; set; }

    public WebSettings WebSettings { get; set; } = new();

    public HeaderSettings HeaderSettings { get; set; } = new();

    public FooterSettings FooterSettings { get; set; } = new();

    public LoadSettings LoadSettings { get; set; } = new();
}
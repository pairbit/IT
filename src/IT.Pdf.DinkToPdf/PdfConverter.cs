using Microsoft.Extensions.Logging;
using System;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace IT.Pdf.DinkToPdf;

public class PdfConverter : IPdfConverter, IDisposable
{
    private static readonly GlobalSettings DefaultGlobalSettings = new()
    {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Portrait,
        PaperSize = PaperKind.A4,
        Margins = new MarginSettings() { Top = 10 }
    };

    private readonly IConverter _converter;
    private readonly Func<GlobalSettings>? _getGlobalSettings;
    private readonly Func<LocalSettings>? _getLocalSettings;
    private readonly ILogger? _logger;
    private Boolean _disposed;

    public PdfConverter(
        IConverter converter,
        Func<GlobalSettings>? getGlobalSettings = null,
        Func<LocalSettings>? getLocalSettings = null,
        ILogger<PdfConverter>? logger = null)
    {
        _converter = converter;
        _getGlobalSettings = getGlobalSettings;
        _getLocalSettings = getLocalSettings;
        _logger = logger;
    }

    public virtual Byte[] Convert(String content)
    {
        if (content is null) throw new ArgumentNullException(nameof(content));
        if (content.Length == 0) throw new ArgumentException("is empty", nameof(content));

        //  HACK:   небольшой хак, для библиотеки libwkhtmltox, необходимо указать стиль с height=0, что бы не рендерились пустые страницы
        //WebSettings =
        //{
        //    DefaultEncoding = "utf-8",
        //    UserStyleSheet = "libwkhtmltox.css"
        //},

        var document = new HtmlToPdfDocument()
        {
            GlobalSettings = GetGlobalSettings(),
            Objects = { GetObjectSettings(content) }
        };

        try
        {
            return _converter.Convert(document);
        }
        catch (Exception ex)
        {
            if (_logger is not null && _logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(ex, "Ошибка генерации pdf: {viewHtml}", content);
            }

            throw new InvalidOperationException("Сервер не смог сгенерировать PDF файл. Попробуйте создать HTML представление или обратитесь к администратору.", ex);
        }
    }

    public void Dispose()
    {
        if (_disposed || _converter is null) return;

        _disposed = true;
        _converter.Dispose();
    }

    private GlobalSettings GetGlobalSettings()
    {
        var globalSettings = _getGlobalSettings?.Invoke();

        if (globalSettings is null) return DefaultGlobalSettings;

        if (globalSettings.PaperSize is null) globalSettings.PaperSize = PaperKind.A4;

        return globalSettings;
    }

    private ObjectSettings GetObjectSettings(String content)
    {
        var objectSettings = new ObjectSettings();

        var localSettings = _getLocalSettings?.Invoke();

        if (localSettings is not null)
        {
            objectSettings.LoadSettings = localSettings.LoadSettings;
            objectSettings.FooterSettings = localSettings.FooterSettings;
            objectSettings.WebSettings = localSettings.WebSettings;
            objectSettings.HeaderSettings = localSettings.HeaderSettings;
            objectSettings.IncludeInOutline = localSettings.IncludeInOutline;
            objectSettings.Page = localSettings.Page;
            objectSettings.PagesCount = localSettings.PagesCount;
            objectSettings.ProduceForms = localSettings.ProduceForms;
            objectSettings.UseExternalLinks = localSettings.UseExternalLinks;
            objectSettings.UseLocalLinks = localSettings.UseLocalLinks;
            //objectSettings.Encoding = System.Text.Encoding.GetEncoding("name");
        }

        objectSettings.HtmlContent = content;

        var webSettings = objectSettings.WebSettings;

        if (webSettings is null)
        {
            webSettings = new WebSettings();
            objectSettings.WebSettings = webSettings;
        }

        var defaultEncoding = webSettings.DefaultEncoding;

        if (defaultEncoding is null || defaultEncoding.Length == 0)
            webSettings.DefaultEncoding = "utf-8";

        return objectSettings;
    }
}
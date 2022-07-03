using IT.Text;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.JinnServer.Internal;

internal class JinnServerService
{
    private readonly ITagFinder _tagFinder;
    private readonly String _url;
    private readonly ILogger? _logger;

    public JinnServerService(String url, ITagFinder tagFinder, ILogger? logger)
    {
        _url = url;
        _logger = logger;
        _tagFinder = tagFinder;
    }

    #region Public Methods

    public String GetResponseText(String message, String soapAction, Encoding? encoding = null)
    {
        if (_logger is not null && _logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug("Request: {request}", message);

        if (encoding == null) encoding = Encoding.ASCII;
        var messageBytes = encoding.GetBytes(message);

        var request = CreateWebRequest(soapAction);
        request.ContentLength = messageBytes.Length;

        using var requestStream = request.GetRequestStream();
        requestStream.Write(messageBytes, 0, messageBytes.Length);

        using var response = (HttpWebResponse)TryGetResponse(request);

        if (_logger is not null && _logger.IsEnabled(LogLevel.Information) && response.StatusCode != HttpStatusCode.OK)
            _logger.LogInformation("Response Code: {responseStatusCode} ({responseStatusDescription}), Size: {requestContentLength}",
                response.StatusCode, response.StatusDescription, request.ContentLength);

        using var responseStream = response.GetResponseStream();
        using var responseReader = new StreamReader(responseStream);

        var responseText = responseReader.ReadToEnd();

        if (_logger is not null && _logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug("Response: {response}", responseText);

        return responseText;
    }

    public async Task<String> GetResponseTextAsync(String message, String soapAction, Encoding? encoding = null)
    {
        if (_logger is not null && _logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug("Request: {request}", message);

        if (encoding == null) encoding = Encoding.ASCII;
        var messageBytes = encoding.GetBytes(message);

        var request = CreateWebRequest(soapAction);
        request.ContentLength = messageBytes.Length;

        using var requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false);
        await requestStream.WriteAsync(messageBytes, 0, messageBytes.Length).ConfigureAwait(false);

        using var response = (HttpWebResponse)await TryGetResponseAsync(request).ConfigureAwait(false);

        if (_logger is not null && _logger.IsEnabled(LogLevel.Information) && response.StatusCode != HttpStatusCode.OK)
            _logger.LogInformation("Response Code: {responseStatusCode} ({responseStatusDescription}), Size: {requestContentLength}",
                response.StatusCode, response.StatusDescription, request.ContentLength);

        using var responseStream = response.GetResponseStream();
        using var responseReader = new StreamReader(responseStream);

        var responseText = await responseReader.ReadToEndAsync().ConfigureAwait(false);

        if (_logger is not null && _logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug("Response: {response}", responseText);

        return responseText;
    }

    public Boolean NotFound(ReadOnlySpan<Char> chars, StringComparison comparison)
    {
        var range = _tagFinder.Outer(chars, Soap.Envelope, comparison);

        if (range.Equals(default)) throw new InvalidOperationException("'Envelope' not found");

        chars = chars[range];

        range = _tagFinder.Outer(chars, "Body", comparison);

        if (range.Equals(default)) throw new InvalidOperationException("'Body' not found");

        chars = chars[range];

        range = _tagFinder.Inner(chars, "Fault", comparison);

        if (!range.Equals(default))
        {
            chars = chars[range];

            var faultcode = chars[_tagFinder.Inner(chars, "faultcode", comparison)].Tos();

            var faultstring = chars[_tagFinder.Inner(chars, "faultstring", comparison)].Tos();

            var message = Message.Build(faultcode, faultstring, "[Fault]");

            range = _tagFinder.Inner(chars, "detail", comparison);

            if (!range.Equals(default))
            {
                chars = chars[range];

                var exm = chars[_tagFinder.Inner(chars, "Exception", comparison)].Tos();

                var ex1 = exm is null ? null : new InvalidOperationException(exm);

                var ex2 = ParseServiceFaultInfo(chars, comparison);

                if (ex1 != null && ex2 != null) throw new InvalidOperationException(message, new AggregateException(ex1, ex2));

                if (ex1 != null) throw new InvalidOperationException(message, ex1);

                if (ex2 != null) throw new InvalidOperationException(message, ex2);
            }

            throw new InvalidOperationException(message);
        }

        var ex = ParseServiceFaultInfo(chars, comparison);

        if (ex != null) throw ex;

        return true;
    }

    private Exception? ParseServiceFaultInfo(ReadOnlySpan<Char> chars, StringComparison comparison)
    {
        var range = _tagFinder.Inner(chars, "ServiceFaultInfo", comparison);

        if (range.Equals(default)) return null;

        chars = chars[range];

        var type = chars[_tagFinder.Inner(chars, "type", comparison)].Tos();

        var comment = chars[_tagFinder.Inner(chars, "comment", comparison)].Tos();

        return new InvalidOperationException(Message.Build(type, comment, "[ServiceFaultInfo]"));
    }

    #endregion Public Methods

    #region Private Methods

    private HttpWebRequest CreateWebRequest(String soapAction)
    {
        var request = (HttpWebRequest)WebRequest.Create(_url);
        request.Method = "POST";
        request.ContentType = "text/xml;charset=UTF-8";
        request.Headers.Add("SOAPAction:" + soapAction);
        return request;
    }

    private static WebResponse TryGetResponse(WebRequest request)
    {
        try
        {
            return request.GetResponse();
        }
        catch (WebException ex)
        {
            var response = ex.Response;

            if (response is null) throw;

            return response;
        }
    }

    private static async Task<WebResponse> TryGetResponseAsync(WebRequest request)
    {
        try
        {
            return await request.GetResponseAsync().ConfigureAwait(false);
        }
        catch (WebException ex)
        {
            var response = ex.Response;

            if (response is null) throw;

            return response;
        }
    }

    #endregion Private Methods
}
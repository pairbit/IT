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

    public Boolean NotFound(ReadOnlySpan<Char> response)
    {
        var range = _tagFinder.Outer(response, Soap.Envelope, StringComparison.OrdinalIgnoreCase);

        if (range.Equals(default)) throw new InvalidOperationException("'Envelope' not found");

        response = response[range];

        range = _tagFinder.Outer(response, "Body", StringComparison.OrdinalIgnoreCase);

        if (range.Equals(default)) throw new InvalidOperationException("'Body' not found");

        response = response[range];

        range = _tagFinder.Outer(response, "ServiceFaultInfo", StringComparison.OrdinalIgnoreCase);

        if (!range.Equals(default))
        {
            response = response[range];

            range = _tagFinder.Inner(response, "type", StringComparison.OrdinalIgnoreCase);

            var type = range.Equals(default) ? null : response[range].ToString();

            range = _tagFinder.Inner(response, "comment", StringComparison.OrdinalIgnoreCase);

            var comment = range.Equals(default) ? null : response[range].ToString();

            throw new InvalidOperationException($"[ServiceFaultInfo][{type}] {comment}");
        }

        //range = TagFinder.Outer(response, "Fault", StringComparison.OrdinalIgnoreCase);

        //if (!range.Equals(default))
        //{
        //    response = response[range];

        //    range = TagFinder.Inner(response, "type", StringComparison.OrdinalIgnoreCase);

        //    var type = range.Equals(default) ? null : response[range];

        //    range = TagFinder.Inner(response, "comment", StringComparison.OrdinalIgnoreCase);

        //    var comment = range.Equals(default) ? null : response[range];

        //    throw new InvalidOperationException($"'ServiceFaultInfo' type '{type}' comment '{comment}'");
        //}
        return true;
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
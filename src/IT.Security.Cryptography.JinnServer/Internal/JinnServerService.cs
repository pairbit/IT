using IT.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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

    public Body ParseResponse(String responseText)
    {
        responseText = JsonConvert.SerializeXmlNode(LoadDocument(responseText), Newtonsoft.Json.Formatting.None, true);

        var envelope = JsonConvert.DeserializeObject<Envelope>(responseText);

        var body = envelope?.Body;

        if (body is null) throw new InvalidOperationException($"{nameof(Envelope.Body)} is null");

        if (body.ServiceFaultInfo is not null) throw new InvalidOperationException(body.ServiceFaultInfo.ToString());

        if (body.Fault is not null) throw new InvalidOperationException(body.Fault.ToString());

        //Arg.NotNull(body.Responses.Where(x => x is not null).SingleOrDefault(), "Parse response error!");

        return body;
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

    internal ValidationResponseType ParseResponseValidation(String responseText)
    {
        var span = responseText.AsSpan();

        span = span[_tagFinder.Outer(span, "Envelope", StringComparison.OrdinalIgnoreCase)];

        responseText = span.ToString();

        var body = ParseResponse(responseText);

        if (body.ValidationResponseType is null) throw new InvalidOperationException($"{nameof(Body.ValidationResponseType)} is null");

        var xml = responseText[_tagFinder.Outer(span, "ValidationResponseType", StringComparison.OrdinalIgnoreCase)];

        //DataMember
        //var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(xml));
        //using var reader = XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.Unicode, new XmlDictionaryReaderQuotas(), null);
        //var dataContractSerializer = new DataContractSerializer(typeof(ValidationResponseType));
        //var data = (ValidationResponseType)dataContractSerializer.ReadObject(reader);

        var serializer = new XmlSerializer(typeof(ValidationResponseType));
        using var reader = new StringReader(xml);
        return (ValidationResponseType)serializer.Deserialize(reader);
    }

    private static XmlDocument LoadDocument(String xml)
    {
        try
        {
            xml = RemoveNamespaces(xml);
            var doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(xml);
            return doc;
        }
        catch (XmlException ex)
        {
            throw new InvalidOperationException("Данные не являются XML документом", ex);
        }
    }

    private static String RemoveNamespaces(String xml)
    {
        var xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xml));

        return xmlDocumentWithoutNs.ToString();
    }

    private static XElement RemoveAllNamespaces(XElement xmlDocument)
    {
        if (!xmlDocument.HasElements)
        {
            var xElement = new XElement(xmlDocument.Name.LocalName);
            xElement.Value = xmlDocument.Value;

            foreach (XAttribute attribute in xmlDocument.Attributes().Where(x => !x.IsNamespaceDeclaration))
                xElement.Add(attribute);

            return xElement;
        }
        return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
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
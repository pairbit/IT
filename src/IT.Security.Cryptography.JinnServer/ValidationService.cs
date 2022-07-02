using IT.Text;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.JinnServer;

using Internal;
using Models;

public class ValidationService : ISignEnhancer, ISignVerifier
{
    private static readonly CultureInfo _cultureInfo = CultureInfo.CreateSpecificCulture("ru-RU");
    private static readonly StringComparison _comparison = StringComparison.OrdinalIgnoreCase;

    //private static readonly String[] _signAlgs = new[] { "1.2.643.7.1.1.3.2", "1.2.643.7.1.1.3.3", "1.2.643.2.2.3" };
    private static readonly String[] _formats = new[] {
        "cades-t", "xades-t", "wssec-t",
        "cades-c", "xades-c", "wssec-c",
        "cades-a", "xades-a", "wssec-a"
    };
    private readonly JinnServerService _service;
    private readonly ValidationOptions _options;
    private readonly Func<String, DateTime>? _parseDateTime;
    private readonly ITagFinder _tagFinder;
    private readonly ILogger? _logger;

    public IReadOnlyCollection<String> Formats => _formats;

    public ValidationService(
        Func<ValidationOptions> getOptions,
        Func<String, DateTime>? parseDateTime = null,
        ILogger<ValidationService>? logger = null)
    {
        if (getOptions is null) throw new ArgumentNullException(nameof(getOptions));

        var options = getOptions();
        var tagFinder = new TagFinder();

        _logger = logger;
        _service = new JinnServerService(options.ValidationUrl, tagFinder, logger);
        _options = options;
        _tagFinder = tagFinder;
        _parseDateTime = parseDateTime;
    }

    #region ISignVerifier

    public Boolean IsVerified(String signature, String? detachedData)
        => IsVerified(_service.GetResponseText(GetRequestValidation(signature, detachedData), Soap.Actions.Validate));

    public Signatures Verify(String signature, String? detachedData)
        => GetSignatures(_service.GetResponseText(GetRequestValidation(signature, detachedData), Soap.Actions.Validate));

    public async Task<Boolean> IsVerifiedAsync(String signature, String? detachedData, CancellationToken cancellationToken)
        => IsVerified(await _service.GetResponseTextAsync(GetRequestValidation(signature, detachedData), Soap.Actions.Validate).ConfigureAwait(false));

    public async Task<Signatures> VerifyAsync(String signature, String? detachedData, CancellationToken cancellationToken)
        => GetSignatures(await _service.GetResponseTextAsync(GetRequestValidation(signature, detachedData), Soap.Actions.Validate).ConfigureAwait(false));

    private Boolean IsVerified(String response)
    {
        var span = response.AsSpan();

        span = ParseValidationResponseType(span);

        var range = _tagFinder.Inner(span, "globalStatus", _comparison);

        if (range.Equals(default)) throw new InvalidOperationException("'ValidationResponseType.globalStatus' not found");

        return span[range].Equals("valid", _comparison);
    }

    internal Signatures GetSignatures(String response)
    {
        var chars = response.AsSpan();

        chars = ParseValidationResponseType(chars);

        var signatures = ParseSignatureInfos(chars[_tagFinder.Outer(chars, "SignatureInfos", _comparison)]);

        signatures.Status = ParseGlobalStatus(chars[_tagFinder.Inner(chars, "globalStatus", _comparison)]);

        signatures.DateTime = ParseDateTime(chars[_tagFinder.Inner(chars, "gmtDateTime", _comparison)]);

        return signatures;
    }

    #endregion ISignVerifier

    #region ISignEnhancer

    public String Enhance(String signature, String format, String? detachedData)
        => GetEnhanced(_service.GetResponseText(GetRequestEnhance(signature, format, detachedData), Soap.Actions.Validate));

    public async Task<String> EnhanceAsync(String signature, String format, String? detachedData, CancellationToken cancellationToken)
        => GetEnhanced(await _service.GetResponseTextAsync(GetRequestEnhance(signature, format, detachedData), Soap.Actions.Validate).ConfigureAwait(false));

    private String GetEnhanced(String response)
    {
        var span = response.AsSpan();

        span = ParseValidationResponseType(span);

        var range = _tagFinder.Inner(span, "advanced", _comparison);

        if (range.Equals(default))
        {
            /*
             <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/">
	<soapenv:Body>
		<tccs:ValidationResponseType xmlns:cst="http://www.roskazna.ru/eb/sign/types/cryptoserver" xmlns:tccs="http://www.roskazna.ru/eb/sign/types/sgv">
			<tccs:gmtDateTime>1.7.2022 18:32:22 UTC</tccs:gmtDateTime>
			<tccs:globalStatus>invalid</tccs:globalStatus>
			<tccs:SignatureInfos>
				<cst:SignatureInfo>
					<cst:reference>
						<cst:xmlID/>
					</cst:reference>
					<cst:status>invalid</cst:status>
					<cst:failInfo>
						<cst:type>invalidDigestValue</cst:type>
						<cst:comment>неудавшееся преобразование [ref-Yr88jj1lob1CFKFG] :</cst:comment>
					</cst:failInfo>
					<cst:validationDate>1.7.2022 18:32:22 UTC</cst:validationDate>
				</cst:SignatureInfo>
			</tccs:SignatureInfos>
		</tccs:ValidationResponseType>
	</soapenv:Body>
</soapenv:Envelope>

             */
            throw new InvalidOperationException("'ValidationResponseType.advanced' not found");
        }

        return span[range].ToString();
    }

    #endregion ISignEnhancer

    #region Private Methods

    private ReadOnlySpan<Char> ParseValidationResponseType(ReadOnlySpan<Char> response)
    {
        var range = _tagFinder.Outer(response, "ValidationResponseType", _comparison);

        if (range.Equals(default) && _service.NotFound(response)) throw new InvalidOperationException("'ValidationResponseType' not found");

        return response[range];
    }

    private String GetRequestValidation(String sign, String? detachedData)
    {
        return detachedData is null || detachedData.Length == 0
            ? Soap.Request.Validation(sign.TryToBase64())
            : Soap.Request.Validation(sign.TryToBase64(), detachedData.TryToBase64());
    }

    private String GetRequestEnhance(String sign, String format, String? detachedData)
    {
        return detachedData is null || detachedData.Length == 0
            ? Soap.Request.Enhance(sign.TryToBase64(), format)
            : Soap.Request.Enhance(sign.TryToBase64(), format, detachedData.TryToBase64());
    }

    private void CheckStatus(Internal.GlobalStatus status, SimpleSignatureInfos signatureInfos)
    {
        var infos = signatureInfos?.SignatureInfo;

        Exception? inner = null;

        if (infos is null || infos.Length == 0)
        {
            if (status == Internal.GlobalStatus.valid) return;
        }
        else
        {
            var inners = new List<Exception>();

            foreach (var info in infos!)
            {
                if (info.FailInfo is null) continue;

                var innerException = new InvalidOperationException(info.ToString());

                innerException.Data.Add(nameof(info.Status), info.Status);

                inners.Add(innerException);
            }

            if (inners.Count > 0)
                inner = inners.Count > 1 ? new AggregateException(inners) : inners.FirstOrDefault();
        }

        if (inner == null && status == Internal.GlobalStatus.valid) return;

        var exception = new InvalidOperationException(Res.NoEnhanced + " " + status.Localize(), inner);

        exception.Data.Add(nameof(Internal.GlobalStatus), status);

        throw exception;
    }

    private String GetEnhanced(Body body)
    {
        var data = body.ValidationResponseType;

        if (data is null) throw new InvalidOperationException($"{nameof(Body.ValidationResponseType)} is null");

        CheckStatus(data.GlobalStatus, data.SignatureInfos);

        var advanced = data.Advanced;

        if (advanced is null || advanced.Length == 0) throw new InvalidOperationException("Internal Error JinnServer");

        return advanced.ToBase64();
    }

    #endregion Private Methods

    #region Parse

    private Signatures ParseSignatureInfos(ReadOnlySpan<Char> chars)
    {
        var list = new Signatures();

        do
        {
            var range = _tagFinder.LastOuter(chars, "SignatureInfo", _comparison);

            if (range.Equals(default)) break;

            list.Insert(0, ParseSignatureInfo(chars[range]));

            chars = chars[..range.Start];
        } while (true);

        return list;
    }

    private Signature ParseSignatureInfo(ReadOnlySpan<Char> chars)
    {
        var info = new Signature();

        info.FirstTimeStamp = ParseDateTime(chars[_tagFinder.LastInner(chars, "firstTimeStamp", _comparison)]);

        info.ValidationDate = ParseDateTime(chars[_tagFinder.LastInner(chars, "validationDate", _comparison)]);

        info.Certificate = ParseCertificate(chars[_tagFinder.Outer(chars, "signerCertInfo", _comparison)]);

        info.Status = ParseSignatureStatus(chars[_tagFinder.Inner(chars, "status", _comparison)]);

        return info;
    }

    private Models.Certificate ParseCertificate(ReadOnlySpan<Char> chars)
    {
        var cert = new Models.Certificate();

        cert.Signature = Tos(chars[_tagFinder.LastInner(chars, "Signature", _comparison)]);

        var temp = chars[_tagFinder.LastInner(chars, "AlgorithmIdentifier", _comparison)];

        cert.AlgorithmIdentifier = Tos(temp[_tagFinder.Inner(temp, "AlgId", _comparison)]);

        chars = chars[_tagFinder.Inner(chars, "TBSCertificate", _comparison)];

        cert.Extensions = ParseExtensions(chars[_tagFinder.Inner(chars, "Extensions", _comparison)]);

        cert.Subject = ParseSubject(chars[_tagFinder.Inner(chars, "Subject", _comparison)]);

        temp = chars[_tagFinder.Inner(chars, "SubjectPublicKeyInfo", _comparison)];

        cert.SubjectPublicKey = Tos(temp[_tagFinder.Inner(temp, "SubjectPublicKey", _comparison)]);

        cert.SubjectPublicKeyAlgorithm = Tos(temp[_tagFinder.Inner(temp, "PublicKeyAlgorithm", _comparison)]);

        temp = chars[_tagFinder.Inner(chars, "Validity", _comparison)];
        cert.ValidityFrom = ParseUTCTime(temp[_tagFinder.Inner(temp, "NotBefore", _comparison)]);
        cert.ValidityTo = ParseUTCTime(temp[_tagFinder.Inner(temp, "NotAfter", _comparison)]);

        cert.Issuer = ParseIssuer(chars[_tagFinder.Inner(chars, "Issuer", _comparison)]);

        temp = chars[_tagFinder.Inner(chars, "Signature", _comparison)];
        cert.SignatureAlg = Tos(temp[_tagFinder.Inner(temp, "AlgId", _comparison)]);

        cert.SerialNumber = Tos(chars[_tagFinder.Inner(chars, "CertificateSerialNumber", _comparison)]);

        cert.Version = Tos(chars[_tagFinder.Inner(chars, "Version", _comparison)]);

        return cert;
    }

    private List<CertificateExtension>? ParseExtensions(ReadOnlySpan<Char> chars)
    {
        var list = new List<CertificateExtension>();

        do
        {
            var range = _tagFinder.LastOuter(chars, "Extension", _comparison);

            if (range.Equals(default)) break;

            list.Insert(0, ParseExtension(chars[range]));

            chars = chars[..range.Start];
        } while (true);

        return list;
    }

    private CertificateExtension ParseExtension(ReadOnlySpan<Char> chars)
    {
        var ext = new CertificateExtension();

        ext.Oid = Tos(chars[_tagFinder.Inner(chars, "ExtensionType", _comparison)]);

        ext.IsCritical = ParseBool(chars[_tagFinder.Inner(chars, "Critical", _comparison)]);

        chars = chars[_tagFinder.Inner(chars, "extValue", _comparison)];

        ext.Value = Tos(chars);

        return ext;
    }

    private CertificateSubject ParseSubject(ReadOnlySpan<Char> chars)
    {
        var subject = new CertificateSubject();

        subject.INN = ParseINN(chars);

        subject.OGRN = ParseOGRN(chars);

        subject.Street = ParseStreet(chars);

        subject.Email = ParseEmail(chars);

        subject.Country = ParseCountry(chars);

        subject.Region = ParseRegion(chars);

        subject.City = ParseCity(chars);

        subject.Organization = ParseOrganization(chars);

        subject.OrganizationalUnit = ParseOrganizationalUnit(chars);

        subject.UnstructuredName = ParseUnstructuredName(chars);

        subject.CommonName = ParseCommonName(chars);

        return subject;
    }

    private DateTime? ParseUTCTime(ReadOnlySpan<Char> chars) => ParseDateTime(chars[_tagFinder.Inner(chars, "UTCTime", _comparison)]);

    private DateTime? ParseDateTime(ReadOnlySpan<Char> chars)
    {
        if (chars.Length == 0) return null;

        var parseDateTime = _parseDateTime;

        if (parseDateTime is not null) return parseDateTime(chars.ToString());

        var index = chars.IndexOf("UTC", _comparison);

        if (index > -1) chars = chars[..index].Trim();

#if NETSTANDARD2_0
        var dateTime = DateTime.Parse(chars.ToString(), _cultureInfo);
#else
        var dateTime = DateTime.Parse(chars, _cultureInfo);
#endif

        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

        return dateTime;
    }

    private CertificateIssuer ParseIssuer(ReadOnlySpan<Char> chars)
    {
        var issuer = new CertificateIssuer();

        issuer.Email = ParseEmail(chars);

        issuer.Region = ParseRegion(chars);

        issuer.INN = ParseINN(chars);

        issuer.OGRN = ParseOGRN(chars);

        issuer.Street = ParseStreet(chars);

        issuer.City = ParseCity(chars);

        issuer.Country = ParseCountry(chars);

        issuer.Organization = ParseOrganization(chars);

        issuer.CommonName = ParseCommonName(chars);

        //????
        issuer.OrganizationalUnit = ParseOrganizationalUnit(chars);

        issuer.UnstructuredName = ParseUnstructuredName(chars);

        return issuer;
    }

    private String? ParseEmail(ReadOnlySpan<Char> chars) => Tos(chars[_tagFinder.Inner(chars, "EmailAddress", _comparison)]);

    private String? ParseINN(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "INN", _comparison)];
        return chars.Length == 0 ? null : Tos(chars[_tagFinder.Inner(chars, "numeric", _comparison)]);
    }

    private String? ParseOGRN(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "OGRN", _comparison)];
        return chars.Length == 0 ? null : Tos(chars[_tagFinder.Inner(chars, "numeric", _comparison)]);
    }

    private String? ParseStreet(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "StreetAddress", _comparison)];
        return chars.Length == 0 ? null : Tos(chars[_tagFinder.Inner(chars, "UTF8String", _comparison)]);
    }

    private String? ParseCountry(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "CountryName", _comparison)];
        return chars.Length == 0 ? null : Tos(chars[_tagFinder.Inner(chars, "iso-3166-code", _comparison)]);
    }

    private String? ParseRegion(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "StateOrProvinceName", _comparison)];
        return chars.Length == 0 ? null : Tos(chars[_tagFinder.Inner(chars, "UTF8String", _comparison)]);
    }

    private String? ParseCity(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "LocalityName", _comparison)];
        return chars.Length == 0 ? null : Tos(chars[_tagFinder.Inner(chars, "UTF8String", _comparison)]);
    }

    private String? ParseOrganization(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "OrganizationName", _comparison)];
        return chars.Length == 0 ? null : Tos(chars[_tagFinder.Inner(chars, "UTF8String", _comparison)]);
    }

    private String? ParseOrganizationalUnit(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "OrganizationalUnitName", _comparison)];
        return chars.Length == 0 ? null : Tos(chars[_tagFinder.Inner(chars, "UTF8String", _comparison)]);
    }

    private String? ParseUnstructuredName(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "unstructuredName", _comparison)];
        return chars.Length == 0 ? null : Tos(chars[_tagFinder.Inner(chars, "UTF8String", _comparison)]);
    }

    private String? ParseCommonName(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "CommonName", _comparison)];
        return chars.Length == 0 ? null : Tos(chars[_tagFinder.Inner(chars, "UTF8String", _comparison)]);
    }

    private static String? Tos(ReadOnlySpan<Char> chars) => chars.Length == 0 ? null : chars.ToString();

    private static Models.SignatureStatus? ParseSignatureStatus(ReadOnlySpan<Char> chars)
    {
        if (chars.Length == 0) return null;
        if (chars.Equals("unknown", _comparison)) return Models.SignatureStatus.Unknown;
        if (chars.Equals("valid", _comparison)) return Models.SignatureStatus.Valid;
        if (chars.Equals("invalid", _comparison)) return Models.SignatureStatus.Invalid;

        throw new FormatException("status not correct");
    }

    private static SignaturesStatus? ParseGlobalStatus(ReadOnlySpan<Char> chars)
    {
        if (chars.Length == 0) return null;
        if (chars.Equals("unknown", _comparison)) return SignaturesStatus.Unknown;
        if (chars.Equals("valid", _comparison)) return SignaturesStatus.Valid;
        if (chars.Equals("partiallyValid", _comparison)) return SignaturesStatus.PartiallyValid;
        if (chars.Equals("invalid", _comparison)) return SignaturesStatus.Invalid;

        throw new FormatException("globalStatus not correct");
    }

    private static Boolean? ParseBool(ReadOnlySpan<Char> chars)
    {
        if (chars.Length == 0) return null;
        if (chars.Equals("{TRUE}", _comparison)) return true;
        if (chars.Equals("{FALSE}", _comparison)) return false;

        throw new FormatException($"Format bool type '{chars.ToString()}' not correct");
    }

    #endregion
}
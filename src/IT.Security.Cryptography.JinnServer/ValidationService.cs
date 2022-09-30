using IT.Text;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.JinnServer;

using Internal;
using Models;

public class ValidationService : ISignatureEnhancer, ISignatureVerifier
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
        _tagFinder = tagFinder;
        _parseDateTime = parseDateTime;
    }

    #region ISignVerifier

    public Boolean Verify(String signature, String? detachedData)
        => IsVerified(_service.GetResponseText(GetRequestValidation(signature, detachedData), Soap.Actions.Validate));

    public async Task<Boolean> VerifyAsync(String signature, String? detachedData, CancellationToken cancellationToken)
        => IsVerified(await _service.GetResponseTextAsync(GetRequestValidation(signature, detachedData), Soap.Actions.Validate).ConfigureAwait(false));

    public Signatures VerifyDetail(String signature, String? detachedData)
        => GetSignatures(_service.GetResponseText(GetRequestValidation(signature, detachedData), Soap.Actions.Validate));

    public async Task<Signatures> VerifyDetailAsync(String signature, String? detachedData, CancellationToken cancellationToken)
        => GetSignatures(await _service.GetResponseTextAsync(GetRequestValidation(signature, detachedData), Soap.Actions.Validate).ConfigureAwait(false));

    #endregion ISignVerifier

    #region ISignEnhancer

    public String Enhance(String signature, String format, String? detachedData)
        => GetEnhanced(_service.GetResponseText(GetRequestEnhance(signature, format, detachedData), Soap.Actions.Validate));

    public async Task<String> EnhanceAsync(String signature, String format, String? detachedData, CancellationToken cancellationToken)
        => GetEnhanced(await _service.GetResponseTextAsync(GetRequestEnhance(signature, format, detachedData), Soap.Actions.Validate).ConfigureAwait(false));

    public EnhancedSignatures EnhanceDetail(String signature, String format, String? detachedData)
        => GetEnhancedSignatures(_service.GetResponseText(GetRequestEnhance(signature, format, detachedData), Soap.Actions.Validate));

    public async Task<EnhancedSignatures> EnhanceDetailAsync(String signature, String format, String? detachedData, CancellationToken cancellationToken)
        => GetEnhancedSignatures(await _service.GetResponseTextAsync(GetRequestEnhance(signature, format, detachedData), Soap.Actions.Validate).ConfigureAwait(false));

    #endregion ISignEnhancer

    #region Private Methods

    private EnhancedSignatures GetEnhancedSignatures(String response)
    {
        var chars = response.AsSpan();

        chars = ParseValidationResponseType(chars);

        var signatures = ParseSignatureInfos(chars);

        signatures.Status = ParseGlobalStatus(chars);

        signatures.DateTime = ParseGmtDateTime(chars);

        return new EnhancedSignatures
        {
            Signatures = signatures,
            Enhanced = chars[_tagFinder.Inner(chars, "advanced", _comparison)].Tos()
        };
    }

    private String GetEnhanced(String response)
    {
        var span = response.AsSpan();

        span = ParseValidationResponseType(span);

        var range = _tagFinder.Inner(span, "advanced", _comparison);

        if (range.Equals(default)) throw ParseExceptions(span) ?? new InvalidOperationException("'ValidationResponseType.advanced' not found");

        return span[range].ToString();
    }

    private Exception? ParseExceptions(ReadOnlySpan<Char> chars)
    {
        var list = new List<Exception>();

        do
        {
            var range = _tagFinder.LastOuter(chars, "SignatureInfo", _comparison);

            if (range.Equals(default)) break;

            var exception = ParseException(chars[range]);

            if (exception is not null) list.Insert(0, exception);

            chars = chars[..range.Start];
        } while (true);

        if (list.Count == 0) return null;

        if (list.Count == 1) return list[0];

        return new AggregateException(list);
    }

    private Exception? ParseException(ReadOnlySpan<Char> chars)
    {
        (var type, var comment) = ParseFailInfo(chars);

        if (type is null && comment is null) return null;

        return new InvalidOperationException(Message.Build(type?.ToString(), comment, "[failInfo]"));
    }

    private (SignatureFaultType?, String?) ParseFailInfo(ReadOnlySpan<Char> chars)
    {
        var range = _tagFinder.Inner(chars, "failInfo", _comparison);

        if (range.Equals(default)) return (null, null);

        chars = chars[range];

        return (ParseSignatureFaultType(chars[_tagFinder.Inner(chars, "type", _comparison)]),
                chars[_tagFinder.Inner(chars, "comment", _comparison)].Tos());
    }

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

        var signatures = ParseSignatureInfos(chars);

        signatures.Status = ParseGlobalStatus(chars);

        signatures.DateTime = ParseGmtDateTime(chars);

        return signatures;
    }

    private ReadOnlySpan<Char> ParseValidationResponseType(ReadOnlySpan<Char> response)
    {
        var range = _tagFinder.Outer(response, "ValidationResponseType", _comparison);

        if (range.Equals(default) && _service.NotFound(response, _comparison)) throw new InvalidOperationException("'ValidationResponseType' not found");

        return response[range];
    }

    private Signatures ParseSignatureInfos(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Outer(chars, "SignatureInfos", _comparison)];

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

        (info.FaultType, info.FaultComment) = ParseFailInfo(chars);

        info.Status = ParseSignatureStatus(chars[_tagFinder.Inner(chars, "status", _comparison)]);

        info.Reference = chars[_tagFinder.Inner(chars, "reference", _comparison)].Tos();

        return info;
    }

    private Certificate ParseCertificate(ReadOnlySpan<Char> chars)
    {
        var cert = new Certificate();

        cert.Signature = chars[_tagFinder.LastInner(chars, "Signature", _comparison)].Tos();

        var temp = chars[_tagFinder.LastInner(chars, "AlgorithmIdentifier", _comparison)];

        cert.AlgorithmIdentifier = temp[_tagFinder.Inner(temp, "AlgId", _comparison)].Tos();

        chars = chars[_tagFinder.Inner(chars, "TBSCertificate", _comparison)];

        cert.Extensions = ParseExtensions(chars[_tagFinder.Inner(chars, "Extensions", _comparison)]);

        cert.Subject = ParseSubject(chars[_tagFinder.Inner(chars, "Subject", _comparison)]);

        temp = chars[_tagFinder.Inner(chars, "SubjectPublicKeyInfo", _comparison)];

        cert.SubjectPublicKey = temp[_tagFinder.Inner(temp, "SubjectPublicKey", _comparison)].Tos();

        cert.SubjectPublicKeyAlgorithm = temp[_tagFinder.Inner(temp, "PublicKeyAlgorithm", _comparison)].Tos();

        temp = chars[_tagFinder.Inner(chars, "Validity", _comparison)];
        cert.ValidityFrom = ParseUTCTime(temp[_tagFinder.Inner(temp, "NotBefore", _comparison)]);
        cert.ValidityTo = ParseUTCTime(temp[_tagFinder.Inner(temp, "NotAfter", _comparison)]);

        cert.Issuer = ParseIssuer(chars[_tagFinder.Inner(chars, "Issuer", _comparison)]);

        temp = chars[_tagFinder.Inner(chars, "Signature", _comparison)];
        cert.SignatureAlg = temp[_tagFinder.Inner(temp, "AlgId", _comparison)].Tos();

        cert.SerialNumber = chars[_tagFinder.Inner(chars, "CertificateSerialNumber", _comparison)].Tos();

        cert.Version = chars[_tagFinder.Inner(chars, "Version", _comparison)].Tos();

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

        ext.Oid = chars[_tagFinder.Inner(chars, "ExtensionType", _comparison)].Tos();

        ext.IsCritical = ParseBool(chars[_tagFinder.Inner(chars, "Critical", _comparison)]);

        chars = chars[_tagFinder.Inner(chars, "extValue", _comparison)];

        ext.Value = chars.Tos();

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

    private DateTime? ParseGmtDateTime(ReadOnlySpan<Char> chars) => ParseDateTime(chars[_tagFinder.Inner(chars, "gmtDateTime", _comparison)]);

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

    private String? ParseEmail(ReadOnlySpan<Char> chars) => chars[_tagFinder.Inner(chars, "EmailAddress", _comparison)].Tos();

    private String? ParseINN(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "INN", _comparison)];
        return chars.Length == 0 ? null : chars[_tagFinder.Inner(chars, "numeric", _comparison)].Tos();
    }

    private String? ParseOGRN(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "OGRN", _comparison)];
        return chars.Length == 0 ? null : chars[_tagFinder.Inner(chars, "numeric", _comparison)].Tos();
    }

    private String? ParseStreet(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "StreetAddress", _comparison)];
        return chars.Length == 0 ? null : chars[_tagFinder.Inner(chars, "UTF8String", _comparison)].Tos();
    }

    private String? ParseCountry(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "CountryName", _comparison)];
        return chars.Length == 0 ? null : chars[_tagFinder.Inner(chars, "iso-3166-code", _comparison)].Tos();
    }

    private String? ParseRegion(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "StateOrProvinceName", _comparison)];
        return chars.Length == 0 ? null : chars[_tagFinder.Inner(chars, "UTF8String", _comparison)].Tos();
    }

    private String? ParseCity(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "LocalityName", _comparison)];
        return chars.Length == 0 ? null : chars[_tagFinder.Inner(chars, "UTF8String", _comparison)].Tos();
    }

    private String? ParseOrganization(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "OrganizationName", _comparison)];
        return chars.Length == 0 ? null : chars[_tagFinder.Inner(chars, "UTF8String", _comparison)].Tos();
    }

    private String? ParseOrganizationalUnit(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "OrganizationalUnitName", _comparison)];
        return chars.Length == 0 ? null : chars[_tagFinder.Inner(chars, "UTF8String", _comparison)].Tos();
    }

    private String? ParseUnstructuredName(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "unstructuredName", _comparison)];
        return chars.Length == 0 ? null : chars[_tagFinder.Inner(chars, "UTF8String", _comparison)].Tos();
    }

    private String? ParseCommonName(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "CommonName", _comparison)];
        return chars.Length == 0 ? null : chars[_tagFinder.Inner(chars, "UTF8String", _comparison)].Tos();
    }

    private SignaturesStatus? ParseGlobalStatus(ReadOnlySpan<Char> chars)
    {
        chars = chars[_tagFinder.Inner(chars, "globalStatus", _comparison)];

        if (chars.Length == 0) return null;
        if (chars.Equals("unknown", _comparison)) return SignaturesStatus.Unknown;
        if (chars.Equals("valid", _comparison)) return SignaturesStatus.Valid;
        if (chars.Equals("partiallyValid", _comparison)) return SignaturesStatus.PartiallyValid;
        if (chars.Equals("invalid", _comparison)) return SignaturesStatus.Invalid;

        throw new FormatException($"globalStatus '{chars.ToString()}' not correct");
    }

    private static SignatureStatus? ParseSignatureStatus(ReadOnlySpan<Char> chars)
    {
        if (chars.Length == 0) return null;
        if (chars.Equals("unknown", _comparison)) return SignatureStatus.Unknown;
        if (chars.Equals("valid", _comparison)) return SignatureStatus.Valid;
        if (chars.Equals("invalid", _comparison)) return SignatureStatus.Invalid;

        throw new FormatException($"status '{chars.ToString()}' not correct");
    }

    private static SignatureFaultType? ParseSignatureFaultType(ReadOnlySpan<Char> chars)
    {
        if (chars.Length == 0) return null;
        if (chars.Equals("unknownDigestAlgorithm", _comparison)) return SignatureFaultType.UnknownDigestAlgorithm;
        if (chars.Equals("unknownSignatureAlgorithm", _comparison)) return SignatureFaultType.UnknownSignatureAlgorithm;
        if (chars.Equals("signerCertificateNotFound", _comparison)) return SignatureFaultType.SignerCertificateNotFound;
        if (chars.Equals("signerCertificateIssuerNotFound", _comparison)) return SignatureFaultType.SignerCertificateIssuerNotFound;
        if (chars.Equals("signerCertificateSignatureInvalid", _comparison)) return SignatureFaultType.SignerCertificateSignatureInvalid;
        if (chars.Equals("signerCertificateCRLNotFound", _comparison)) return SignatureFaultType.SignerCertificateCRLNotFound;
        if (chars.Equals("signerCertificateExpired", _comparison)) return SignatureFaultType.SignerCertificateExpired;
        if (chars.Equals("signerCertificateRevoked", _comparison)) return SignatureFaultType.SignerCertificateRevoked;
        if (chars.Equals("invalidDigestValue", _comparison)) return SignatureFaultType.InvalidDigestValue;
        if (chars.Equals("invalidSignatureValue", _comparison)) return SignatureFaultType.InvalidSignatureValue;
        if (chars.Equals("invalidSignatureTimeStamp", _comparison)) return SignatureFaultType.InvalidSignatureTimeStamp;

        throw new FormatException($"SignatureFaultType '{chars.ToString()}' not correct");
    }

    private static Boolean? ParseBool(ReadOnlySpan<Char> chars)
    {
        if (chars.Length == 0) return null;
        if (chars.Equals("{TRUE}", _comparison)) return true;
        if (chars.Equals("{FALSE}", _comparison)) return false;

        throw new FormatException($"Format bool type '{chars.ToString()}' not correct");
    }

    private static String GetRequestValidation(String sign, String? detachedData)
    {
        return detachedData is null || detachedData.Length == 0
            ? Soap.Request.Validation(sign.TryToBase64())
            : Soap.Request.Validation(sign.TryToBase64(), detachedData.TryToBase64());
    }

    private static String GetRequestEnhance(String sign, String format, String? detachedData)
    {
        return detachedData is null || detachedData.Length == 0
            ? Soap.Request.Enhance(sign.TryToBase64(), format)
            : Soap.Request.Enhance(sign.TryToBase64(), format, detachedData.TryToBase64());
    }

    #endregion Private Methods
}
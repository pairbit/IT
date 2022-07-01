using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace IT.Security.Cryptography.JinnServer.Internal;

internal class JinnServerService
{
    private readonly String _url;
    private readonly ILogger? _logger;

    public JinnServerService(String url, ILogger? logger)
    {
        _url = url;
        _logger = logger;
    }

    #region Public Methods

    public ValidationResponseType GetResponseValidation(String message, String soapAction, Encoding? encoding = null)
    {
        var responseText = GetResponseText(message, soapAction, encoding);
        return ParseResponseValidation(responseText);
    }

    public async Task<ValidationResponseType> GetResponseValidationAsync(String message, String soapAction, Encoding? encoding = null)
    {
        var responseText = await GetResponseTextAsync(message, soapAction, encoding).ConfigureAwait(false);
        return ParseResponseValidation(responseText);
    }

    public Body GetResponse(String message, String soapAction, Encoding? encoding = null)
    {
        var responseText = GetResponseText(message, soapAction, encoding);
        return ParseResponse(responseText);
    }

    public async Task<Body> GetResponseAsync(String message, String soapAction, Encoding? encoding = null)
    {
        var responseText = await GetResponseTextAsync(message, soapAction, encoding).ConfigureAwait(false);
        //responseText = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Body><tccs:ValidationResponseType xmlns:cst=\"http://www.roskazna.ru/eb/sign/types/cryptoserver\" xmlns:tccs=\"http://www.roskazna.ru/eb/sign/types/sgv\"><tccs:gmtDateTime>4.8.2021 7:21:12 UTC</tccs:gmtDateTime><tccs:globalStatus>invalid</tccs:globalStatus><tccs:SignatureInfos><cst:SignatureInfo><cst:reference><cst:issuerAndSerial><cst:IssuerAndSerial><cst:Issuer><cst:DistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.840.113549.1.9.1</cst:AttributeType><cst:EmailAddress>test@uc.test</cst:EmailAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.3.131.1.1</cst:AttributeType><cst:INN><cst:numeric>002311111111</cst:numeric></cst:INN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.100.1</cst:AttributeType><cst:OGRN><cst:numeric>0011111111111</cst:numeric></cst:OGRN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.9</cst:AttributeType><cst:StreetAddress><cst:UTF8String>улица Моросейка, дом 12</cst:UTF8String></cst:StreetAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.6</cst:AttributeType><cst:CountryName><cst:iso-3166-code>RU</cst:iso-3166-code></cst:CountryName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.8</cst:AttributeType><cst:StateOrProvinceName><cst:UTF8String>г. Москва</cst:UTF8String></cst:StateOrProvinceName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.7</cst:AttributeType><cst:LocalityName><cst:UTF8String>Москва</cst:UTF8String></cst:LocalityName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.10</cst:AttributeType><cst:OrganizationName><cst:UTF8String>Тестовый промежуточный УЦ Юнисерт4</cst:UTF8String></cst:OrganizationName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.3</cst:AttributeType><cst:CommonName><cst:UTF8String>Тестовый промежуточный УЦ Юнисерт4 Для Разработчиков</cst:UTF8String></cst:CommonName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName></cst:DistinguishedName></cst:Issuer><cst:SerialNumber>19885</cst:SerialNumber></cst:IssuerAndSerial></cst:issuerAndSerial></cst:reference><cst:status>invalid</cst:status><cst:failInfo><cst:type>signerCertificateSignatureInvalid</cst:type><cst:comment>сертификат автора подписи не соответствует значению подписанного атрибута SigningCertificateV2</cst:comment></cst:failInfo><cst:signerCertInfo><cst:Certificate><cst:TBSCertificate><cst:Version>2</cst:Version><cst:CertificateSerialNumber>19885</cst:CertificateSerialNumber><cst:Signature><cst:AlgId>1.2.643.7.1.1.3.2</cst:AlgId></cst:Signature><cst:Issuer><cst:DistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.840.113549.1.9.1</cst:AttributeType><cst:EmailAddress>test@uc.test</cst:EmailAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.3.131.1.1</cst:AttributeType><cst:INN><cst:numeric>002311111111</cst:numeric></cst:INN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.100.1</cst:AttributeType><cst:OGRN><cst:numeric>0011111111111</cst:numeric></cst:OGRN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.9</cst:AttributeType><cst:StreetAddress><cst:UTF8String>улица Моросейка, дом 12</cst:UTF8String></cst:StreetAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.6</cst:AttributeType><cst:CountryName><cst:iso-3166-code>RU</cst:iso-3166-code></cst:CountryName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.8</cst:AttributeType><cst:StateOrProvinceName><cst:UTF8String>г. Москва</cst:UTF8String></cst:StateOrProvinceName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.7</cst:AttributeType><cst:LocalityName><cst:UTF8String>Москва</cst:UTF8String></cst:LocalityName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.10</cst:AttributeType><cst:OrganizationName><cst:UTF8String>Тестовый промежуточный УЦ Юнисерт4</cst:UTF8String></cst:OrganizationName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.3</cst:AttributeType><cst:CommonName><cst:UTF8String>Тестовый промежуточный УЦ Юнисерт4 Для Разработчиков</cst:UTF8String></cst:CommonName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName></cst:DistinguishedName></cst:Issuer><cst:Validity><cst:NotBefore><cst:UTCTime>15.7.2021 10:10:0 UTC</cst:UTCTime></cst:NotBefore><cst:NotAfter><cst:UTCTime>15.7.2022 10:10:0 UTC</cst:UTCTime></cst:NotAfter></cst:Validity><cst:Subject><cst:DistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.3</cst:AttributeType><cst:CommonName><cst:UTF8String>Новый Пользователь Тестирович</cst:UTF8String></cst:CommonName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.4</cst:AttributeType><cst:Surname><cst:UTF8String>Пользователь</cst:UTF8String></cst:Surname></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.42</cst:AttributeType><cst:GivenName><cst:UTF8String>Новый</cst:UTF8String></cst:GivenName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue>#E0C49612301406052A85036403160B3138323933373438323634</cst:AttributeTypeAndValue></cst:RelativeDistinguishedName></cst:DistinguishedName></cst:Subject><cst:SubjectPublicKeyInfo><cst:PublicKeyAlgorithm><cst:AlgId>1.2.643.7.1.1.1.1</cst:AlgId><cst:gostR34102012_256><cst:gostR34102012PublicKeyParameters><cst:OBJECT_IDENTIFIER>1.2.643.2.2.36.0</cst:OBJECT_IDENTIFIER><cst:OBJECT_IDENTIFIER>1.2.643.7.1.1.2.2</cst:OBJECT_IDENTIFIER></cst:gostR34102012PublicKeyParameters></cst:gostR34102012_256></cst:PublicKeyAlgorithm><cst:SubjectPublicKey>04401D3D8235F8AC48D2CF7DE95666FEE308268F5AED8BB539A592037A92C8924416AFB69FAEDC4DFC023570766A37BBD8B6D8A0FAC9AECE2B270979AC0B46E12EAC</cst:SubjectPublicKey></cst:SubjectPublicKeyInfo><cst:Extensions><cst:Extension><cst:ExtensionType>2.5.29.15</cst:ExtensionType><cst:Critical>{TRUE}</cst:Critical><cst:extValue><cst:KeyUsage>11111</cst:KeyUsage></cst:extValue></cst:Extension><cst:Extension><cst:ExtensionType>2.5.29.37</cst:ExtensionType><cst:extValue><cst:ExtKeyUsage><cst:ClientAuth>1.3.6.1.5.5.7.3.2</cst:ClientAuth></cst:ExtKeyUsage></cst:extValue></cst:Extension><cst:Extension><cst:ExtensionType>2.5.29.31</cst:ExtensionType><cst:extValue><cst:CRLDistributionPoints><cst:DistributionPoint><cst:DistributionPointName><cst:FullName><cst:GeneralName><cst:URI>http://cdp.unicert</cst:URI></cst:GeneralName></cst:FullName></cst:DistributionPointName></cst:DistributionPoint></cst:CRLDistributionPoints></cst:extValue></cst:Extension><cst:Extension><cst:ExtensionType>2.5.29.14</cst:ExtensionType><cst:extValue><cst:SubjectKeyIdentifier>97179717EA7499D6C67922A51660EB7F84431CEC</cst:SubjectKeyIdentifier></cst:extValue></cst:Extension><cst:Extension><cst:ExtensionType>2.5.29.35</cst:ExtensionType><cst:extValue><cst:AuthorityKeyIdentifier><cst:KeyIdentifier>C0909A2C7408835926838C39914EA0AF9C7D658D</cst:KeyIdentifier><cst:AuthorityCertIssuer><cst:GeneralName><cst:DirectoryName><cst:DistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.840.113549.1.9.1</cst:AttributeType><cst:EmailAddress>admin-ca@stand.local</cst:EmailAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.100.1</cst:AttributeType><cst:OGRN><cst:numeric>1234567890123</cst:numeric></cst:OGRN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.3.131.1.1</cst:AttributeType><cst:INN><cst:numeric>001234567890</cst:numeric></cst:INN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.6</cst:AttributeType><cst:CountryName><cst:iso-3166-code>RU</cst:iso-3166-code></cst:CountryName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.8</cst:AttributeType><cst:StateOrProvinceName><cst:UTF8String>г.Москва</cst:UTF8String></cst:StateOrProvinceName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.7</cst:AttributeType><cst:LocalityName><cst:UTF8String>Москва</cst:UTF8String></cst:LocalityName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.9</cst:AttributeType><cst:StreetAddress><cst:UTF8String>Театральная аллея, д.3, стр. 1</cst:UTF8String></cst:StreetAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.11</cst:AttributeType><cst:OrganizationalUnitName><cst:UTF8String>Сервисный центр</cst:UTF8String></cst:OrganizationalUnitName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.10</cst:AttributeType><cst:OrganizationName><cst:UTF8String>Информзащита</cst:UTF8String></cst:OrganizationName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.3</cst:AttributeType><cst:CommonName><cst:UTF8String>Корневой тестовый УЦ Информзащита гост 2012</cst:UTF8String></cst:CommonName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName></cst:DistinguishedName></cst:DirectoryName></cst:GeneralName></cst:AuthorityCertIssuer><cst:AuthorityCertSerial>234560131158599659122815935534490191309</cst:AuthorityCertSerial></cst:AuthorityKeyIdentifier></cst:extValue></cst:Extension></cst:Extensions></cst:TBSCertificate><cst:AlgorithmIdentifier><cst:AlgId>1.2.643.7.1.1.3.2</cst:AlgId></cst:AlgorithmIdentifier><cst:Signature>FA61D05D66792D389D860C5355C040459458D0326A30D242BD5ACBFEF933B8D5422DCFF3094E92D1DDA70E8F8C6C947063C2BB0A4D7A39EBFDBD80EC82F95E4E</cst:Signature></cst:Certificate></cst:signerCertInfo><cst:validationDate>4.8.2021 7:21:12 UTC</cst:validationDate></cst:SignatureInfo></tccs:SignatureInfos></tccs:ValidationResponseType></soapenv:Body></soapenv:Envelope>";
        //responseText = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Body><tccs:ValidationResponseType xmlns:cst=\"http://www.roskazna.ru/eb/sign/types/cryptoserver\" xmlns:tccs=\"http://www.roskazna.ru/eb/sign/types/sgv\"><tccs:gmtDateTime>4.8.2021 7:21:12 UTC</tccs:gmtDateTime><tccs:globalStatus>invalid</tccs:globalStatus><tccs:SignatureInfos><cst:SignatureInfo><cst:reference><cst:issuerAndSerial><cst:IssuerAndSerial><cst:Issuer><cst:DistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.840.113549.1.9.1</cst:AttributeType><cst:EmailAddress>test@uc.test</cst:EmailAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.3.131.1.1</cst:AttributeType><cst:INN><cst:numeric>002311111111</cst:numeric></cst:INN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.100.1</cst:AttributeType><cst:OGRN><cst:numeric>0011111111111</cst:numeric></cst:OGRN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.9</cst:AttributeType><cst:StreetAddress><cst:UTF8String>улица Моросейка, дом 12</cst:UTF8String></cst:StreetAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.6</cst:AttributeType><cst:CountryName><cst:iso-3166-code>RU</cst:iso-3166-code></cst:CountryName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.8</cst:AttributeType><cst:StateOrProvinceName><cst:UTF8String>г. Москва</cst:UTF8String></cst:StateOrProvinceName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.7</cst:AttributeType><cst:LocalityName><cst:UTF8String>Москва</cst:UTF8String></cst:LocalityName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.10</cst:AttributeType><cst:OrganizationName><cst:UTF8String>Тестовый промежуточный УЦ Юнисерт4</cst:UTF8String></cst:OrganizationName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.3</cst:AttributeType><cst:CommonName><cst:UTF8String>Тестовый промежуточный УЦ Юнисерт4 Для Разработчиков</cst:UTF8String></cst:CommonName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName></cst:DistinguishedName></cst:Issuer><cst:SerialNumber>19885</cst:SerialNumber></cst:IssuerAndSerial></cst:issuerAndSerial></cst:reference><cst:status>invalid</cst:status><cst:failInfo><cst:type>signerCertificateSignatureInvalid</cst:type><cst:comment>сертификат автора подписи не соответствует значению подписанного атрибута SigningCertificateV2</cst:comment></cst:failInfo><cst:signerCertInfo><cst:Certificate><cst:TBSCertificate><cst:Version>2</cst:Version><cst:CertificateSerialNumber>19885</cst:CertificateSerialNumber><cst:Signature><cst:AlgId>1.2.643.7.1.1.3.2</cst:AlgId></cst:Signature><cst:Issuer><cst:DistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.840.113549.1.9.1</cst:AttributeType><cst:EmailAddress>test@uc.test</cst:EmailAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.3.131.1.1</cst:AttributeType><cst:INN><cst:numeric>002311111111</cst:numeric></cst:INN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.100.1</cst:AttributeType><cst:OGRN><cst:numeric>0011111111111</cst:numeric></cst:OGRN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.9</cst:AttributeType><cst:StreetAddress><cst:UTF8String>улица Моросейка, дом 12</cst:UTF8String></cst:StreetAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.6</cst:AttributeType><cst:CountryName><cst:iso-3166-code>RU</cst:iso-3166-code></cst:CountryName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.8</cst:AttributeType><cst:StateOrProvinceName><cst:UTF8String>г. Москва</cst:UTF8String></cst:StateOrProvinceName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.7</cst:AttributeType><cst:LocalityName><cst:UTF8String>Москва</cst:UTF8String></cst:LocalityName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.10</cst:AttributeType><cst:OrganizationName><cst:UTF8String>Тестовый промежуточный УЦ Юнисерт4</cst:UTF8String></cst:OrganizationName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.3</cst:AttributeType><cst:CommonName><cst:UTF8String>Тестовый промежуточный УЦ Юнисерт4 Для Разработчиков</cst:UTF8String></cst:CommonName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName></cst:DistinguishedName></cst:Issuer><cst:Validity><cst:NotBefore><cst:UTCTime>15.7.2021 10:10:0 UTC</cst:UTCTime></cst:NotBefore><cst:NotAfter><cst:UTCTime>15.7.2022 10:10:0 UTC</cst:UTCTime></cst:NotAfter></cst:Validity><cst:Subject><cst:DistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.3</cst:AttributeType><cst:CommonName><cst:UTF8String>Новый Пользователь Тестирович</cst:UTF8String></cst:CommonName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.4</cst:AttributeType><cst:Surname><cst:UTF8String>Пользователь</cst:UTF8String></cst:Surname></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.42</cst:AttributeType><cst:GivenName><cst:UTF8String>Новый</cst:UTF8String></cst:GivenName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue>#E0C49612301406052A85036403160B3138323933373438323634</cst:AttributeTypeAndValue></cst:RelativeDistinguishedName></cst:DistinguishedName></cst:Subject><cst:SubjectPublicKeyInfo><cst:PublicKeyAlgorithm><cst:AlgId>1.2.643.7.1.1.1.1</cst:AlgId><cst:gostR34102012_256><cst:gostR34102012PublicKeyParameters><cst:OBJECT_IDENTIFIER>1.2.643.2.2.36.0</cst:OBJECT_IDENTIFIER><cst:OBJECT_IDENTIFIER>1.2.643.7.1.1.2.2</cst:OBJECT_IDENTIFIER></cst:gostR34102012PublicKeyParameters></cst:gostR34102012_256></cst:PublicKeyAlgorithm><cst:SubjectPublicKey>04401D3D8235F8AC48D2CF7DE95666FEE308268F5AED8BB539A592037A92C8924416AFB69FAEDC4DFC023570766A37BBD8B6D8A0FAC9AECE2B270979AC0B46E12EAC</cst:SubjectPublicKey></cst:SubjectPublicKeyInfo><cst:Extensions><cst:Extension><cst:ExtensionType>2.5.29.15</cst:ExtensionType><cst:Critical>{TRUE}</cst:Critical><cst:extValue><cst:KeyUsage>11111</cst:KeyUsage></cst:extValue></cst:Extension><cst:Extension><cst:ExtensionType>2.5.29.37</cst:ExtensionType><cst:extValue><cst:ExtKeyUsage><cst:ClientAuth>1.3.6.1.5.5.7.3.2</cst:ClientAuth></cst:ExtKeyUsage></cst:extValue></cst:Extension><cst:Extension><cst:ExtensionType>2.5.29.31</cst:ExtensionType><cst:extValue><cst:CRLDistributionPoints><cst:DistributionPoint><cst:DistributionPointName><cst:FullName><cst:GeneralName><cst:URI>http://cdp.unicert</cst:URI></cst:GeneralName></cst:FullName></cst:DistributionPointName></cst:DistributionPoint></cst:CRLDistributionPoints></cst:extValue></cst:Extension><cst:Extension><cst:ExtensionType>2.5.29.14</cst:ExtensionType><cst:extValue><cst:SubjectKeyIdentifier>97179717EA7499D6C67922A51660EB7F84431CEC</cst:SubjectKeyIdentifier></cst:extValue></cst:Extension><cst:Extension><cst:ExtensionType>2.5.29.35</cst:ExtensionType><cst:extValue><cst:AuthorityKeyIdentifier><cst:KeyIdentifier>C0909A2C7408835926838C39914EA0AF9C7D658D</cst:KeyIdentifier><cst:AuthorityCertIssuer><cst:GeneralName><cst:DirectoryName><cst:DistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.840.113549.1.9.1</cst:AttributeType><cst:EmailAddress>admin-ca@stand.local</cst:EmailAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.100.1</cst:AttributeType><cst:OGRN><cst:numeric>1234567890123</cst:numeric></cst:OGRN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.3.131.1.1</cst:AttributeType><cst:INN><cst:numeric>001234567890</cst:numeric></cst:INN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.6</cst:AttributeType><cst:CountryName><cst:iso-3166-code>RU</cst:iso-3166-code></cst:CountryName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.8</cst:AttributeType><cst:StateOrProvinceName><cst:UTF8String>г.Москва</cst:UTF8String></cst:StateOrProvinceName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.7</cst:AttributeType><cst:LocalityName><cst:UTF8String>Москва</cst:UTF8String></cst:LocalityName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.9</cst:AttributeType><cst:StreetAddress><cst:UTF8String>Театральная аллея, д.3, стр. 1</cst:UTF8String></cst:StreetAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.11</cst:AttributeType><cst:OrganizationalUnitName><cst:UTF8String>Сервисный центр</cst:UTF8String></cst:OrganizationalUnitName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.10</cst:AttributeType><cst:OrganizationName><cst:UTF8String>Информзащита</cst:UTF8String></cst:OrganizationName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.3</cst:AttributeType><cst:CommonName><cst:UTF8String>Корневой тестовый УЦ Информзащита гост 2012</cst:UTF8String></cst:CommonName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName></cst:DistinguishedName></cst:DirectoryName></cst:GeneralName></cst:AuthorityCertIssuer><cst:AuthorityCertSerial>234560131158599659122815935534490191309</cst:AuthorityCertSerial></cst:AuthorityKeyIdentifier></cst:extValue></cst:Extension></cst:Extensions></cst:TBSCertificate><cst:AlgorithmIdentifier><cst:AlgId>1.2.643.7.1.1.3.2</cst:AlgId></cst:AlgorithmIdentifier><cst:Signature>FA61D05D66792D389D860C5355C040459458D0326A30D242BD5ACBFEF933B8D5422DCFF3094E92D1DDA70E8F8C6C947063C2BB0A4D7A39EBFDBD80EC82F95E4E</cst:Signature></cst:Certificate></cst:signerCertInfo><cst:validationDate>4.8.2021 7:21:12 UTC</cst:validationDate></cst:SignatureInfo><cst:SignatureInfo><cst:reference><cst:issuerAndSerial><cst:IssuerAndSerial><cst:Issuer><cst:DistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.840.113549.1.9.1</cst:AttributeType><cst:EmailAddress>test@uc.test</cst:EmailAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.3.131.1.1</cst:AttributeType><cst:INN><cst:numeric>002311111111</cst:numeric></cst:INN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.100.1</cst:AttributeType><cst:OGRN><cst:numeric>0011111111111</cst:numeric></cst:OGRN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.9</cst:AttributeType><cst:StreetAddress><cst:UTF8String>улица Моросейка, дом 12</cst:UTF8String></cst:StreetAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.6</cst:AttributeType><cst:CountryName><cst:iso-3166-code>RU</cst:iso-3166-code></cst:CountryName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.8</cst:AttributeType><cst:StateOrProvinceName><cst:UTF8String>г. Москва</cst:UTF8String></cst:StateOrProvinceName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.7</cst:AttributeType><cst:LocalityName><cst:UTF8String>Москва</cst:UTF8String></cst:LocalityName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.10</cst:AttributeType><cst:OrganizationName><cst:UTF8String>Тестовый промежуточный УЦ Юнисерт4</cst:UTF8String></cst:OrganizationName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.3</cst:AttributeType><cst:CommonName><cst:UTF8String>Тестовый промежуточный УЦ Юнисерт4 Для Разработчиков</cst:UTF8String></cst:CommonName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName></cst:DistinguishedName></cst:Issuer><cst:SerialNumber>19885</cst:SerialNumber></cst:IssuerAndSerial></cst:issuerAndSerial></cst:reference><cst:status>invalid</cst:status><cst:failInfo><cst:type>signerCertificateSignatureInvalid</cst:type><cst:comment>Какая-то вторая ошибка</cst:comment></cst:failInfo><cst:signerCertInfo><cst:Certificate><cst:TBSCertificate><cst:Version>2</cst:Version><cst:CertificateSerialNumber>19885</cst:CertificateSerialNumber><cst:Signature><cst:AlgId>1.2.643.7.1.1.3.2</cst:AlgId></cst:Signature><cst:Issuer><cst:DistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.840.113549.1.9.1</cst:AttributeType><cst:EmailAddress>test@uc.test</cst:EmailAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.3.131.1.1</cst:AttributeType><cst:INN><cst:numeric>002311111111</cst:numeric></cst:INN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.100.1</cst:AttributeType><cst:OGRN><cst:numeric>0011111111111</cst:numeric></cst:OGRN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.9</cst:AttributeType><cst:StreetAddress><cst:UTF8String>улица Моросейка, дом 12</cst:UTF8String></cst:StreetAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.6</cst:AttributeType><cst:CountryName><cst:iso-3166-code>RU</cst:iso-3166-code></cst:CountryName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.8</cst:AttributeType><cst:StateOrProvinceName><cst:UTF8String>г. Москва</cst:UTF8String></cst:StateOrProvinceName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.7</cst:AttributeType><cst:LocalityName><cst:UTF8String>Москва</cst:UTF8String></cst:LocalityName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.10</cst:AttributeType><cst:OrganizationName><cst:UTF8String>Тестовый промежуточный УЦ Юнисерт4</cst:UTF8String></cst:OrganizationName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.3</cst:AttributeType><cst:CommonName><cst:UTF8String>Тестовый промежуточный УЦ Юнисерт4 Для Разработчиков</cst:UTF8String></cst:CommonName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName></cst:DistinguishedName></cst:Issuer><cst:Validity><cst:NotBefore><cst:UTCTime>15.7.2021 10:10:0 UTC</cst:UTCTime></cst:NotBefore><cst:NotAfter><cst:UTCTime>15.7.2022 10:10:0 UTC</cst:UTCTime></cst:NotAfter></cst:Validity><cst:Subject><cst:DistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.3</cst:AttributeType><cst:CommonName><cst:UTF8String>Новый Пользователь Тестирович</cst:UTF8String></cst:CommonName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.4</cst:AttributeType><cst:Surname><cst:UTF8String>Пользователь</cst:UTF8String></cst:Surname></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.42</cst:AttributeType><cst:GivenName><cst:UTF8String>Новый</cst:UTF8String></cst:GivenName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue>#E0C49612301406052A85036403160B3138323933373438323634</cst:AttributeTypeAndValue></cst:RelativeDistinguishedName></cst:DistinguishedName></cst:Subject><cst:SubjectPublicKeyInfo><cst:PublicKeyAlgorithm><cst:AlgId>1.2.643.7.1.1.1.1</cst:AlgId><cst:gostR34102012_256><cst:gostR34102012PublicKeyParameters><cst:OBJECT_IDENTIFIER>1.2.643.2.2.36.0</cst:OBJECT_IDENTIFIER><cst:OBJECT_IDENTIFIER>1.2.643.7.1.1.2.2</cst:OBJECT_IDENTIFIER></cst:gostR34102012PublicKeyParameters></cst:gostR34102012_256></cst:PublicKeyAlgorithm><cst:SubjectPublicKey>04401D3D8235F8AC48D2CF7DE95666FEE308268F5AED8BB539A592037A92C8924416AFB69FAEDC4DFC023570766A37BBD8B6D8A0FAC9AECE2B270979AC0B46E12EAC</cst:SubjectPublicKey></cst:SubjectPublicKeyInfo><cst:Extensions><cst:Extension><cst:ExtensionType>2.5.29.15</cst:ExtensionType><cst:Critical>{TRUE}</cst:Critical><cst:extValue><cst:KeyUsage>11111</cst:KeyUsage></cst:extValue></cst:Extension><cst:Extension><cst:ExtensionType>2.5.29.37</cst:ExtensionType><cst:extValue><cst:ExtKeyUsage><cst:ClientAuth>1.3.6.1.5.5.7.3.2</cst:ClientAuth></cst:ExtKeyUsage></cst:extValue></cst:Extension><cst:Extension><cst:ExtensionType>2.5.29.31</cst:ExtensionType><cst:extValue><cst:CRLDistributionPoints><cst:DistributionPoint><cst:DistributionPointName><cst:FullName><cst:GeneralName><cst:URI>http://cdp.unicert</cst:URI></cst:GeneralName></cst:FullName></cst:DistributionPointName></cst:DistributionPoint></cst:CRLDistributionPoints></cst:extValue></cst:Extension><cst:Extension><cst:ExtensionType>2.5.29.14</cst:ExtensionType><cst:extValue><cst:SubjectKeyIdentifier>97179717EA7499D6C67922A51660EB7F84431CEC</cst:SubjectKeyIdentifier></cst:extValue></cst:Extension><cst:Extension><cst:ExtensionType>2.5.29.35</cst:ExtensionType><cst:extValue><cst:AuthorityKeyIdentifier><cst:KeyIdentifier>C0909A2C7408835926838C39914EA0AF9C7D658D</cst:KeyIdentifier><cst:AuthorityCertIssuer><cst:GeneralName><cst:DirectoryName><cst:DistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.840.113549.1.9.1</cst:AttributeType><cst:EmailAddress>admin-ca@stand.local</cst:EmailAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.100.1</cst:AttributeType><cst:OGRN><cst:numeric>1234567890123</cst:numeric></cst:OGRN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>1.2.643.3.131.1.1</cst:AttributeType><cst:INN><cst:numeric>001234567890</cst:numeric></cst:INN></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.6</cst:AttributeType><cst:CountryName><cst:iso-3166-code>RU</cst:iso-3166-code></cst:CountryName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.8</cst:AttributeType><cst:StateOrProvinceName><cst:UTF8String>г.Москва</cst:UTF8String></cst:StateOrProvinceName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.7</cst:AttributeType><cst:LocalityName><cst:UTF8String>Москва</cst:UTF8String></cst:LocalityName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.9</cst:AttributeType><cst:StreetAddress><cst:UTF8String>Театральная аллея, д.3, стр. 1</cst:UTF8String></cst:StreetAddress></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.11</cst:AttributeType><cst:OrganizationalUnitName><cst:UTF8String>Сервисный центр</cst:UTF8String></cst:OrganizationalUnitName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.10</cst:AttributeType><cst:OrganizationName><cst:UTF8String>Информзащита</cst:UTF8String></cst:OrganizationName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName><cst:RelativeDistinguishedName><cst:AttributeTypeAndValue><cst:AttributeType>2.5.4.3</cst:AttributeType><cst:CommonName><cst:UTF8String>Корневой тестовый УЦ Информзащита гост 2012</cst:UTF8String></cst:CommonName></cst:AttributeTypeAndValue></cst:RelativeDistinguishedName></cst:DistinguishedName></cst:DirectoryName></cst:GeneralName></cst:AuthorityCertIssuer><cst:AuthorityCertSerial>234560131158599659122815935534490191309</cst:AuthorityCertSerial></cst:AuthorityKeyIdentifier></cst:extValue></cst:Extension></cst:Extensions></cst:TBSCertificate><cst:AlgorithmIdentifier><cst:AlgId>1.2.643.7.1.1.3.2</cst:AlgId></cst:AlgorithmIdentifier><cst:Signature>FA61D05D66792D389D860C5355C040459458D0326A30D242BD5ACBFEF933B8D5422DCFF3094E92D1DDA70E8F8C6C947063C2BB0A4D7A39EBFDBD80EC82F95E4E</cst:Signature></cst:Certificate></cst:signerCertInfo><cst:validationDate>4.8.2021 7:21:12 UTC</cst:validationDate></cst:SignatureInfo></tccs:SignatureInfos></tccs:ValidationResponseType></soapenv:Body></soapenv:Envelope>";
        return ParseResponse(responseText);
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

    private String GetResponseText(String message, String soapAction, Encoding? encoding = null)
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

        if (_logger is not null && response.StatusCode != HttpStatusCode.OK)
            _logger.LogInformation("Response Code: {responseStatusCode} ({responseStatusDescription}), Size: {requestContentLength}",
                response.StatusCode, response.StatusDescription, request.ContentLength);

        using var responseStream = response.GetResponseStream();
        using var responseReader = new StreamReader(responseStream);

        return responseReader.ReadToEnd();
    }

    private async Task<String> GetResponseTextAsync(String message, String soapAction, Encoding? encoding = null)
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

        if (_logger is not null && response.StatusCode != HttpStatusCode.OK)
            _logger.LogInformation("Response Code: {responseStatusCode} ({responseStatusDescription}), Size: {requestContentLength}",
                response.StatusCode, response.StatusDescription, request.ContentLength);

        using var responseStream = response.GetResponseStream();
        using var responseReader = new StreamReader(responseStream);

        return await responseReader.ReadToEndAsync().ConfigureAwait(false);
    }

    private Body ParseResponse(String responseText)
    {
        if (_logger is not null && _logger.IsEnabled(LogLevel.Debug)) 
            _logger.LogDebug("Response: {response}", responseText);

        var range = TagFinder.Outer(responseText.AsSpan(), "Envelope".AsSpan(), "soapenv".AsSpan(), StringComparison.OrdinalIgnoreCase);

        responseText = responseText[range];

        responseText = JsonConvert.SerializeXmlNode(LoadDocument(responseText), Newtonsoft.Json.Formatting.None, true);

        var envelope = JsonConvert.DeserializeObject<Envelope>(responseText);
        
        var body = envelope?.Body;
        
        if (body is null) throw new InvalidOperationException($"{nameof(Envelope.Body)} is null");
        
        if (body.ServiceFaultInfo is not null) throw new InvalidOperationException(body.ServiceFaultInfo.ToString());
        
        if (body.Fault is not null) throw new InvalidOperationException(body.Fault.ToString());
        
        //Arg.NotNull(body.Responses.Where(x => x is not null).SingleOrDefault(), "Parse response error!");
        
        return body;
    }

    private ValidationResponseType ParseResponseValidation(String responseText)
    {
        var body = ParseResponse(responseText);

        if (body.ValidationResponseType is null) throw new InvalidOperationException($"{nameof(Body.ValidationResponseType)} is null");

        var matches = Regex.Match(responseText, Soap.Regex.Response);
        var xml = matches.Groups[2].Value;

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
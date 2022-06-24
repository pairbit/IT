using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Xml;
using System.Xml.Serialization;

namespace IT.Security.Cryptography.JinnServer.Internal;

public enum GlobalStatus
{
    unknown,
    invalid,
    partiallyValid,
    valid,
}

[XmlRoot(Namespace = "http://www.roskazna.ru/eb/sign/types/sgv")]
public class ValidationResponseType
{
    [XmlElement(Namespace = "http://www.roskazna.ru/eb/sign/types/sgv", ElementName = "advanced")]
    public byte[] Advanced { get; set; }

    [XmlElement(Namespace = "http://www.roskazna.ru/eb/sign/types/sgv", ElementName = "globalStatus")]
    public GlobalStatus GlobalStatus { get; set; }

    public SignatureInfos SignatureInfos { get; set; }

    public ServiceFaultInfo ServiceFaultInfo { get; set; }
}

[XmlRoot(Namespace = "http://www.roskazna.ru/eb/sign/types/sgv")]
public class SignatureInfos
{
    [XmlElement(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver", ElementName = "SignatureInfo")]
    public SignatureInfo[] SignatureInfo { get; set; }
}

public class SignatureInfo
{
    [XmlElement(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver", ElementName = "reference")]
    public SignatureRef Reference { get; set; }

    [XmlElement(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver", ElementName = "status")]
    public SignatureStatus Status { get; set; }

    [XmlElement(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver", ElementName = "failInfo")]
    public FailInfo FailInfo { get; set; }

    [XmlElement(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver", ElementName = "signerCertInfo")]
    public SignerCertInfo SignerCertInfo { get; set; }

    [XmlElement(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver", ElementName = "validationDate")]
    public string ValidationDate { get; set; }
}

public class FailInfo
{
    [XmlElement(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver", ElementName = "type")]
    public ValidationFaultType Type { get; set; }

    [XmlElement(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver", ElementName = "comment")]
    public string Comment { get; set; }
}

public class SignerCertInfo
{
    public Certificate Certificate { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string GivenName { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(GivenName)); } }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string Surname { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(Surname)); } }

    /// <summary>
    /// Общее имя
    /// </summary>
    public string CommonName { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(CommonName)); } }

    //TODO опечатка?
    /// <summary>
    /// Неструктурированное имя
    /// </summary>
    public string UnstructuredName { get { return GetIssuerAnyPropertyFromSignerCertSubject("unstructuredName"); } }

    /// <summary>
    /// Email
    /// </summary>
    public string EmailAddress { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(EmailAddress)); } }

    /// <summary>
    /// Регион
    /// </summary>
    public string StateOrProvinceName { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(StateOrProvinceName)); } }

    /// <summary>
    /// ИНН
    /// </summary>
    public string INN { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(INN)); } }

    /// <summary>
    /// ОГРН
    /// </summary>
    public string OGRN { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(OGRN)); } }

    /// <summary>
    /// ОГРНИП
    /// </summary>
    public string OGRNIP { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(OGRNIP)); } }

    /// <summary>
    /// СНИЛС
    /// </summary>
    public string SNILS { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(SNILS)); } }

    /// <summary>
    /// Адрес улицы
    /// </summary>
    public string StreetAddress { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(StreetAddress)); } }

    /// <summary>
    /// Город
    /// </summary>
    public string LocalityName { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(LocalityName)); } }

    /// <summary>
    /// Страна
    /// </summary>
    public string CountryName { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(CountryName)); } }

    /// <summary>
    /// Организация
    /// </summary>
    public string OrganizationName { get { return GetIssuerAnyPropertyFromSignerCertSubject(nameof(OrganizationName)); } }

    /// <summary>
    /// ИД ключа субъекта. Используется для уникальной идентификации сертификата в БД ВРС.
    /// </summary>
    public string SubjectKeyIdentifier { get { return GetIssuerAnyPropertyFromSignerCertExtensions(nameof(SubjectKeyIdentifier))?.extValue?.InnerText; } }

    /// <summary>
    /// Использование ключа. Битовая маска.
    /// </summary>
    public string KeyUsage { get { return GetIssuerAnyPropertyFromSignerCertExtensions(nameof(KeyUsage))?.extValue?.InnerText; } }

    /// <summary>
    /// Информация о корневом сертификате.
    /// </summary>
    public string AuthorityCertIssuer { get { return GetAttributeFromExtension(nameof(AuthorityCertIssuer)); } }

    /// <summary>
    /// Серийный номер корневого сертификата в DEC
    /// </summary>
    public string AuthorityCertSerial { get { return GetAttributeFromExtension(nameof(AuthorityCertSerial)); } }

    private string GetAttributeFromExtension(string attribute)
    {
        var authorityKeyIdentifierChildNodes = GetIssuerAnyPropertyFromSignerCertExtensions("AuthorityKeyIdentifier")?.extValue?.ChildNodes;

        foreach (XmlNode child in authorityKeyIdentifierChildNodes)
        {
            if (child.LocalName == attribute)
                return child.InnerText;
        }
        return null;
    }

    private Extension GetIssuerAnyPropertyFromSignerCertExtensions(string propertyName)
    {
        return Certificate.TBSCertificate.Extensions.Where(x => x.extValue.LocalName == propertyName).FirstOrDefault();
    }

    private string GetIssuerAnyPropertyFromSignerCertSubject(string propertyName)
    {
        return Certificate.TBSCertificate.Subject.GetValue(propertyName);
    }
}

public enum ValidationFaultType
{
    unknownDigestAlgorithm,
    unknownSignatureAlgorithm,
    signerCertificateNotFound,
    signerCertificateIssuerNotFound,
    signerCertificateSignatureInvalid,
    signerCertificateCRLNotFound,
    signerCertificateExpired,
    signerCertificateRevoked,
    invalidDigestValue,
    invalidSignatureValue,
    invalidSignatureTimeStamp,
}

public enum SignatureStatus
{
    //[Display(Name = "Недостаточно информации для определения статуса ни одной из имеющихся подписей")]
    unknown,

    //[Display(Name = "Подлинность не подтверждена")]
    invalid,

    //[Display(Name = "Подлинность подтверждена")]
    valid,
}

public class Certificate
{
    public TBSCertificate TBSCertificate { get; set; }

    public SignatureAlg AlgorithmIdentifier { get; set; }

    public String Signature { get; set; }
}

public class TBSCertificate
{
    public string CertificateSerialNumber { get; set; }

    public SignatureAlg Signature { get; set; }

    //public Name Issuer { get; set; }

    public Validity Validity { get; set; }

    public Name Subject { get; set; }

    public Extension[] Extensions { get; set; }
}

public class Extension
{
    public string ExtensionType { get; set; }

    public string Critical { get; set; }

    //[XmlElement(Namespace = "http://www.roskazna.ru/eb/sign/types/sgv", ElementName = "extValue")]
    public XmlNode extValue { get; set; }
}

public class SignatureAlg
{
    public string AlgId { get; set; }
}

public class Name
{
    [XmlArray(Order = 0)]
    [XmlArrayItem("RelativeDistinguishedName", IsNullable = false)]
    [XmlArrayItem(IsNullable = false, NestingLevel = 1)]
    public AttributeTypeAndValue[][] DistinguishedName { get; set; }

    public String GetValue(String localName)
    {
        return DistinguishedName.Where(x => x.Select(z => z.Any.LocalName == localName).FirstOrDefault())?
                                .FirstOrDefault()?.FirstOrDefault()?.Any?.InnerText;
    }
}

[GeneratedCodeAttribute("System.Xml", "4.6.1055.0")]
[SerializableAttribute()]
[DebuggerStepThroughAttribute()]
[DesignerCategoryAttribute("code")]
[XmlTypeAttribute(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver")]
public class AttributeTypeAndValue
{
    [XmlElementAttribute(Order = 0)]
    public string AttributeType { get; set; }

    [XmlAnyElementAttribute(Order = 1)]
    public XmlElement Any { get; set; }
}

public class Validity
{
    public Time NotBefore { get; set; }

    public Time NotAfter { get; set; }
}

[GeneratedCodeAttribute("System.Xml", "4.6.1055.0")]
[SerializableAttribute()]
[DebuggerStepThroughAttribute()]
[DesignerCategoryAttribute("code")]
[XmlTypeAttribute(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver")]
public class Time
{
    [XmlElementAttribute("GeneralizedTime", typeof(string), Order = 0)]
    [XmlElementAttribute("UTCTime", typeof(string), Order = 0)]
    [XmlChoiceIdentifierAttribute("ItemElementName")]
    public string Item { get; set; }

    [XmlElementAttribute(Order = 1)]
    [XmlIgnoreAttribute()]
    public ItemChoiceType ItemElementName { get; set; }
}

[GeneratedCodeAttribute("System.Xml", "4.6.1055.0")]
[SerializableAttribute()]
[XmlTypeAttribute(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver", IncludeInSchema = false)]
public enum ItemChoiceType
{
    GeneralizedTime,

    UTCTime,
}

[GeneratedCodeAttribute("System.Xml", "4.6.1055.0")]
[SerializableAttribute()]
[DebuggerStepThroughAttribute()]
[DesignerCategoryAttribute("code")]
[XmlTypeAttribute(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver")]
public class SignatureRef
{
    [XmlElementAttribute("issuerAndSerial", typeof(SignerIdentifier), Order = 0)]
    [XmlElementAttribute("xmlID", typeof(string), Order = 0)]
    public object Item { get; set; }

    /// <summary>
    /// Серийный номер
    /// </summary>
    public string SerialNumber
    {
        get
        {
            var serialNumber = GetSerialNumber();
            if (string.IsNullOrEmpty(serialNumber))
                return string.Empty;

            var num = BigInteger.Parse(serialNumber);
            return num.ToString("X4");
        }
    }

    /// <summary>
    /// Общее имя
    /// </summary>
    public string CommonName { get { return GetIssuerAnyPropertyFromReference(nameof(CommonName)); } }

    //TODO опечатка?
    /// <summary>
    /// Неструктурированное имя
    /// </summary>
    public string UnstructuredName { get { return GetIssuerAnyPropertyFromReference("unstructuredName"); } }

    /// <summary>
    /// Email
    /// </summary>
    public string EmailAddress { get { return GetIssuerAnyPropertyFromReference(nameof(EmailAddress)); } }

    /// <summary>
    /// Регион
    /// </summary>
    public string StateOrProvinceName { get { return GetIssuerAnyPropertyFromReference(nameof(StateOrProvinceName)); } }

    /// <summary>
    /// ИНН
    /// </summary>
    public string INN { get { return GetIssuerAnyPropertyFromReference(nameof(INN)); } }

    /// <summary>
    /// ОГРН
    /// </summary>
    public string OGRN { get { return GetIssuerAnyPropertyFromReference(nameof(OGRN)); } }

    /// <summary>
    /// СНИЛС
    /// </summary>
    public string SNILS { get { return GetIssuerAnyPropertyFromReference(nameof(SNILS)); } }

    /// <summary>
    /// Адрес улицы
    /// </summary>
    public string StreetAddress { get { return GetIssuerAnyPropertyFromReference(nameof(StreetAddress)); } }

    /// <summary>
    /// Город
    /// </summary>
    public string LocalityName { get { return GetIssuerAnyPropertyFromReference(nameof(LocalityName)); } }

    /// <summary>
    /// Страна
    /// </summary>
    public string CountryName { get { return GetIssuerAnyPropertyFromReference(nameof(CountryName)); } }

    /// <summary>
    /// Организация
    /// </summary>
    public string OrganizationName { get { return GetIssuerAnyPropertyFromReference(nameof(OrganizationName)); } }

    private string GetIssuerAnyPropertyFromReference(string propertyName)
    {
        var signerIdentifier = Item as SignerIdentifier;

        if (signerIdentifier == null) return string.Empty;

        var issuerAndSerial = (IssuerAndSerial)signerIdentifier.Item;
        var issuer = issuerAndSerial.Issuer;

        return issuer.GetValue(propertyName);
    }

    private string GetSerialNumber()
    {
        var signerIdentifier = Item as SignerIdentifier;
        var issuerAndSerial = (IssuerAndSerial)signerIdentifier?.Item;
        return issuerAndSerial?.SerialNumber;
    }
}

[GeneratedCodeAttribute("System.Xml", "4.6.1055.0")]
[SerializableAttribute()]
[DebuggerStepThroughAttribute()]
[DesignerCategoryAttribute("code")]
[XmlTypeAttribute(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver")]
public class SignerIdentifier
{
    [XmlElementAttribute("IssuerAndSerial", typeof(IssuerAndSerial), Order = 0)]
    [XmlElementAttribute("KeyIdentifier", typeof(byte[]), DataType = "hexBinary", Order = 0)]
    public object Item { get; set; }
}

[GeneratedCodeAttribute("System.Xml", "4.6.1055.0")]
[SerializableAttribute()]
[DebuggerStepThroughAttribute()]
[DesignerCategoryAttribute("code")]
[XmlTypeAttribute(Namespace = "http://www.roskazna.ru/eb/sign/types/cryptoserver")]
public class IssuerAndSerial
{
    [XmlElementAttribute(Order = 0)]
    public Name Issuer { get; set; }

    [XmlElementAttribute(DataType = "integer", Order = 1)]
    public string SerialNumber { get; set; }
}

[XmlRoot(Namespace = "http://www.roskazna.ru/eb/sign/types/sgv")]
public class ServiceFaultInfo
{
    [XmlElement(Namespace = "http://www.roskazna.ru/eb/sign/types/sgv", ElementName = "comment")]
    public string Comment { get; set; }
}
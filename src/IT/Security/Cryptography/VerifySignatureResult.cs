using System;

namespace IT.Security.Cryptography
{
    public class VerifySignatureResult
    {
        public Boolean? IsVerify { get; set; }
        public string ResultStatus { get; set; }
        public Signature[] Signatures { get; set; }
        public int Count { get; set; }
        public string Error { get; set; }
        public string ValidationStatus { get; set; }
    }

    public class Signature
    {
        public int Number { get; set; }
        public string Status { get; set; }
        public Certificate Certificate { get; set; }
    }

    public class Certificate
    {
        public string SerialNumber { get; set; }
        public DateTime ValidityDateFrom { get; set; }
        public DateTime ValidityDateTo { get; set; }
        public Owner Owner { get; set; }
        public Issuer Issuer { get; set; }
    }

    public class Owner
    {
        public string FullName { get; set; }
        public string Organization { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string OGRNIP { get; set; }
        public string SNILS { get; set; }
        public string OGRN { get; set; }
        public string INN { get; set; }
        public string Email { get; set; }
    }

    public class Issuer
    {
        public string Name { get; set; }
        public string Organization { get; set; }
        public string Location { get; set; }
        public string CertificateNumber { get; set; }
        public string OGRN { get; set; }
        public string INN { get; set; }
        public string Email { get; set; }
    }
}
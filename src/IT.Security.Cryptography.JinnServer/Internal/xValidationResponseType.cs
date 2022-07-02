using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace IT.Security.Cryptography.JinnServer.Internal;

using Models;

internal static class xValidationResponseType
{
    public static Signatures ToVerifySignatureResult(this ValidationResponseType data)
    {
        var count = data.SignatureInfos == null ? 0 : data.SignatureInfos.SignatureInfo.Length;

        var signatures = new Signatures();
        signatures.Status = (Models.SignaturesStatus)(Int32)data.GlobalStatus;

        for (var i = 0; i < count; i++)
        {
            var signatureInfo = data.SignatureInfos.SignatureInfo[i];

            var signature = new Models.Signature
            {
                Status = (Models.SignatureStatus)(Int32)signatureInfo.Status,
            };

            if (signatureInfo.FailInfo != null)
            {
                signature.FaultComment = signatureInfo.FailInfo.Comment;
                signature.FaultType = (Models.SignatureFaultType?)(Int32?)signatureInfo.FailInfo.Type;
            }

            if (signatureInfo.SignerCertInfo != null)
            {
                //signature.Alg = signatureInfo.SignerCertInfo.Certificate.AlgorithmIdentifier.AlgId;
                //signature.Value = signatureInfo.SignerCertInfo.Certificate.Signature;
                signature.Certificate = Create(signatureInfo);

                if (string.IsNullOrWhiteSpace(signature.Certificate.Subject.FullName))
                    signature.Certificate.Subject.FullName = null;

                //signature.Certificate.Issuer.Location = $"{signature.Certificate.Subject.Country}, {signatureInfo.Reference.StateOrProvinceName}, {signatureInfo.Reference.LocalityName}, {signatureInfo.Reference.StreetAddress}";

                if (signatureInfo.SignerCertInfo.AuthorityCertSerial != null)
                {
                    //signature.Certificate.Issuer.CertificateNumber = BigInteger.Parse(signatureInfo.SignerCertInfo.AuthorityCertSerial).ToString("X");
                }
            }
            signatures.Add(signature);
        }
        return signatures;
    }

    private static Models.Certificate Create(SignatureInfo signatureInfo)
    {
        var ruFormat = CultureInfo.CreateSpecificCulture("ru-RU");
        return new Models.Certificate
        {
            SignatureAlg = signatureInfo.SignerCertInfo.Certificate.TBSCertificate.Signature.AlgId,
            SerialNumber = BigInteger.Parse(signatureInfo.SignerCertInfo.Certificate.TBSCertificate.CertificateSerialNumber).ToString("X"),
            ValidityFrom = DateTime.Parse(signatureInfo.SignerCertInfo.Certificate.TBSCertificate.Validity.NotBefore.Item.Replace("UTC", string.Empty), ruFormat),
            ValidityTo = DateTime.Parse(signatureInfo.SignerCertInfo.Certificate.TBSCertificate.Validity.NotAfter.Item.Replace("UTC", string.Empty), ruFormat),
            Subject = new CertificateSubject
            {
                FullName = $"{signatureInfo.SignerCertInfo.Surname} {signatureInfo.SignerCertInfo.GivenName}",
                Organization = signatureInfo.SignerCertInfo.OrganizationName,
                Country =
                            string.Equals(signatureInfo.SignerCertInfo.CountryName, "RU", StringComparison.CurrentCultureIgnoreCase)
                            || string.Equals(signatureInfo.SignerCertInfo.CountryName, "RUS", StringComparison.CurrentCultureIgnoreCase)
                                ? "Российская Федерация"
                                : signatureInfo.SignerCertInfo.CountryName,
                Region = signatureInfo.SignerCertInfo.StateOrProvinceName,
                City = signatureInfo.SignerCertInfo.LocalityName,
                INN = signatureInfo.SignerCertInfo.INN,
                SNILS = signatureInfo.SignerCertInfo.SNILS,
                Email = signatureInfo.SignerCertInfo.EmailAddress,
                OGRN = signatureInfo.SignerCertInfo.OGRN,
                OGRNIP = signatureInfo.SignerCertInfo.OGRNIP
            },
            Issuer = new CertificateIssuer
            {
                CommonName = signatureInfo.Reference.CommonName,
                Organization = signatureInfo.Reference.OrganizationName,
                OGRN = signatureInfo.Reference.OGRN,
                INN = signatureInfo.Reference.INN,
                Email = signatureInfo.Reference.EmailAddress
            }
        };
    }
}
using IT.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace IT.Security.Cryptography.JinnServer.Internal
{
    internal static class xValidationResponseType
    {
        public static VerifySignatureResult ToVerifySignatureResult(this Services.ValidationResponseType data)
        {
            var model = new VerifySignatureResult();

            //model.IsVerify = 
            model.ResultStatus = Res.Get(data.GlobalStatus);
            model.Count = data.SignatureInfos == null ? 0 : data.SignatureInfos.SignatureInfo.Length;
            var signatures = new List<Signature>();

            var ruFormat = CultureInfo.CreateSpecificCulture("ru-RU");

            for (var i = 0; i < model.Count; i++)
            {
                var signatureInfo = data.SignatureInfos.SignatureInfo[i];

                var signature = new Signature
                {
                    Number = i + 1,
                    Status = Res.Get(data.SignatureInfos.SignatureInfo[i].Status),
                    Certificate = new Certificate
                    {
                        SerialNumber = BigInteger.Parse(signatureInfo.SignerCertInfo.Certificate.TBSCertificate.CertificateSerialNumber).ToString("X"),
                        ValidityDateFrom = DateTime.Parse(signatureInfo.SignerCertInfo.Certificate.TBSCertificate.Validity.NotBefore.Item.Replace("UTC", string.Empty), ruFormat),
                        ValidityDateTo = DateTime.Parse(signatureInfo.SignerCertInfo.Certificate.TBSCertificate.Validity.NotAfter.Item.Replace("UTC", string.Empty), ruFormat),
                        Owner = new Owner
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
                        Issuer = new Issuer
                        {
                            Name = signatureInfo.Reference.CommonName,
                            Organization = signatureInfo.Reference.OrganizationName,
                            OGRN = signatureInfo.Reference.OGRN,
                            INN = signatureInfo.Reference.INN,
                            Email = signatureInfo.Reference.EmailAddress
                        }
                    }
                };

                if (string.IsNullOrWhiteSpace(signature.Certificate.Owner.FullName))
                    signature.Certificate.Owner.FullName = null;

                signature.Certificate.Issuer.Location = $"{signature.Certificate.Owner.Country}, {signatureInfo.Reference.StateOrProvinceName}, {signatureInfo.Reference.LocalityName}, {signatureInfo.Reference.StreetAddress}";

                if (signatureInfo.SignerCertInfo.AuthorityCertSerial != null)
                {
                    signature.Certificate.Issuer.CertificateNumber = BigInteger.Parse(signatureInfo.SignerCertInfo.AuthorityCertSerial).ToString("X");
                }

                signatures.Add(signature);
            }
            model.Signatures = signatures.ToArray();
            return model;
        }
    }
}
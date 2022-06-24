using System;

namespace IT.Security.Cryptography.JinnServer.Internal;

internal static class Soap
{
    public static class NS
    {
        public const String Envelope = "http://schemas.xmlsoap.org/soap/envelope/";
        public const String Cryptoserver = "http://www.roskazna.ru/eb/sign/types/cryptoserver";
        public const String SGV = "http://www.roskazna.ru/eb/sign/types/sgv";
    }

    public static class Actions
    {
        public const String Sign = NS.SGV + "/Sign";
        public const String Digest = NS.SGV + "/Digest";
        public const String Validate = NS.SGV + "/Validate";
    }

    public static class Request
    {
        public const String Digest = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""><s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><DigestRequestType xmlns=""http://www.roskazna.ru/eb/sign/types/sgv""><dataBytes>{0}</dataBytes><algorithmId>{1}</algorithmId></DigestRequestType></s:Body></s:Envelope>";
        public const String DigestPart = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""><s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><DigestRequestType xmlns=""http://www.roskazna.ru/eb/sign/types/sgv""><dataBytes>{0}</dataBytes><algorithmId>{1}</algorithmId><state>{2}</state></DigestRequestType></s:Body></s:Envelope>";

        public const String Sign = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""><s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><SigningRequestType xmlns=""http://www.roskazna.ru/eb/sign/types/sgv""><data>{0}</data><signatureType>{1}</signatureType><actor>http://smev.gosuslugi.ru/actors/smev</actor><algorithmId>{2}</algorithmId></SigningRequestType></s:Body></s:Envelope>";
        public const String SignMessage = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:cst=""http://www.roskazna.ru/eb/sign/types/cryptoserver"" xmlns:sgv=""http://www.roskazna.ru/eb/sign/types/sgv""><soapenv:Body>{0}</soapenv:Body></soapenv:Envelope>";

        public const String AutoSign = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""><s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><SigningRequestType xmlns=""http://www.roskazna.ru/eb/sign/types/sgv""><data>{0}</data><signatureType>{1}</signatureType><algorithmId>{2}</algorithmId></SigningRequestType></s:Body></s:Envelope>";
        public const String SignWithTransforms = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""><s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><SigningRequestType xmlns=""http://www.roskazna.ru/eb/sign/types/sgv""><data>{0}</data><signatureType>{1}</signatureType><detached>{2}</detached><xmlPartID>{3}</xmlPartID><transforms>{4}</transforms><businessProcessId>{5}</businessProcessId></SigningRequestType></s:Body></s:Envelope>";

        public const String Validation = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""><s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ValidationRequestType xmlns=""http://www.roskazna.ru/eb/sign/types/sgv""><signedData>{0}</signedData><createAdvanced>{1}</createAdvanced><algorithmId>{2}</algorithmId></ValidationRequestType></s:Body></s:Envelope>";
        public const String ValidationWithDetached = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""><s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ValidationRequestType xmlns = ""http://www.roskazna.ru/eb/sign/types/sgv""><signedData>{0}</signedData><externalData>{1}</externalData><createAdvanced>{2}</createAdvanced><algorithmId>{3}</algorithmId></ValidationRequestType></s:Body></s:Envelope>";

        public const String ValidationWithoutAlg = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""><s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ValidationRequestType xmlns=""http://www.roskazna.ru/eb/sign/types/sgv""><signedData>{0}</signedData><createAdvanced>{1}</createAdvanced></ValidationRequestType></s:Body></s:Envelope>";
        public const String ValidationWithDetachedWithoutAlg = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""><s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ValidationRequestType xmlns = ""http://www.roskazna.ru/eb/sign/types/sgv""><signedData>{0}</signedData><externalData>{1}</externalData><createAdvanced>{2}</createAdvanced></ValidationRequestType></s:Body></s:Envelope>";
    }

    public static class Regex
    {
        public const String Response = "<soapenv:Envelope(.*)><soapenv:Body>(.*)<\\/soapenv:Body><\\/soapenv:Envelope>";
        public const String ResponseValidationFault = "<soapenv:Envelope(.*)><soapenv:Body>(.*)<detail>(.*)<\\/detail>(.*)<\\/soapenv:Body><\\/soapenv:Envelope>";

        public const String ResponseSignFault = "<soapenv:Envelope(.*)><soapenv:Body>(.*)<\\/soapenv:Body><\\/soapenv:Envelope>";
        public const String ResponseSignMatchFault = "<soapenv:Envelope(.*)><soapenv:Body><tccs:ServiceFaultInfo (.*)<\\/tccs:ServiceFaultInfo><\\/soapenv:Body><\\/soapenv:Envelope>";
    }
}
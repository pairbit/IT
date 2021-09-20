using System.Xml.Serialization;

namespace IT.Security.Cryptography.JinnServer.Internal.Services
{
    [XmlRoot(Namespace = Soap.NS.Envelope)]
    internal class Envelope
    {
        public Body Body { get; set; }
    }
}
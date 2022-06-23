using System;
using System.Collections.Generic;

namespace IT.Security.Cryptography;

public class CryptoInformer : ICryptoInformer
{
    private readonly static Dictionary<String, String> _oids = new()
    {
        { "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr3411", "1.2.643.2.2.9" },
        { "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102001-gostr3411", "1.2.643.2.2.9" },

        { "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-256", "1.2.643.7.1.1.2.2" },
        { "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-256", "1.2.643.7.1.1.2.2" },

        { "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-512", "1.2.643.7.1.1.2.3" },
        { "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-512", "1.2.643.7.1.1.2.3" },
    };

    public String GetOid(String alg) => Hash.TryGetOid(alg) ?? (_oids.TryGetValue(alg, out var oid) ? oid : alg);
}
using IT.Security.Cryptography.BouncyCastle;

namespace IT.Security.Cryptography.Tests.Verifying;

public class BouncyCastle_RawSignatureVerifierTest : RawSignatureVerifierTest
{
    public BouncyCastle_RawSignatureVerifierTest() : base(new RawSignatureVerifier())
    {

    }
}
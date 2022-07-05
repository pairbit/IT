using Org.BouncyCastle.Security;

namespace IT.Security.Cryptography.Tests.Hashing;

public class BouncyCastleTest : HashTest
{
    private static readonly IT.Security.Cryptography.BouncyCastle.Hasher _hasher = new();

    public BouncyCastleTest() : base(_hasher)
    {
    }
}
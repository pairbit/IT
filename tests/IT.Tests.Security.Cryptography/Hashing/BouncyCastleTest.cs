using Org.BouncyCastle.Security;

namespace IT.Tests.Security.Cryptography.Hashing;

public class BouncyCastleTest : HashTest
{
    private static readonly IT.Security.Cryptography.BouncyCastle.Hasher _hasher = new();

    public BouncyCastleTest() : base(_hasher)
    {
    }
}
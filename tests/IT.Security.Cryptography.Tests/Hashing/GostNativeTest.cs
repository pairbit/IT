using IT.Security.Cryptography.Gost;

namespace IT.Security.Cryptography.Tests.Hashing;

public class GostNativeTest : HashTest
{
    private static readonly Hasher _hasher = new();

    public GostNativeTest() : base(_hasher)
    {

    }
}
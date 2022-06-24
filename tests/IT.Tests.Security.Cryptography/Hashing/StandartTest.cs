using IT.Security.Cryptography;

namespace IT.Tests.Security.Cryptography.Hashing;

public class StandartTest : HashTest
{
    private static readonly Hasher _hasher = new();

    public StandartTest() : base(_hasher)
    {

    }
}
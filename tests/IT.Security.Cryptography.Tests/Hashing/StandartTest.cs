namespace IT.Security.Cryptography.Tests.Hashing;

public class StandartTest : HashTest
{
    private static readonly Hasher _hasher = new();

    public StandartTest() : base(_hasher)
    {

    }
}
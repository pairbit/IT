using IT.Security.Cryptography.JinnServer;

namespace IT.Security.Cryptography.Tests.Hashing;

public class JinnServerTest : HashTest
{
    private static readonly SigningService _hasher = new(GetOptions, new CryptoInformer());

    public JinnServerTest() : base(_hasher)
    {

    }

    private static SigningOptions GetOptions()
    {
        return new SigningOptions { SigningUrl = "http://jinn.ucfk.ru/tccs/SigningService", PartInBytes = "700KB" };
    }
}
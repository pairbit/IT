using IT.Security.Cryptography;
using IT.Security.Cryptography.JinnServer;

namespace IT.Tests.Security.Cryptography.Hashing;

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
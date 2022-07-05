using IT.Security.Cryptography.JinnServer;

namespace IT.Security.Cryptography.Tests.Signing;

public class JinnServerSigner : SignTest
{
    private static readonly SigningService _signer = new(GetOptions, new CryptoInformer());

    public JinnServerSigner() : base(_signer)
    {

    }

    private static SigningOptions GetOptions()
    {
        return new SigningOptions { SigningUrl = "http://jinn.ucfk.ru/tccs/SigningService", PartInBytes = "700KB" };
    }
}
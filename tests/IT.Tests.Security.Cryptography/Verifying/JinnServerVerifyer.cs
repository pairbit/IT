using IT.Security.Cryptography.JinnServer;

namespace IT.Tests.Security.Cryptography.Verifying;

public class JinnServerVerifyer : VerifyTest
{
    private static readonly ValidationService _verifier = new(GetOptions);

    public JinnServerVerifyer() : base(_verifier)
    {

    }

    private static ValidationOptions GetOptions()
    {
        return new() { ValidationUrl = "http://jinn.ucfk.ru/tccs/SignatureValidationService" };
    }
}
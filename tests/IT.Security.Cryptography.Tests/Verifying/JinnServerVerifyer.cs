using IT.Security.Cryptography.JinnServer;

namespace IT.Security.Cryptography.Tests.Verifying;

public class JinnServerVerifyer : VerifyTest
{
    private static readonly ValidationService _verifier = new(GetOptions);

    public JinnServerVerifyer() : base(_verifier, _verifier)
    {

    }

    private static ValidationOptions GetOptions()
    {
        return new() { ValidationUrl = "http://jinn.ucfk.ru/tccs/SignatureValidationService" };
    }
}
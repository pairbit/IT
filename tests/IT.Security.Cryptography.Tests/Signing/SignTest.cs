using System.Text;

namespace IT.Security.Cryptography.Tests.Signing;

public abstract class SignTest
{
    private readonly ISigner _signer;

    public SignTest(ISigner signer)
    {
        _signer = signer;
    }

    [Test]
    public void Sign()
    {
        var data = Convert.ToBase64String(Encoding.UTF8.GetBytes("<Doc>My Data for Sign</Doc>"));

        var alg = _signer.Algs.ToArray()[0];

        foreach (var format in _signer.Formats)
        {
            Console.Write($"[{alg}][{format}]: ");
            try
            {
                var sign = _signer.Sign(alg, data, format, detached: false);

                Console.Write($"{format}: {sign}");
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.Write(ex.Message);
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }
}
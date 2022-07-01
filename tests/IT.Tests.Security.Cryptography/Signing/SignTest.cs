using IT.Security.Cryptography;
using System.Text;

namespace IT.Tests.Security.Cryptography.Signing;

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

        foreach (var format in _signer.Formats)
        {
            foreach (var alg in _signer.Algs)
            {
                Console.Write($"[{format}][{alg}]: ");
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
}
using System;

namespace SimpleBase;

internal sealed class Base58Alphabet : EncodingAlphabet
{
    private static readonly Lazy<Base58Alphabet> bitcoinAlphabet = new Lazy<Base58Alphabet>(()
        => new Base58Alphabet("123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz"));

    private static readonly Lazy<Base58Alphabet> rippleAlphabet = new Lazy<Base58Alphabet>(()
        => new Base58Alphabet("rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz"));

    private static readonly Lazy<Base58Alphabet> flickrAlphabet = new Lazy<Base58Alphabet>(()
        => new Base58Alphabet("123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ"));

    /// <summary>
    /// Initializes a new instance of the <see cref="Base58Alphabet"/> class
    /// using a custom alphabet.
    /// </summary>
    /// <param name="alphabet">Alphabet to use.</param>
    public Base58Alphabet(string alphabet)
        : base(58, alphabet)
    {
    }

    /// <summary>
    /// Gets Bitcoin alphabet.
    /// </summary>
    public static Base58Alphabet Bitcoin => bitcoinAlphabet.Value;

    /// <summary>
    /// Gets Base58 alphabet.
    /// </summary>
    public static Base58Alphabet Ripple => rippleAlphabet.Value;

    /// <summary>
    /// Gets Flickr alphabet.
    /// </summary>
    public static Base58Alphabet Flickr => flickrAlphabet.Value;
}
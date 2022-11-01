using System;
using System.Diagnostics;

namespace SimpleBase;

internal abstract class EncodingAlphabet
{
    /// <summary>
    /// Specifies the highest possible char value in an encoding alphabet
    /// Any char above with would raise an exception.
    /// </summary>
    private const int maxLength = 127;

    /// <summary>
    /// Holds a mapping from character to an actual byte value
    /// The values are held as "value + 1" so a zero would denote "not set"
    /// and would cause an exception.
    /// </summary>
    /// <remarks>byte[] has no discernible perf impact and saves memory.</remarks>
    private readonly byte[] reverseLookupTable = new byte[maxLength];

    /// <summary>
    /// Initializes a new instance of the <see cref="EncodingAlphabet"/> class.
    /// </summary>
    /// <param name="length">Length of the alphabe.</param>
    /// <param name="alphabet">Alphabet character.</param>
    public EncodingAlphabet(int length, string alphabet)
    {
        if (alphabet is null)
        {
            throw new ArgumentNullException(nameof(alphabet));
        }

        if (alphabet.Length != length)
        {
            throw new ArgumentException($"Required alphabet length is {length} but provided alphabet is "
                + $"{alphabet.Length} characters long");
        }

        this.Length = length;
        this.Value = alphabet;

        for (short i = 0; i < length; i++)
        {
            this.Map(alphabet[i], i);
        }
    }

    /// <summary>
    /// Gets the length of the alphabet.
    /// </summary>
    public int Length { get; private set; }

    /// <summary>
    /// Gets the characters of the alphabet.
    /// </summary>
    public string Value { get; private set; }

    internal ReadOnlySpan<byte> ReverseLookupTable => reverseLookupTable;

    /// <summary>
    /// Generates a standard invalid character exception for alphabets.
    /// </summary>
    /// <remarks>
    /// The reason this is not a throwing method itself is
    /// that the compiler has no way of knowing whether the execution
    /// will end after the method call and can incorrectly assume
    /// reachable code.
    /// </remarks>
    /// <param name="c">Characters.</param>
    /// <returns>Exception to be thrown.</returns>
    public static Exception InvalidCharacter(char c)
    {
        return new ArgumentException($"Invalid character: {c}");
    }

    /// <summary>
    /// Map a character to a value.
    /// </summary>
    /// <param name="c">Characters.</param>
    /// <param name="value">Corresponding value.</param>
    protected void Map(char c, int value)
    {
        Debug.Assert(c < maxLength, $"Alphabet contains character above {maxLength}");
        this.reverseLookupTable[c] = (byte)(value + 1);
    }
}
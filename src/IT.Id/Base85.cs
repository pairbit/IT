using K4os.Text.BaseX;

namespace System;

internal static class Base85
{
	private const string Z85Xml = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?_~|()[]{}@%$#";

	/// <summary>Z85 codec.</summary>
	private readonly static Base85ZCodec _codec = new(Z85Xml);

	public static void Decode(ReadOnlySpan<char> source, Span<byte> bytes)
	{
		_codec.Decode(source, bytes);
	}

	public static void Encode(ReadOnlySpan<byte> source, Span<char> bytes)
	{
		_codec.Encode(source, bytes);
	}

	public static String Encode(ReadOnlySpan<byte> source)
	{
		return _codec.Encode(source);
	}
}
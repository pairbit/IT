using System;

namespace K4os.Text.BaseX
{
	/// <summary>Static class with helper and factory methods for Base85 codec.</summary>
	internal static class Base85
	{
		//& на _
		//< на ~
		//> на |
		private const string DigitsX85 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?_~|()[]{}@%$#";

		private const string DigitsZ85 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#";

		private const string Digits85 =
			"!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
			"[\\]^_`abcdefghijklmnopqrstu";

		private const char DigitZ = 'z';

		/// <summary>Default Base85 codec.</summary>
		public static Base85Codec Default { get; } = new Base85Codec(Digits85, DigitZ);

		/// <summary>Z85 codec.</summary>
		public static Base85ZCodec X85 { get; } = new Base85ZCodec(DigitsX85);

		/// <summary>Z85 codec.</summary>
		public static Base85ZCodec Z85 { get; } = new Base85ZCodec(DigitsZ85);

		/// <summary>Converts byte array to Base85 string.</summary>
		/// <param name="decoded">Decoded buffer.</param>
		/// <returns>Base64 encoded string.</returns>
		public static string ToBase85(this byte[] decoded) => Default.Encode(decoded);
		
		/// <summary>Converts byte span to Base85 string.</summary>
		/// <param name="decoded">Decoded buffer.</param>
		/// <returns>Base64 encoded string.</returns>
		public static string ToBase85(this ReadOnlySpan<byte> decoded) => Default.Encode(decoded);

		/// <summary>Converts Base85 encoded string to byte array.</summary>
		/// <param name="encoded">Encoded string.</param>
		/// <returns>Decoded byte array.</returns>
		public static byte[] FromBase85(this string encoded) => Default.Decode(encoded);
	}
}
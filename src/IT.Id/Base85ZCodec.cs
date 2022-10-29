using System;
using System.Runtime.CompilerServices;

namespace K4os.Text.BaseX
{
	/// <summary>
	/// Base85Z codec. This is not full Ascii85 implementation is it does not
	/// handle whitespace not linebreaks. It is faster (because of that) though.
	/// </summary>
	internal class Base85ZCodec : BaseXCodec
	{
		private const uint U85P1 = 85u;
		private const uint U85P2 = 85u * 85u;
		private const uint U85P3 = 85u * 85u * 85u;
		private const uint U85P4 = 85u * 85u * 85u * 85u;

		/// <summary>Create new Base85 codec using specific set of digits.</summary>
		/// <param name="digits">Digits.</param>
		/// <exception cref="ArgumentException">Throw when given digits are not valid.</exception>
		public Base85ZCodec(string digits) :
			base(85, digits, true)
		{
		}

		/// <inheritdoc />
		public override unsafe int ErrorIndex(ReadOnlySpan<char> source)
		{
			var length = source.Length;
			fixed (char* source0 = source)
				return ErrorIndex(source0, length);
		}

		private unsafe int ErrorIndex(char* source0, int length)
		{
			var current = source0;
			var sourceE = source0 + length;
			var index = 0;
			while (current < sourceE)
			{
				var c = *current;
				if (IsValid(c))
				{
					current++;
					index = (index + 1) % 5;
				}
				else
				{
					return (int)(current - source0);
				}
			}

			if (index == 1) // not a valid padding
				return (int)(current - source0);

			return -1;
		}

		/// <inheritdoc />
		public override int MaximumDecodedLength(int sourceLength) =>
			sourceLength <= 0 ? 0 : sourceLength * 4;

		/// <inheritdoc />
		public override unsafe int DecodedLength(ReadOnlySpan<char> source)
			=> source.Length / 5 * 4;

		/// <inheritdoc />
		public override int MaximumEncodedLength(int sourceLength) =>
			sourceLength <= 0 ? 0 : (sourceLength + 3) / 4 * 5;

		/// <inheritdoc />
		protected override unsafe int DecodeImpl(
			char* source, int sourceLength, byte* target, int targetLength)
		{
			var target0 = target;
			var limit = source + sourceLength;
			var limit5 = limit - 5;

			fixed (byte* map = CharToByte)
			{
				while (source < limit5)
				{
					source = DecodeBlock(map, source, out var value4);
					target = WriteBlock(target, value4);
				}

				var left = (int)(limit - source);
				if (left == 1)
					throw new ArgumentException("Corrupted data, invalid padding");

				if (left > 1)
				{
					_ = DecodeTail(map, source, out var value4, left);
					target = WriteTail(target, value4, left - 1);
				}

				return (int)(target - target0);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe char* DecodeBlock(byte* map, char* source, out uint value4)
		{
			value4 =
				Decode1(map, *source) * U85P4 +
				Decode1(map, *(source + 1)) * U85P3 +
				Decode1(map, *(source + 2)) * U85P2 +
				Decode1(map, *(source + 3)) * U85P1 +
				Decode1(map, *(source + 4));

			return source + 5;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe char* DecodeTail(byte* map, char* source, out uint value4, int left)
		{
			var result = 0u;
			result += (left > 0 ? Decode1(map, *(source + 0)) : 84u) * U85P4;
			result += (left > 1 ? Decode1(map, *(source + 1)) : 84u) * U85P3;
			result += (left > 2 ? Decode1(map, *(source + 2)) : 84u) * U85P2;
			result += (left > 3 ? Decode1(map, *(source + 3)) : 84u) * U85P1;
			result += (left > 4 ? Decode1(map, *(source + 4)) : 84u);
			value4 = result;

			return source + left;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe byte* WriteBlock(byte* target, uint value4)
		{
			*(target + 0) = (byte)(value4 >> 24);
			*(target + 1) = (byte)(value4 >> 16);
			*(target + 2) = (byte)(value4 >> 8);
			*(target + 3) = (byte)value4;

			return target + 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe byte* WriteTail(byte* target, uint value4, int left)
		{
			if (left > 0) *(target + 0) = (byte)(value4 >> 24);
			if (left > 1) *(target + 1) = (byte)(value4 >> 16);
			if (left > 2) *(target + 2) = (byte)(value4 >> 8);
			if (left > 3) *(target + 3) = (byte)value4;

			return target + left;
		}

		/// <inheritdoc />
		protected override unsafe int EncodeImpl(
			byte* source, int sourceLength, char* target, int targetLength)
		{
			var target0 = target;
			var limit = source + (sourceLength & ~0x03u);
			var padding = 4 - (int)(sourceLength & 0x03u);

			fixed (char* map = ByteToChar)
			{
				while (source < limit)
				{
					var value4 = ReadBlock(source);
					source += 4;
					target = EncodeBlock(map, target, value4);
				}

				if (padding < 4)
				{
					var valueT = ReadTail(source, 4 - padding);
					target = EncodeTail(map, target, valueT, 5 - padding);
				}
			}

			return (int)(target - target0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe uint ReadBlock(byte* source) =>
			(uint)*(source + 0) << 24 |
			(uint)*(source + 1) << 16 |
			(uint)*(source + 2) << 8 |
			*(source + 3);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe uint ReadTail(byte* source, int left)
		{
			var result = 0u;
			if (left > 0) result |= (uint)*(source + 0) << 24;
			if (left > 1) result |= (uint)*(source + 1) << 16;
			if (left > 2) result |= (uint)*(source + 2) << 8;
			if (left > 3) result |= *(source + 3);

			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe char* EncodeBlock(char* map, char* target, uint value)
		{
			*(target + 0) = Encode1(map, (value / U85P4).Mod85());
			*(target + 1) = Encode1(map, (value / U85P3).Mod85());
			*(target + 2) = Encode1(map, (value / U85P2).Mod85());
			*(target + 3) = Encode1(map, (value / U85P1).Mod85());
			*(target + 4) = Encode1(map, value.Mod85());

			return target + 5;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe char* EncodeTail(char* map, char* target, uint value, int left)
		{
			if (left > 0) *(target + 0) = Encode1(map, (value / U85P4).Mod85());
			if (left > 1) *(target + 1) = Encode1(map, (value / U85P3).Mod85());
			if (left > 2) *(target + 2) = Encode1(map, (value / U85P2).Mod85());
			if (left > 3) *(target + 3) = Encode1(map, (value / U85P1).Mod85());
			if (left > 4) *(target + 4) = Encode1(map, value.Mod85());

			return target + left;
		}
	}
}
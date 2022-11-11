using System.Buffers;

namespace System.Text.Json.Serialization;

public class IdJsonConverter : JsonConverter<Id>
{
    public override Id Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String) throw new JsonException("Expected string");

        if (reader.HasValueSequence)
        {
            var seq = reader.ValueSequence;

            if (seq.Length != 16) throw new JsonException("Id must be 16 bytes long");

            Span<byte> buffer = stackalloc byte[16];

            seq.CopyTo(buffer);

            return Parse(buffer);
        }

        return Parse(reader.ValueSpan);
    }

    public override void Write(Utf8JsonWriter writer, Id value, JsonSerializerOptions options)
    {
        Span<byte> bytes = stackalloc byte[16];

        value.TryFormat(bytes, out _, default, null);

        writer.WriteStringValue(bytes);
    }

    private static Id Parse(ReadOnlySpan<Byte> bytes)
    {
        try
        {
            return Id.Parse(bytes);
        }
        catch (ArgumentException ex)
        {
            throw new JsonException($"Id invalid: {ex.Message}");
        }
        catch (FormatException ex)
        {
            throw new JsonException($"Id invalid: {ex.Message}");
        }
    }
}
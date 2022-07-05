namespace IT.Tests.Data;

public record Address
{
    public short Number { get; set; }

    public string Street { get; set; }

    public City City { get; set; }
}
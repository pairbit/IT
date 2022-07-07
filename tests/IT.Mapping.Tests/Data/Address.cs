namespace IT.Mapping.Tests.Data;

public record Address
{
    public short Number { get; set; }

    public string Street { get; set; }

    public City City { get; set; }
}

public record Address2
{
    public short Number { get; set; }

    public string Street { get; set; }

    public City2 City { get; set; }
}
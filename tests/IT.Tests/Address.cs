namespace IT.Tests;

public record Address
{
    public Int16 Number { get; set; }

    public string Street { get; set; }

    public City City { get; set; }
}
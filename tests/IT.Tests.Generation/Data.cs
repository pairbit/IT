namespace IT.Tests.Generation;

public record User
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime Created { get; set; }

    public Address[] Addresses { get; set; }

    public User Mother { get; set; }

    public User Father { get; set; }

    public User[] Childs { get; set; }
}

public record Address
{
    public Int16 Number { get; set; }

    public string Street { get; set; }

    public City City { get; set; }
}

public record City
{
    public string Name { get; set; }
}
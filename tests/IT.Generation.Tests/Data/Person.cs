namespace IT.Generation.Tests.Data;

public class MyList<T> : List<T>, IEquatable<MyList<T>>
{
    public MyList()
    {
    }

    public MyList(int capacity) : base(capacity)
    {
    }

    public MyList(IEnumerable<T> collection) : base(collection)
    {
    }

    public override bool Equals(object? obj) => Equals(obj as MyList<T>);

    public bool Equals(MyList<T>? other) => ReferenceEquals(this, other) || other is not null && this.SequenceEqual(other);

    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var item in this) hash.Add(item);

        return hash.ToHashCode();
    }
}

public record Person
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime Created { get; set; }

    public MyList<Address> Addresses { get; set; }

    public Person Mother { get; set; }

    public Person Father { get; set; }

    public MyList<Person> Childs { get; set; }
}
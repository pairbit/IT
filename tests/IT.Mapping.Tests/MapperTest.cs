using IT.Generation;
using IT.Mapping.Tests.Data;

namespace IT.Mapping.Tests;

public abstract class MapperTest
{
    private readonly IGenerator _generator = new Generation.KGySoft.Generator();
    private readonly IMapper _mapper;
    private readonly IMapper<Person, Person2> _personMapper;

    public MapperTest(IMapper mapper)
    {
        _mapper = mapper;
        _personMapper = new Mapper<Person, Person2>(mapper);
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CopyTest()
    {
        var city = _generator.Generate<City>();
        var cityCopy = _mapper.Map<City>(city);

        Assert.That(city, Is.EqualTo(cityCopy));

        var city2 = _generator.Generate<City2>();
        var city2Copy = _mapper.Map<City2>(city2);

        Assert.That(city2, Is.EqualTo(city2Copy));
    }

    [Test]
    public void CityTest()
    {
        var city = _generator.Generate<City>();
        var city2 = _mapper.Map<City2>(city);

        CityEquals(city, city2);

        city2 = _generator.Generate<City2>();
        city = _mapper.Map<City>(city2);

        CityEquals(city, city2);
    }

    [Test]
    public void AddressTest()
    {
        var address = _generator.Generate<Address>();
        var address2 = _mapper.Map<Address2>(address);

        AddressEquals(address, address2);

        address2 = _generator.Generate<Address2>();
        address = _mapper.Map<Address>(address2);

        AddressEquals(address, address2);
    }

    [Test]
    public void PersonTest()
    {
        var person = _generator.Generate<Person>();
        var person2 = _mapper.Map<Person2>(person);

        PersonEquals(person, person2);

        var person20 = _personMapper.Map(person);
        Assert.That(person2, Is.EqualTo(person20));

        var person21 = _mapper.Map<Person, Person2>(person);
        Assert.That(person2, Is.EqualTo(person21));

        var person21o = _mapper.Map(typeof(Person), person, typeof(Person2));
        Assert.That(person2, Is.EqualTo(person21o));

        Person2? person22 = null;
        var person23 = _mapper.Map(person, person22);
        Assert.False(ReferenceEquals(person22, person23));
        Assert.That(person2, Is.EqualTo(person23));

        person23 = _personMapper.Map(person, person22);
        Assert.False(ReferenceEquals(person22, person23));
        Assert.That(person2, Is.EqualTo(person23));

        var person23o = _mapper.Map(typeof(Person), person, typeof(Person2), person22);
        Assert.False(ReferenceEquals(person22, person23o));
        Assert.That(person2, Is.EqualTo(person23o));

        person22 = new Person2 { NotMapped2 = "NotMapped2" };
        person23 = _mapper.Map(person, person22);
        Assert.True(ReferenceEquals(person22, person23));
        person2.NotMapped2 = "NotMapped2";
        Assert.That(person2, Is.EqualTo(person23));

        person23 = _personMapper.Map(person, person22);
        Assert.True(ReferenceEquals(person22, person23));
        Assert.That(person2, Is.EqualTo(person23));

        person22 = new Person2 { NotMapped2 = "NotMapped2o" };
        var person22o = _mapper.Map(typeof(Person), person, typeof(Person2), person22);
        Assert.True(ReferenceEquals(person22, person22o));
        person2.NotMapped2 = "NotMapped2o";
        Assert.That(person2, Is.EqualTo(person22o));

        person2 = _generator.Generate<Person2>();
        person = _mapper.Map<Person>(person2);

        PersonEquals(person, person2);
    }

    private void PersonEquals(Person person, Person2 person2)
    {
        if (person is null && person2 is null) return;

        if (person is not null && person2 is not null)
        {
            Assert.False(person.Equals(person2));
            Assert.That(person.Id, Is.EqualTo(person2.Id));
            Assert.That(person.Name, Is.EqualTo(person2.Name));
            Assert.That(person.Age, Is.EqualTo(person2.Age));
            Assert.That(person.IsDeleted, Is.EqualTo(person2.IsDeleted));
            Assert.That(person.Created, Is.EqualTo(person2.Created));

            PersonEquals(person.Mother, person2.Mother);
            PersonEquals(person.Father, person2.Father);

            ListEquals(person.Addresses, person2.Addresses, AddressEquals);
            ListEquals(person.Childs, person2.Childs, PersonEquals);
        }
        else
        {
            Assert.Fail();
        }
    }

    private void ListEquals<T, T2>(IReadOnlyList<T> list, IReadOnlyList<T2> list2, Action<T, T2> equals)
    {
        if (list is null && list2 is null ) return;
        if ((list is null || list.Count == 0) && (list2 is null || list2.Count == 0))
        {
            Console.WriteLine($"Warning '{list?.Count}' != '{list2?.Count}'");
            return;
        }
        if (list is not null && list2 is not null)
        {
            Assert.False(list.Equals(list2));
            Assert.That(list.Count, Is.EqualTo(list2.Count));

            for (int i = 0; i < list.Count; i++)
            {
                equals(list[i], list2[i]);
            }
        }
        else
        {
            Assert.Fail();
        }
    }

    private void AddressEquals(Address address, Address2 address2)
    {
        Assert.False(address.Equals(address2));
        Assert.That(address.Street, Is.EqualTo(address2.Street));
        Assert.That(address.Number, Is.EqualTo(address2.Number));

        CityEquals(address.City, address2.City);
    }

    private void CityEquals(City city, City2 city2)
    {
        Assert.False(city.Equals(city2));
        Assert.That(city.Name, Is.EqualTo(city2.Name));
        Assert.That(city.Count, Is.EqualTo(city2.Count));
    }
}
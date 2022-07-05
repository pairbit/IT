//namespace IT.Tests.System;

//public class Tests
//{
//    [SetUp]
//    public void Setup()
//    {

//    }

//    [Test]
//    public void Test1()
//    {
//        var cities = new City[] { new City { Name = "Ganalulo" }, new City { Name = "Berlin" } };
//        var cities2 = new City[] { new City { Name = "Ganalulo" }, new City { Name = "Berlin" } };

//        var hash1 = cities.GetHashCode();
//        var hash2 = cities2.GetHashCode();

//        Assert.False(ReferenceEquals(cities, cities2));
//        Assert.False(hash1.Equals(hash2));
//        Assert.False(EqualityComparer<City[]>.Default.Equals(cities, cities2));

//        var mylist = new MyList<City>(cities);
//        var mylist2 = new MyList<City>(cities2);

//        hash1 = mylist.GetHashCode();
//        hash2 = mylist2.GetHashCode();

//        Assert.False(ReferenceEquals(mylist, mylist2));
//        Assert.True(hash1.Equals(hash2));
//        Assert.True(EqualityComparer<MyList<City>>.Default.Equals(mylist, mylist2));
//    }

//}
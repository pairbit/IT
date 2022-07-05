using IT.Data;
using IT.Generation;

namespace IT.Data.Tests;

public abstract class DataTest<TId, TValue>
{
    private readonly IDataRepository<TId, TValue> _repo;
    private readonly IGenerator _generator = new Generation.KGySoft.Generator();

    public DataTest(IDataRepository<TId, TValue> repo)
    {
        _repo = repo;
    }

    [Test]
    public void Create()
    {
        var value = NewValue();

        var id = _repo.Create(value);

        var idComparer = EqualityComparer<TId>.Default;

        Assert.False(idComparer.Equals(id, default));

        var id2 = _repo.Create(value);

        Assert.False(idComparer.Equals(id2, default));

        Assert.False(idComparer.Equals(id, id2));

        var valueComparer = EqualityComparer<TValue>.Default;

        Assert.True(valueComparer.Equals(value, _repo.GetById(id)));
        Assert.True(valueComparer.Equals(value, _repo.GetById(id2)));

        Assert.True(_repo.ExistsById(id));
        Assert.True(_repo.DeleteById(id));
        Assert.False(_repo.ExistsById(id));
        var count = _repo.DeleteByIds(id, id2);
        Assert.True(count == 1);
    }

    [Test]
    public void CreateById()
    {
        var value = NewValue();
        var id = NewId();

        Assert.False(_repo.ExistsById(id));

        Assert.True(_repo.CreateById(value, id));

        Assert.True(_repo.ExistsById(id));

        Assert.False(_repo.CreateById(value, id));

        Assert.True(_repo.DeleteById(id));

        Assert.True(_repo.DeleteByIds(NewId(), NewId(), NewId()) == 0);
    }

    private TId NewId() => _generator.Generate<TId>();

    private TValue NewValue() => _generator.Generate<TValue>();
}
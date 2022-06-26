namespace IT.Data;

public readonly record struct IdValue<TId, TValue>(TId Id, TValue Value);
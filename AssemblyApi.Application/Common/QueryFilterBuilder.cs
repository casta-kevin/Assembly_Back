using System;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyApi.Application.Common;

public class QueryFilterBuilder<T>
{
    private readonly List<Func<IQueryable<T>, IQueryable<T>>> _filters = new();

    public QueryFilterBuilder<T> Add(bool condition, Func<IQueryable<T>, IQueryable<T>> filter)
    {
        if (condition)
            _filters.Add(filter);

        return this;
    }

    public IQueryable<T> Apply(IQueryable<T> query)
    {
        foreach (var filter in _filters)
        {
            query = filter(query);
        }

        return query;
    }

    public static QueryFilterBuilder<T> Create() => new();
}

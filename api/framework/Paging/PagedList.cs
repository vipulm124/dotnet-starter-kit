﻿using FSH.Framework.Abstractions.Paging;
using Mapster;

namespace FSH.Framework.Paging;

public record PagedList<T>(IReadOnlyList<T> Items, int PageNumber, int PageSize, int TotalCount) : IPagedList<T>
    where T : class
{
    public int CurrentPageSize => Items.Count;
    public int CurrentStartIndex => TotalCount == 0 ? 0 : ((PageNumber - 1) * PageSize) + 1;
    public int CurrentEndIndex => TotalCount == 0 ? 0 : CurrentStartIndex + CurrentPageSize - 1;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
    public IPagedList<TR> MapTo<TR>(Func<T, TR> map)
        where TR : class
    {
        return new PagedList<TR>(Items.Select(map).ToList(), PageNumber, PageSize, TotalCount);
    }
    public IPagedList<TR> MapTo<TR>()
        where TR : class
    {
        return new PagedList<TR>(Items.Adapt<IReadOnlyList<TR>>(), PageNumber, PageSize, TotalCount);
    }
}
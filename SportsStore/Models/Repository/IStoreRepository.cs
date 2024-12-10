using System;
public interface IStoreRepository
{
    IQueryable<Product> Products { get; }
}

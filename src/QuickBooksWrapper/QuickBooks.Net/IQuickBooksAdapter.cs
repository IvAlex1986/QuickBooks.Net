using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using System.Collections.Generic;

namespace QuickBooks.Net
{
    public interface IQuickBooksAdapter
    {
        ServiceContext Context { get; }

        IEnumerable<T> FindAll<T>() where T : IEntity, new();

        T Add<T>(T entity) where T : IEntity;

        T Update<T>(T entity) where T : IEntity;

        T Delete<T>(T entity) where T : IEntity;
    }
}

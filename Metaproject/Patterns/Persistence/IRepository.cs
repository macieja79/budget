using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace Metaproject.Patterns
{
    public interface IRepository<T>
    {
        T Get(Guid id);
        IEnumerable<T> Find(Expression<Func<T, bool>> pred);
        IEnumerable<T> Find(Func<T, bool> pred);
        IEnumerable<T> GetAll();
        void Add(T item);
        void Remove(T item);
    }
}

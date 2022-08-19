using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public interface IRepository<T> where T : class
    {
        // T  - Category for now

        T GetFirstorDefault(Expression<Func<T, bool>> filter, string? IncludeProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? IncludeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);


    }
}

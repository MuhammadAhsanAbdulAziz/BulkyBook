using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbset;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
        }


        public void Add(T entity)
        {
            dbset.Add(entity);
        }
        // Includeprop = "Category,CoverType"
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? IncludeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (IncludeProperties != null)
            {
                foreach(var includeprop in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeprop);
                    }
            }
            return query.ToList();
        }

        public T GetFirstorDefault(Expression<Func<T, bool>> filter, string? IncludeProperties = null)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);
            if (IncludeProperties != null)
            {
                foreach (var includeprop in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }
    }
}

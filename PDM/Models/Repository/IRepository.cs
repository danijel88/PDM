using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDM.Models.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> SaveChangesAsync();
        void SaveChanges();
        void Dispose();
        T Get(int id);
        T Get(Expression<Func<T, bool>> predicate);
        T Get(Expression<Func<T, bool>> predicate, string[] includes);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(string[] includes);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, string[] includes);
    }
}

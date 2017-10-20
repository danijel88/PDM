using Microsoft.EntityFrameworkCore;
using PDM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDM.Models.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private ApplicationDbContext _context;
        private DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            entities = _context.Set<T>();
        }
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }
            entities.Remove(entity);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public T Get(int id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return entities.Where(predicate).SingleOrDefault();
        }

        public T Get(Expression<Func<T, bool>> predicate, string[] includes)
        {
            return includes
                    .Aggregate(entities.AsQueryable(), (query, path) => query.Include(path))
                    .Where(predicate)
                    .SingleOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return entities.ToList();
        }

        public IEnumerable<T> GetAll(string[] includes)
        {
            return includes.Aggregate(entities.AsQueryable(), (query, path) => query.Include(path));
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return entities.Where(predicate).ToList();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, string[] includes)
        {
            return includes
                    .Aggregate(entities.AsQueryable(), (query, path) => query.Include(path))
                    .Where(predicate)
                    .ToList();
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }
            entities.Add(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);

        }

        public void Update(T entity)
        {

            entities.Update(entity);
        }
    }
}

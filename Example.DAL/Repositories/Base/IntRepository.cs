﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Example.DAL.Entities.Abstract;
using Example.DAL.Repositories.Abstract;

namespace Example.DAL.Repositories.Base
{
    public class IntRepository<TEntity> : IIntRepository<TEntity> where TEntity : class, IIntEntity
    {
        private readonly IDbContextProvider _dbContextProvider;

        public IntRepository(IDbContextProvider dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        protected DbContext DbContext { get { return _dbContextProvider.Context; } }

        protected DbSet<TEntity> Set { get { return DbContext.Set<TEntity>(); } }

        public virtual TEntity FindById(int id)
        {
            return Set.Find(id);
        }

        public virtual Task<TEntity> FindByIdAsync(int id)
        {
            return Set.FindAsync(id);
        }

        public virtual IEnumerable<TEntity> FindByIds(IEnumerable<int> ids)
        {
            return Set.Where(e => ids.Contains(e.Id)).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<int> ids)
        {
            return await Set.Where(e => ids.Contains(e.Id)).ToListAsync();
        }

        public virtual IEnumerable<TEntity> FindByIds(IEnumerable<string> ids)
        {
            return Set.Where(e => ids.Contains(e.Id.ToString())).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<string> ids)
        {
            return await Set.Where(e => ids.Contains(e.Id.ToString())).ToListAsync();
        }

        //public virtual IEnumerable<TEntity> GetAll()
        //{
        //    return Set.ToList();
        //}

        //public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        //{
        //    return await Set.ToListAsync();
        //}

        public virtual TEntity Insert(TEntity entity)
        {
            return Set.Add(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual void Delete(int id)
        {
            var entity = Set.Find(id);

            if (entity == null)
            {
                return;
            }

            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Set.Remove(entity);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return Set.AsQueryable();
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            if (!Set.Local.Contains(entity))
            {
                Set.Attach(entity);
            }
        }
    }
}

using DataBaseAccess.DbContext;
using DataBaseAccess.Repsitory;
using System.Collections.Generic;

namespace DataBaseAccess.Repsitories
{
    public abstract class BaseRepository<T> : IRepository<T> where T: class
    {
        protected readonly ApplicationDbContext _applicationDbContext;

        public BaseRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public abstract void Create(T item);
        public abstract void Delete(T item);
        public abstract T Get(long itemId);
        public abstract IEnumerable<T> GetAll();
        public abstract void Update(T item);
    }
}

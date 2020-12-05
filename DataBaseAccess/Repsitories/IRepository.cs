using System.Collections.Generic;

namespace DataBaseAccess.Repsitory
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(); 
        T Get(long itemId); 
        void Create(T item);
        void Update(T item); 
        void Delete(T item);
    }
}

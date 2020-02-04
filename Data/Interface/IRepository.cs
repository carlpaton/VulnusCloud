using System.Collections.Generic;

namespace Data.Interface
{
    public interface IRepository<T>
    {
        T Select(int id);
        List<T> SelectList();
        int Insert(T obj);
        void InsertBulk(List<T> list);
        void Update(T obj);
        void Delete(int id);
    }
}

using System.Collections.Generic;

namespace Data.Implementation
{
    public interface IBaseContext
    {
        void Open();

        T Select<T>(string sql, object parameters = null) where T : new();
        List<T> SelectList<T>(string sql, object parameters = null);

        int Insert(string storedProc, object poco);

        void Update(string storedProc, object poco);
        void Delete(string storedProc, int id);

        void ExecuteNonQuery(string sql);
    }
}

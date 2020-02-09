using System.Collections.Generic;
using Data.Implementation;
using Data.Interface;
using Data.Schema;

namespace Data.MsSQL
{
    public class ComponentRepository : MsSQLContext, IComponentRepository
    {
        private readonly string _connectionString;
        public ComponentRepository(string connectionString) : base(connectionString)
        {
            // Shim for BulkInsert
            _connectionString = connectionString;
        }

        public ComponentModel Select(int id)
        {
            var storedProc = "sp_select_component";
            return Select<ComponentModel>(storedProc, new { id });
        }

        public List<ComponentModel> SelectList()
        {
            var storedProc = "sp_selectlist_component";
            return SelectList<ComponentModel>(storedProc);
        }

        public int Insert(ComponentModel obj)
        {
            var storedProc = "sp_insert_component";
            var insertObj = new
            {
                name = obj.Name
            };
            return Insert(storedProc, insertObj);
        }

        public void InsertBulk(List<ComponentModel> listPoco)
        {
            foreach (var obj in listPoco)
            {
                // sweet hack, although a new connection per insert will probably be used -_- perhaps it will pool? meh :D
                // probably better to just have the sql command text in the code for a bulk insert
                new ComponentRepository(_connectionString).Insert(obj);
            }
        }

        public void Update(ComponentModel obj)
        {
            var storedProc = "sp_update_component";
            var updateObj = new
            {
                id = obj.Id,
                name = obj.Name
            };
            Update(storedProc, updateObj);
        }

        public void Delete(int id)
        {
            var storedProc = "sp_delete_component";
            Delete(storedProc, id);
        }

        public ComponentModel SelectByName(string name)
        {
            var storedProc = "sp_select_by_name_component";
            return Select<ComponentModel>(storedProc, new { name });
        }
    }
}

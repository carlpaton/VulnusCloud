using System.Collections.Generic;
using Data.Implementation;
using Data.Interface;
using Data.Schema;

namespace Data.MsSQL
{
   public class OssIndexRepository : MsSQLContext, IOssIndexRepository
   {
       private readonly string _connectionString;
        public OssIndexRepository(string connectionString) : base(connectionString)
        {
            // Shim for BulkInsert
            _connectionString = connectionString;
        }

        public OssIndexModel Select(int id)
        {
           var storedProc = "sp_select_oss_index";
           return Select<OssIndexModel>(storedProc, new { id });
        }

      public List<OssIndexModel> SelectList()
      {
           var storedProc = "sp_selectlist_oss_index";
           return SelectList<OssIndexModel>(storedProc);
      }

      public int Insert(OssIndexModel obj)
      {
           var storedProc = "sp_insert_oss_index";
           var insertObj = new
           {
                component_id = obj.ComponentId,
                version = obj.Version,
                type_format = obj.TypeFormat,
                coordinates = obj.Coordinates,
                description = obj.Description,
                reference = obj.Reference,
                expire_date = obj.ExpireDate,
                http_status = obj.HttpStatus
           };
           return Insert(storedProc, insertObj);
      }

      public void InsertBulk(List<OssIndexModel> listPoco)
      {
         foreach (var obj in listPoco)
         {
            // sweet hack, although a new connection per insert will probably be used -_- perhaps it will pool? meh :D
            // probably better to just have the sql command text in the code for a bulk insert
            new OssIndexRepository(_connectionString).Insert(obj);
         }
      }

      public void Update(OssIndexModel obj)
      {
           var storedProc = "sp_update_oss_index";
           var updateObj = new
           {
                id = obj.Id,
                component_id = obj.ComponentId,
                version = obj.Version,
                type_format = obj.TypeFormat,
                coordinates = obj.Coordinates,
                description = obj.Description,
                reference = obj.Reference,
                expire_date = obj.ExpireDate,
                http_status = obj.HttpStatus
           };
           Update(storedProc, updateObj);
      }

      public void Delete(int id)
      {
           var storedProc = "sp_delete_oss_index";
           Delete(storedProc, id);
      }
   }
}

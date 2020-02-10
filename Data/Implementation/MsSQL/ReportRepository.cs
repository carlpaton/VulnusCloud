using System.Collections.Generic;
using Data.Implementation;
using Data.Interface;
using Data.Schema;

namespace Data.MsSQL
{
   public class ReportRepository : MsSQLContext, IReportRepository
   {
       private readonly string _connectionString;
        public ReportRepository(string connectionString) : base(connectionString)
        {
            // Shim for BulkInsert
            _connectionString = connectionString;
        }

        public ReportModel Select(int id)
        {
           var storedProc = "sp_select_report";
           return Select<ReportModel>(storedProc, new { id });
        }

      public List<ReportModel> SelectList()
      {
           var storedProc = "sp_selectlist_report";
           return SelectList<ReportModel>(storedProc);
      }

      public int Insert(ReportModel obj)
      {
           var storedProc = "sp_insert_report";
           var insertObj = new
           {
                project_id = obj.ProjectId,
                insert_date = obj.InsertDate
           };
           return Insert(storedProc, insertObj);
      }

      public void InsertBulk(List<ReportModel> listPoco)
      {
         foreach (var obj in listPoco)
         {
            // sweet hack, although a new connection per insert will probably be used -_- perhaps it will pool? meh :D
            // probably better to just have the sql command text in the code for a bulk insert
            new ReportRepository(_connectionString).Insert(obj);
         }
      }

      public void Update(ReportModel obj)
      {
           var storedProc = "sp_update_report";
           var updateObj = new
           {
                id = obj.Id,
                project_id = obj.ProjectId,
                insert_date = obj.InsertDate
           };
           Update(storedProc, updateObj);
      }

      public void Delete(int id)
      {
           var storedProc = "sp_delete_report";
           Delete(storedProc, id);
      }

        public List<ReportModel> SelectByProjectId(int id)
        {
            var storedProc = "sp_selectlist_report_by_project_id";
            return SelectList<ReportModel>(storedProc, new { id });
        }
    }
}

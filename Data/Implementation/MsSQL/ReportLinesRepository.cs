using System.Collections.Generic;
using Data.Implementation;
using Data.Interface;
using Data.Schema;

namespace Data.MsSQL
{
   public class ReportLinesRepository : MsSQLContext, IReportLinesRepository
   {
       private readonly string _connectionString;
        public ReportLinesRepository(string connectionString) : base(connectionString)
        {
            // Shim for BulkInsert
            _connectionString = connectionString;
        }

        public ReportLinesModel Select(int id)
        {
           var storedProc = "sp_select_report_lines";
           return Select<ReportLinesModel>(storedProc, new { id });
        }

      public List<ReportLinesModel> SelectList()
      {
           var storedProc = "sp_selectlist_report_lines";
           return SelectList<ReportLinesModel>(storedProc);
      }

      public int Insert(ReportLinesModel obj)
      {
           var storedProc = "sp_insert_report_lines";
           var insertObj = new
           {
                report_id = obj.ReportId,
                oss_index_id = obj.OssIndexId
           };
           return Insert(storedProc, insertObj);
      }

      public void InsertBulk(List<ReportLinesModel> listPoco)
      {
         foreach (var obj in listPoco)
         {
            // sweet hack, although a new connection per insert will probably be used -_- perhaps it will pool? meh :D
            // probably better to just have the sql command text in the code for a bulk insert
            new ReportLinesRepository(_connectionString).Insert(obj);
         }
      }

      public void Update(ReportLinesModel obj)
      {
           var storedProc = "sp_update_report_lines";
           var updateObj = new
           {
                id = obj.Id,
                report_id = obj.ReportId,
                oss_index_id = obj.OssIndexId
           };
           Update(storedProc, updateObj);
      }

      public void Delete(int id)
      {
           var storedProc = "sp_delete_report_lines";
           Delete(storedProc, id);
      }
   }
}

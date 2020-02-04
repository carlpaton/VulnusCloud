using System.Collections.Generic;
using Data.Implementation;
using Data.Interface;
using Data.Schema;

namespace Data.MsSQL
{
   public class ProjectRepository : MsSQLContext, IProjectRepository
   {
       private readonly string _connectionString;
        public ProjectRepository(string connectionString) : base(connectionString)
        {
            // Shim for BulkInsert
            _connectionString = connectionString;
        }

        public ProjectModel Select(int id)
        {
           var storedProc = "sp_select_project";
           return Select<ProjectModel>(storedProc, new { id });
        }

      public List<ProjectModel> SelectList()
      {
           var storedProc = "sp_selectlist_project";
           return SelectList<ProjectModel>(storedProc);
      }

      public int Insert(ProjectModel obj)
      {
           var storedProc = "sp_insert_project";
           var insertObj = new
           {
                project_name = obj.ProjectName
           };
           return Insert(storedProc, insertObj);
      }

      public void InsertBulk(List<ProjectModel> listPoco)
      {
         foreach (var obj in listPoco)
         {
            // sweet hack, although a new connection per insert will probably be used -_- perhaps it will pool? meh :D
            // probably better to just have the sql command text in the code for a bulk insert
            new ProjectRepository(_connectionString).Insert(obj);
         }
      }

      public void Update(ProjectModel obj)
      {
           var storedProc = "sp_update_project";
           var updateObj = new
           {
                id = obj.Id,
                project_name = obj.ProjectName
           };
           Update(storedProc, updateObj);
      }

      public void Delete(int id)
      {
           var storedProc = "sp_delete_project";
           Delete(storedProc, id);
      }
   }
}

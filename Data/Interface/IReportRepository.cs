using Data.Schema;
using System.Collections.Generic;

namespace Data.Interface 
{
   public interface IReportRepository : IRepository<ReportModel>
   {
        List<ReportModel> SelectByProjectId(int id);
    }
}

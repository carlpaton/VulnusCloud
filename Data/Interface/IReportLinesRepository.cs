using Data.Schema;
using System.Collections.Generic;

namespace Data.Interface 
{
   public interface IReportLinesRepository : IRepository<ReportLinesModel>
   {
        List<ReportLinesModel> SelectListByReportId(int id);
    }
}

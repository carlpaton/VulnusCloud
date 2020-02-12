using Data.Schema;
using System.Collections.Generic;

namespace Data.Interface 
{
   public interface IOssIndexVulnerabilitiesRepository : IRepository<OssIndexVulnerabilitiesModel>
   {
        List<OssIndexVulnerabilitiesModel> SelectlistByOssIndexId(int id);
   }
}

using Data.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interface 
{
   public interface IOssIndexRepository : IRepository<OssIndexModel>
   {
        OssIndexModel SelectByComponentId(int id);

        List<OssIndexModel> SelectByHttpStatus(int httpStatus);

        Task<IEnumerable<OssIndexModel>> SelectByHttpStatusAsync(int httpStatus);
    }
}

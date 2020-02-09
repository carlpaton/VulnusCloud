using Data.Schema; 

namespace Data.Interface 
{
   public interface IOssIndexRepository : IRepository<OssIndexModel>
   {
        OssIndexModel SelectByComponentId(int id);
    }
}

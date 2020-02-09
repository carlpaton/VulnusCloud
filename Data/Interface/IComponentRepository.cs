using Data.Schema; 

namespace Data.Interface 
{
   public interface IComponentRepository : IRepository<ComponentModel>
   {
        ComponentModel SelectByName(string name);
    }
}

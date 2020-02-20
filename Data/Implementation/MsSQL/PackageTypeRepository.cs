using System.Collections.Generic;
using System.Linq;
using Data.Interface;
using Data.Schema;

namespace Data.Implementation.MsSQL
{
    // TODO - this should be in the database, fine for now but not extensible

    public class PackageTypeRepository : IPackageTypeRepository
    {
        public PackageTypeModel Select(int id)
        {
            return SelectList()
                .Where(x => x.Id == id)
                .First();
        }

        public List<PackageTypeModel> SelectList()
        {
            return new List<PackageTypeModel>() 
            {
                new PackageTypeModel()
                {
                    Id = 1,
                    Name = "Nuget"
                }
            };
        }
    }
}

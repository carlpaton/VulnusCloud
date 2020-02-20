using Data.Schema;
using System.Collections.Generic;

namespace Data.Interface
{
    public interface IPackageTypeRepository
    {
        PackageTypeModel Select(int id);
        List<PackageTypeModel> SelectList();
    }
}

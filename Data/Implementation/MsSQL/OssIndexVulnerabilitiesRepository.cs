using System.Collections.Generic;
using Data.Implementation;
using Data.Interface;
using Data.Schema;

namespace Data.MsSQL
{
    public class OssIndexVulnerabilitiesRepository : MsSQLContext, IOssIndexVulnerabilitiesRepository
    {
        private readonly string _connectionString;
        public OssIndexVulnerabilitiesRepository(string connectionString) : base(connectionString)
        {
            // Shim for BulkInsert
            _connectionString = connectionString;
        }

        public OssIndexVulnerabilitiesModel Select(int id)
        {
            var storedProc = "sp_select_oss_index_vulnerabilities";
            return Select<OssIndexVulnerabilitiesModel>(storedProc, new { id });
        }

        public List<OssIndexVulnerabilitiesModel> SelectList()
        {
            var storedProc = "sp_selectlist_oss_index_vulnerabilities";
            return SelectList<OssIndexVulnerabilitiesModel>(storedProc);
        }

        public int Insert(OssIndexVulnerabilitiesModel obj)
        {
            var storedProc = "sp_insert_oss_index_vulnerabilities";
            var insertObj = new
            {
                oss_index_id = obj.OssIndexId,
                insert_date = obj.InsertDate,
                oss_id = obj.OssId,
                title = obj.Title,
                description = obj.Description,
                cvssScore = obj.CvssScore,
                cvssVector = obj.CvssVector,
                cve = obj.Cve,
                reference = obj.Reference
            };
            return Insert(storedProc, insertObj);
        }

        public void InsertBulk(List<OssIndexVulnerabilitiesModel> listPoco)
        {
            foreach (var obj in listPoco)
            {
                // sweet hack, although a new connection per insert will probably be used -_- perhaps it will pool? meh :D
                // probably better to just have the sql command text in the code for a bulk insert
                new OssIndexVulnerabilitiesRepository(_connectionString).Insert(obj);
            }
        }

        public void Update(OssIndexVulnerabilitiesModel obj)
        {
            var storedProc = "sp_update_oss_index_vulnerabilities";
            var updateObj = new
            {
                id = obj.Id,
                oss_index_id = obj.OssIndexId,
                insert_date = obj.InsertDate,
                oss_id = obj.OssId,
                title = obj.Title,
                description = obj.Description,
                cvssScore = obj.CvssScore,
                cvssVector = obj.CvssVector,
                cve = obj.Cve,
                reference = obj.Reference
            };
            Update(storedProc, updateObj);
        }

        public void Delete(int id)
        {
            var storedProc = "sp_delete_oss_index_vulnerabilities";
            Delete(storedProc, id);
        }

        public List<OssIndexVulnerabilitiesModel> SelectlistByOssIndexId(int id)
        {
            var storedProc = "sp_selectlist_oss_index_vulnerabilities_by_oss_index_id";
            return SelectList<OssIndexVulnerabilitiesModel>(storedProc, new { id });
        }
    }
}

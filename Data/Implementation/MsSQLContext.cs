using Dapper;
using System.Data.SqlClient; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Implementation
{
    public class MsSQLContext : IDisposable, IBaseContext
    {
        private SqlConnection _dbConn;
        private readonly string _connectionString = "";

        public MsSQLContext(string connectionString)
        {
            _connectionString = connectionString;
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public void Delete(string storedProc, int id)
        {
            using (_dbConn)
            {
                Open();
                _dbConn.Execute(
                    storedProc, 
                    new { id },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public int Insert(string storedProc, object poco)
        {
            using (_dbConn)
            {
                Open();
                return _dbConn.ExecuteScalar<int>(
                    storedProc, 
                    poco,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public T Select<T>(string sql, object parameters = null) where T : new()
        {
            using (_dbConn)
            {
                Open();
                var o = _dbConn.Query<T>(
                    sql, 
                    parameters,
                    commandType: CommandType.StoredProcedure).SingleOrDefault();
                if (o != null)
                    return o;

                return new T();
            }
        }

        public List<T> SelectList<T>(string sql, object parameters = null)
        {
            using (_dbConn)
            {
                Open();
                return _dbConn.Query<T>(
                    sql, 
                    parameters, 
                    commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void Update(string storedProc, object poco)
        {
            using (_dbConn)
            {
                Open();
                _dbConn.Execute(
                    storedProc, 
                    poco,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public void ExecuteNonQuery(string sql)
        {
            using (_dbConn)
            {
                Open();
                using (var command = new SqlCommand(sql, _dbConn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<IEnumerable<T>> SelectListAsync<T>(string sql, object parameters = null)
        {
            using (_dbConn)
            {
                Open();

                var returnList = await _dbConn.QueryAsync<T>(
                    sql,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return returnList;
            }
        }

        #region helpers
        public void Dispose()
        {
            _dbConn.Close();
            _dbConn.Dispose();
        }
        public void Open()
        {
            // _dbConn wil be disposed so needs to be instantiated again
            _dbConn = new SqlConnection(_connectionString);

            if (_dbConn.State == ConnectionState.Closed)
                _dbConn.Open();
        }
        #endregion
    }
}
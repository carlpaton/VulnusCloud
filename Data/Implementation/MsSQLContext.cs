using Dapper;
using System.Data.SqlClient; 
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Implementation
{
    public class MsSQLContext : IBaseContext
    {
        private readonly string _connectionString = "";

        public MsSQLContext(string connectionString)
        {
            _connectionString = connectionString;
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public void Delete(string storedProc, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                connection.Execute(
                    storedProc, 
                    new { id },
                    commandType: CommandType.StoredProcedure);

                connection.Close();
            }
        }

        public int Insert(string storedProc, object poco)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var o = connection.ExecuteScalar<int>(
                    storedProc, 
                    poco,
                    commandType: CommandType.StoredProcedure);

                connection.Close();
                return o;
            }
        }

        public T Select<T>(string sql, object parameters = null) where T : new()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var o = connection.Query<T>(
                    sql, 
                    parameters,
                    commandType: CommandType.StoredProcedure).SingleOrDefault();

                connection.Close();
                if (o != null)
                    return o;

                return new T();
            }
        }

        public List<T> SelectList<T>(string sql, object parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var o = connection.Query<T>(
                    sql, 
                    parameters, 
                    commandType: CommandType.StoredProcedure).ToList();

                connection.Close();
                return o;
            }
        }

        public void Update(string storedProc, object poco)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                connection.Execute(
                    storedProc, 
                    poco,
                    commandType: CommandType.StoredProcedure);

                connection.Close();
            }
        }

        public void ExecuteNonQuery(string sql)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using (var command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public async Task<IEnumerable<T>> SelectListAsync<T>(string sql, object parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var returnList = await connection.QueryAsync<T>(
                    sql,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                connection.Close();
                return returnList;
            }
        }
    }
}
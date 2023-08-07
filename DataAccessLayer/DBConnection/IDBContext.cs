using System.Data.SqlClient;

namespace DataAccessLayer.DBConnection;

public interface IDBContext
{
    Task<bool> ExecuteNonQueryAsync(string query);
    Task<SqlDataReader> ExecuteQueryAsync(string query);
}

using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataAccessLayer.DBConnection;

public class DBContext
{
    private readonly string connectionString;

    public DBContext()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        configurationBuilder.AddJsonFile(path, false);

        var root = configurationBuilder.Build();
        connectionString = root.GetSection("ConnectionStrings").GetSection("db").Value!;
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }

    public async Task<SqlDataReader> ExecuteQueryAsync(string query)
    {
        try
        {
            SqlConnection conn = GetConnection();
            SqlCommand command = new(query, conn);
            await conn.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            return reader;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> ExecuteNonQueryAsync(string query)
    {
        using SqlConnection conn = GetConnection();
        try
        {
            SqlCommand command = new(query, conn);
            await conn.OpenAsync();
            return await command.ExecuteNonQueryAsync() > 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

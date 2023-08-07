using DataAccessLayer.DAO.Interfaces;
using DataAccessLayer.DBConnection;
using DataAccessLayer.Domain;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;

namespace DataAccessLayer.DAO.Implementations;

public class DepartmentDAO : IDepartmentDAO
{
    private IDBContext DbContext { get; }

    public DepartmentDAO(IDBContext dBContext)
    {
        DbContext = dBContext;
    }

    public async Task<List<Department>> GetAllDeps()
    {
        string query = $@"select * from Departments";
        List<Department> response = new();
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new Department
                    {
                        DepId = Convert.ToInt32(reader["DepID"]),
                        DepName = reader["DepName"].ToString()!,
                    }
                );
            }
        }
        return response;
    }

    public async Task<bool> CreateNewDep(List<Department> depList)
    {
        string query = $@"";
        if (!depList.IsNullOrEmpty())
        {
            foreach (var dep in depList)
            {
                query += $@"insert into Departments values ({dep.DepId}, {dep.DepName})";
            }
        }
        return await DbContext.ExecuteNonQueryAsync(query);
    }

    public async Task<bool> EditDep(List<Department> depList)
    {
        string query = $@"";
        if (!depList.IsNullOrEmpty())
        {
            foreach (var dep in depList)
            {
                query +=
                    $@"update Departments
                        set DepName = {dep.DepName}
                        where DepID = {dep.DepId}";
            }
        }
        return await DbContext.ExecuteNonQueryAsync(query);
    }
}

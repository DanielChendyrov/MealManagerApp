﻿using DataAccessLayer.DAO.Interfaces;
using DataAccessLayer.DBConnection;
using DataAccessLayer.Domain;
using System.Data.SqlClient;

namespace DataAccessLayer.DAO.Implementations;

public class RoleDAO : IRoleDAO
{
    private IDBContext DbContext { get; }

    public RoleDAO(IDBContext dBContext)
    {
        DbContext = dBContext;
    }

    public async Task<List<CompanyRole>> GetAllCompanyRoles()
    {
        string query = $@"select * from CompanyRoles";
        List<CompanyRole> response = new();
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new()
                    {
                        CompRoleId = Convert.ToInt32(reader["CompRoleID"]),
                        CompRoleName = reader["CompRoleName"].ToString()!,
                    }
                );
            }
        }
        return response;
    }

    public async Task<List<SystemRole>> GetAllSystemRoles()
    {
        string query = $@"select * from SystemRoles";
        List<SystemRole> response = new();
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new()
                    {
                        SysRoleId = Convert.ToInt32(reader["SysRoleID"]),
                        SysRoleName = reader["SysRoleName"].ToString()!,
                    }
                );
            }
        }
        return response;
    }

    public async Task<bool> AddRole<T>(T request)
    {
        string query = $@"";
        if (request != null)
        {
            if (request.GetType() == typeof(CompanyRole))
            {
                query +=
                    $@"insert into CompanyRoles values
                            ({(request as CompanyRole)!.CompRoleId}, '{(request as CompanyRole)!.CompRoleName}')";
            }
            else if (request.GetType() == typeof(SystemRole))
            {
                query +=
                    $@"insert into CompanyRoles values
                            ({(request as SystemRole)!.SysRoleId}, '{(request as SystemRole)!.SysRoleName}')";
            }
        }
        return await DbContext.ExecuteNonQueryAsync(query);
    }

    public async Task<bool> EditRole<T>(T request)
    {
        string query = $@"";
        if (request != null)
        {
            if (request.GetType() == typeof(CompanyRole))
            {
                query +=
                    $@"update CompanyRoles set CompRoleName = '{(request as CompanyRole)!.CompRoleName}'
                        where CompRoleID = {(request as CompanyRole)!.CompRoleId}";
            }
            else if (request.GetType() == typeof(SystemRole))
            {
                query +=
                    $@"update SystemRoles set SysRoleName = '{(request as SystemRole)!.SysRoleName}'
                        where SysRoleID = {(request as SystemRole)!.SysRoleId}";
            }
        }
        return await DbContext.ExecuteNonQueryAsync(query);
    }
}

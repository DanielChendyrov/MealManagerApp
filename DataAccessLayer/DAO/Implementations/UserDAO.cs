using DataAccessLayer.DAO.Interfaces;
using DataAccessLayer.DBConnection;
using DataAccessLayer.Domain;
using System.Data.SqlClient;

namespace DataAccessLayer.DAO.Implementations;

public class UserDAO : IUserDAO
{
    private readonly DBContext _dbContext;

    public UserDAO()
    {
        _dbContext = new DBContext();
    }

    public async Task<List<User>> GetAllUsers()
    {
        string query =
            $@"select u.UserID, u.FullName, u.Username, u.[Password], d.*, cr.*, sr.*
                from Users u 
            join Departments d on d.DepID = u.DepID
            join CompanyRoles cr on cr.CompRoleID = u.CompRoleID
            join SystemRoles sr on sr.SysRoleID = u.SysRoleID";
        List<User> response = new();
        using (SqlDataReader reader = await _dbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new User
                    {
                        UserId = Convert.ToInt32(reader["UserID"]),
                        FullName = reader["FullName"].ToString()!,
                        Username = reader["Username"].ToString()!,
                        Password = reader["Password"].ToString()!,
                        DepId = Convert.ToInt32(reader["DepID"]),
                        CompRoleId = Convert.ToInt32(reader["CompRoleID"]),
                        SysRoleId = Convert.ToInt32(reader["SysRoleID"]),
                        Dep = new()
                        {
                            DepId = Convert.ToInt32(reader["DepID"]),
                            DepName = reader["DepName"].ToString()!,
                        },
                        CompRole = new()
                        {
                            CompRoleId = Convert.ToInt32(reader["CompRoleID"]),
                            CompRoleName = reader["CompRoleName"].ToString()!,
                        },
                        SysRole = new()
                        {
                            SysRoleId = Convert.ToInt32(reader["SysRoleID"]),
                            SysRoleName = reader["SysRoleName"].ToString()!,
                        },
                    }
                );
            }
        }
        return response;
    }

    public async Task<List<User>> GetUsersByDep(int depId)
    {
        string query =
            $@"select u.UserID, u.FullName, u.Username, u.[Password], d.*, cr.*, sr.*
                from Users u 
            join Departments d on d.DepID = u.DepID
            join CompanyRoles cr on cr.CompRoleID = u.CompRoleID
            join SystemRoles sr on sr.SysRoleID = u.SysRoleID
            where d.DepID = {depId}";
        List<User> response = new();
        using (SqlDataReader reader = await _dbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new User
                    {
                        UserId = Convert.ToInt32(reader["UserID"]),
                        FullName = reader["FullName"].ToString()!,
                        Username = reader["Username"].ToString()!,
                        Password = reader["Password"].ToString()!,
                        DepId = Convert.ToInt32(reader["DepID"]),
                        CompRoleId = Convert.ToInt32(reader["CompRoleID"]),
                        SysRoleId = Convert.ToInt32(reader["SysRoleID"]),
                        Dep = new()
                        {
                            DepId = Convert.ToInt32(reader["DepID"]),
                            DepName = reader["DepName"].ToString()!,
                        },
                        CompRole = new()
                        {
                            CompRoleId = Convert.ToInt32(reader["CompRoleID"]),
                            CompRoleName = reader["CompRoleName"].ToString()!,
                        },
                        SysRole = new()
                        {
                            SysRoleId = Convert.ToInt32(reader["SysRoleID"]),
                            SysRoleName = reader["SysRoleName"].ToString()!,
                        },
                    }
                );
            }
        }
        return response;
    }

    public async Task<User> LogIn(User request)
    {
        string query =
            $@"select u.UserID, u.FullName, u.Username, u.[Password], d.*, cr.*, sr.*
                from Users u 
            join Departments d on d.DepID = u.DepID
            join CompanyRoles cr on cr.CompRoleID = u.CompRoleID
            join SystemRoles sr on sr.SysRoleID = u.SysRoleID
            where u.Username = '{request.Username}' and u.[Password] = '{request.Password}'";
        User response = new();
        using (SqlDataReader reader = await _dbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response = new()
                {
                    UserId = Convert.ToInt32(reader["UserID"]),
                    FullName = reader["FullName"].ToString()!,
                    Username = reader["Username"].ToString()!,
                    Password = reader["Password"].ToString()!,
                    DepId = Convert.ToInt32(reader["DepID"]),
                    CompRoleId = Convert.ToInt32(reader["CompRoleID"]),
                    SysRoleId = Convert.ToInt32(reader["SysRoleID"]),
                    Dep = new()
                    {
                        DepId = Convert.ToInt32(reader["DepID"]),
                        DepName = reader["DepName"].ToString()!,
                    },
                    CompRole = new()
                    {
                        CompRoleId = Convert.ToInt32(reader["CompRoleID"]),
                        CompRoleName = reader["CompRoleName"].ToString()!,
                    },
                    SysRole = new()
                    {
                        SysRoleId = Convert.ToInt32(reader["SysRoleID"]),
                        SysRoleName = reader["SysRoleName"].ToString()!,
                    }
                };
            }
        }
        return response;
    }

    public async Task<bool> SignUp(User request)
    {
        string query =
            $@"if not exists (
                select * from Users where Username = '{request.Username}'
            )
                begin
	                insert into Users values
                        ('{request.FullName}', '{request.Username}', 
                        '{request.Password}', {request.DepId},
                        {request.CompRoleId}, {request.SysRoleId})
                end";
        return await _dbContext.ExecuteNonQueryAsync(query);
    }

    public async Task<bool> EditUser(User request)
    {
        string query =
            $@"update Users
                set FullName = '{request.FullName}', DepID = {request.DepId},
                    CompRoleID = {request.CompRoleId}, SysRoleID = {request.SysRoleId}
            where UserID = {request.UserId}";
        return await _dbContext.ExecuteNonQueryAsync(query);
    }
}

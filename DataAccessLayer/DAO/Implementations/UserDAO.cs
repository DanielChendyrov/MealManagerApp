using DataAccessLayer.DAO.Interfaces;
using DataAccessLayer.DBConnection;
using DataAccessLayer.Domain;
using System.Data.SqlClient;

namespace DataAccessLayer.DAO.Implementations;

public class UserDAO : IUserDAO
{
    private IDBContext DbContext { get; }

    public UserDAO(IDBContext dBContext)
    {
        DbContext = dBContext;
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
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
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

    public async Task<User> GetUserByID(int uid)
    {
        string query =
            $@"select u.UserID, u.FullName, u.Username, u.[Password], d.*, cr.*, sr.*
                from Users u 
            join Departments d on d.DepID = u.DepID
            join CompanyRoles cr on cr.CompRoleID = u.CompRoleID
            join SystemRoles sr on sr.SysRoleID = u.SysRoleID
            where u.UserID = {uid}";
        User response = new();
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
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
                    },
                };
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
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
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
            where u.Username = '{request.Username}'";
        User response = new();
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
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
                    },
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
                        (N'{request.FullName}', '{request.Username}', 
                        '{request.Password}', {request.DepId},
                        {request.CompRoleId}, {request.SysRoleId})
                end";
        return await DbContext.ExecuteNonQueryAsync(query);
    }

    public async Task<bool> EditUser(User request)
    {
        string query =
            $@"update Users
                set FullName = N'{request.FullName}', DepID = {request.DepId},
                    CompRoleID = {request.CompRoleId}, SysRoleID = {request.SysRoleId}
            where UserID = {request.UserId}";
        return await DbContext.ExecuteNonQueryAsync(query);
    }

    public async Task<bool> ChangePassword(User request)
    {
        string query =
            $@"update Users
                set Password = '{request.Password}'
            where UserID = {request.UserId}";
        return await DbContext.ExecuteNonQueryAsync(query);
    }
}

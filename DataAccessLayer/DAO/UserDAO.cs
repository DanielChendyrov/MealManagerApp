using DataAccessLayer.DBConnection;
using DataAccessLayer.Domain;
using System.Data.SqlClient;

namespace DataAccessLayer.DAO;

public class UserDAO
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

    public async Task<User> LogIn(string username, string password)
    {
        string query =
            $@"select u.UserID, u.FullName, u.Username, u.[Password], d.*, cr.*, sr.*
                from Users u 
                join Departments d on d.DepID = u.DepID
                join CompanyRoles cr on cr.CompRoleID = u.CompRoleID
                join SystemRoles sr on sr.SysRoleID = u.SysRoleID
                where u.Username = '{username}' and u.[Password] = '{password}'";
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

    public async Task<bool> SignUp(
        string fullName,
        string username,
        string password,
        int depID,
        int compRoleID,
        int sysRoleID
    )
    {
        string query =
            $@"select * from Users where Username = ''
                if @@rowcount = 0
	                insert into Users values
                        ('{fullName}', '{username}', '{password}', {depID}, {compRoleID}, {sysRoleID})";
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

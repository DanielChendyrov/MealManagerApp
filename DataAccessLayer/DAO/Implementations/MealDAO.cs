using DataAccessLayer.DAO.Interfaces;
using DataAccessLayer.DBConnection;
using DataAccessLayer.Domain;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;

namespace DataAccessLayer.DAO.Implementations;

public class MealDAO : IMealDAO
{
    private readonly DBContext _dbContext;

    public MealDAO()
    {
        _dbContext = new();
    }

    public async Task<List<Serving>> GetPersonalMonthlyStats(int uid)
    {
        string query =
            $@"select s.ServingID, s.Quantity, s.BookedDate,
	                s.FormID, s.UserID, m.* from Servings s
                join Meals m on m.MealID = s.MealID
                where UserID = {uid} and month(s.BookedDate) = month(convert(date, sysdatetime()))
	                and year(s.BookedDate) = year(convert(date, sysdatetime()))
                order by s.BookedDate, m.MealID";
        List<Serving> response = new();
        using (SqlDataReader reader = await _dbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new Serving
                    {
                        ServingId = Convert.ToInt32(reader["ServingID"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        BookedDate = Convert.ToDateTime(reader["BookedDate"]),
                        FormId = Convert.ToInt32(reader["FormID"]),
                        UserId = Convert.ToInt32(reader["UserID"]),
                        MealId = Convert.ToInt32(reader["MealID"]),
                        Meal = new()
                        {
                            MealId = Convert.ToInt32(reader["MealID"]),
                            MealName = reader["MealName"].ToString()!,
                            Time = (TimeSpan)reader["Time"],
                        }
                    }
                );
            }
        }
        return response;
    }

    public async Task<List<Serving>> GetCompanyDailyStats(DateTime requestDate)
    {
        string query =
            $@"select u.UserID, u.FullName, d.*, m.*, s.Quantity from Servings s
                join Meals m on m.MealID = s.MealID
                join Users u on u.UserID = s.UserID
                join Departments d on d.DepID = u.DepID
                where cast(s.BookedDate as date) = '{requestDate:yyyy-MM-dd}'
                order by m.MealID, d.DepName, u.FullName";
        List<Serving> response = new();
        using (SqlDataReader reader = await _dbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new Serving
                    {
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        UserId = Convert.ToInt32(reader["UserID"]),
                        MealId = Convert.ToInt32(reader["MealID"]),
                        User = new()
                        {
                            UserId = Convert.ToInt32(reader["UserID"]),
                            FullName = reader["FullName"].ToString()!,
                            DepId = Convert.ToInt32(reader["DepID"]),
                            Dep = new()
                            {
                                DepId = Convert.ToInt32(reader["DepID"]),
                                DepName = reader["DepName"].ToString()!,
                            },
                        },
                        Meal = new()
                        {
                            MealId = Convert.ToInt32(reader["MealID"]),
                            MealName = reader["MealName"].ToString()!,
                            Time = (TimeSpan)reader["Time"],
                        },
                    }
                );
            }
        }
        return response;
    }

    public async Task<List<Serving>> GetCompanyMonthlyStats(DateTime requestDate)
    {
        string query =
            $@"select u.UserID, u.FullName, s.Quantity, s.BookedDate,
                m.*, d.* from Servings s
            join Users u on u.UserID = s.UserID
            join Departments d on d.DepID = u.DepID
            join Meals m on m.MealID = s.MealID
            where month(s.BookedDate) = month('{requestDate}')
                and year(s.BookedDate) = year('{requestDate}')
            order by d.DepName, u.FullName, m.MealID";
        List<Serving> response = new();
        using (SqlDataReader reader = await _dbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new Serving
                    {
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        UserId = Convert.ToInt32(reader["UserID"]),
                        MealId = Convert.ToInt32(reader["MealID"]),
                        User = new()
                        {
                            UserId = Convert.ToInt32(reader["UserID"]),
                            FullName = reader["FullName"].ToString()!,
                            DepId = Convert.ToInt32(reader["DepID"]),
                            Dep = new()
                            {
                                DepId = Convert.ToInt32(reader["DepID"]),
                                DepName = reader["DepName"].ToString()!,
                            },
                        },
                        Meal = new()
                        {
                            MealId = Convert.ToInt32(reader["MealID"]),
                            MealName = reader["MealName"].ToString()!,
                            Time = (TimeSpan)reader["Time"],
                        },
                    }
                );
            }
        }
        return response;
    }

    public async Task<List<Serving>> FindExistingRegistration(int depID)
    {
        string query = 
            $@"select s.ServingID, s.Quantity, s.BookedDate, m.*, u.UserID, u.DepID
	            from Servings s
            join Users u on u.UserID = s.UserID
            join Meals m on m.MealID = s.MealID
            where u.DepID = {depID} and convert(date, s.BookedDate) = convert(date, current_timestamp)";
        List<Serving> response = new();
        using (SqlDataReader reader = await _dbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(new Serving
                {
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    UserId = Convert.ToInt32(reader["UserID"]),
                    MealId = Convert.ToInt32(reader["MealID"]),
                    User = new()
                    {
                        UserId = Convert.ToInt32(reader["UserID"]),
                        FullName = reader["FullName"].ToString()!,
                        DepId = Convert.ToInt32(reader["DepID"]),
                    },
                    Meal = new()
                    {
                        MealId = Convert.ToInt32(reader["MealID"]),
                        MealName = reader["MealName"].ToString()!,
                        Time = (TimeSpan)reader["Time"],
                    },
                });
            }
        }
        return response;
    }

    public async Task<bool> RegisterMeal(Form request)
    {
        string query = $@"";
        if (!request.Servings.IsNullOrEmpty())
        {
            query +=
                $@"insert into Forms values
                    (current_timestamp, {request.UserId}, {request.DepId})
                declare @fid int
                select @fid = scope_identity()";
            foreach (var s in request.Servings)
            {
                query += 
                    $@"if exists (
                        select * from Servings
	                    where MealID = {s.MealId} and UserID = {s.UserId} and BookedDate = {s.BookedDate}
                    )
                        begin
                            insert into Servings values
                                ({s.Quantity}, '{s.BookedDate}', @fid, {s.MealId}, {s.UserId})
                        end";
            }
        }
        return await _dbContext.ExecuteNonQueryAsync(query);
    }
}

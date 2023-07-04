using DataAccessLayer.DAO.Interfaces;
using DataAccessLayer.DBConnection;
using DataAccessLayer.Domain;
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

    public async Task<List<Serving>> GetDepartmentDailyStats(DateTime bookedDate)
    {
        string query =
            $@"select u.UserID, u.FullName, d.*, m.*, s.Quantity from Servings s
                join Meals m on m.MealID = s.MealID
                join Users u on u.UserID = s.UserID
                join Departments d on d.DepID = u.DepID
                where cast(s.BookedDate as date) = '{bookedDate:yyyy-MM-dd}'
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
                        User = new()
                        {
                            UserId = Convert.ToInt32(reader["UserID"]),
                            FullName = reader["FullName"].ToString()!,
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
}

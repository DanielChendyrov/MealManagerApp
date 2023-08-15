using DataAccessLayer.DAO.Interfaces;
using DataAccessLayer.DBConnection;
using DataAccessLayer.Domain;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;

namespace DataAccessLayer.DAO.Implementations;

public class MealDAO : IMealDAO
{
    private IDBContext DbContext { get; }

    public MealDAO(IDBContext dBContext)
    {
        DbContext = dBContext;
    }

    public async Task<List<Meal>> GetAllMeals()
    {
        string query = $@"select * from Meals";
        List<Meal> response = new();
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new Meal
                    {
                        MealId = Convert.ToInt32(reader["MealID"]),
                        MealName = reader["MealName"].ToString()!,
                        Time = (TimeSpan)reader["Time"],
                    }
                );
            }
        }
        return response;
    }

    public async Task<List<Serving>> GetPersonalMonthlyStats(int uid)
    {
        string query =
            $@"select s.ServingID, s.Quantity, s.BookedDate,
	            s.FormID, s.UserID, m.*
            from Servings s
                join Meals m on m.MealID = s.MealID
            where UserID = {uid} and month(s.BookedDate) = month(convert(date, sysdatetime()))
	            and year(s.BookedDate) = year(convert(date, sysdatetime()))
            order by s.BookedDate, m.MealID";
        List<Serving> response = new();
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
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
            $@"select u.UserID, u.FullName, d.*, m.*, s.Quantity
            from Servings s
                join Meals m on m.MealID = s.MealID
                join Users u on u.UserID = s.UserID
                join Departments d on d.DepID = u.DepID
            where cast(s.BookedDate as date) = '{requestDate:yyyy-MM-dd}'
            order by m.MealID, d.DepName, u.FullName";
        List<Serving> response = new();
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
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
            $@"select u.UserID, u.FullName,
                s.Quantity, s.BookedDate, m.*, d.* 
            from Servings s
                join Users u on u.UserID = s.UserID
                join Departments d on d.DepID = u.DepID
                join Meals m on m.MealID = s.MealID
            where month(s.BookedDate) = month('{requestDate:yyyy-MM-dd}')
                and year(s.BookedDate) = year('{requestDate:yyyy-MM-dd}')
            order by d.DepName, u.FullName, m.MealID";
        List<Serving> response = new();
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new Serving
                    {
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        UserId = Convert.ToInt32(reader["UserID"]),
                        MealId = Convert.ToInt32(reader["MealID"]),
                        BookedDate = Convert.ToDateTime(reader["BookedDate"]),
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
            $@"select s.ServingID, s.Quantity, s.BookedDate, m.*, u.UserID, u.FullName, u.DepID
	        from Servings s
                join Users u on u.UserID = s.UserID
                join Meals m on m.MealID = s.MealID
            where u.DepID = {depID}";
        List<Serving> response = new();
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new Serving
                    {
                        ServingId = Convert.ToInt32(reader["ServingID"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        BookedDate = Convert.ToDateTime(reader["BookedDate"]),
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
                    }
                );
            }
        }
        return response;
    }

    public async Task<List<Serving>> GetAll3rdShiftMeals(DateTime bookedDate, int depID)
    {
        string query =
            $@"select s.*, u.UserID, u.FullName, u.DepID, u.IsDeleted
            from Servings s
                join Users u on s.UserID = u.UserID
            where u.DepID = {depID}
                and s.MealID = 3
                and s.BookedDate = '{bookedDate:yyyy-MM-dd}'
                and u.IsDeleted = 0";
        List<Serving> response = new();
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new()
                    {
                        ServingId = Convert.ToInt32(reader["ServingID"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        BookedDate = Convert.ToDateTime(reader["BookedDate"]),
                        UserId = Convert.ToInt32(reader["UserID"]),
                        MealId = Convert.ToInt32(reader["MealID"]),
                        User = new()
                        {
                            UserId = Convert.ToInt32(reader["UserID"]),
                            FullName = reader["FullName"].ToString()!,
                            DepId = Convert.ToInt32(reader["DepID"]),
                        },
                    }
                );
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
                select @fid = scope_identity()
                ";
            foreach (var s in request.Servings)
            {
                query +=
                    $@"if not exists (
                        select * from Servings
	                    where MealID = {s.MealId} and UserID = {s.UserId} and convert(varchar, BookedDate, 105) = '{s.BookedDate}'
                    )
                        begin
                            insert into Servings values
                                ({s.Quantity}, convert(datetime, '{s.BookedDate:dd-MM-yyyy}', 105), @fid, {s.MealId}, {s.UserId})
                        end
                    ";
            }
        }
        return await DbContext.ExecuteNonQueryAsync(query);
    }

    public async Task<List<Serving>> GetAllPersonalOrders(int uid)
    {
        string query =
            $@"select s.ServingID, s.Quantity, s.BookedDate, s.UserID, m.* from Servings s
            join Meals m on m.MealID = s.MealID
            where s.UserID = {uid} and s.BookedDate >= convert(date, sysdatetime())";
        List<Serving> response = new();
        using (SqlDataReader reader = await DbContext.ExecuteQueryAsync(query))
        {
            while (reader.Read())
            {
                response.Add(
                    new Serving
                    {
                        ServingId = Convert.ToInt32(reader["ServingID"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        BookedDate = Convert.ToDateTime(reader["BookedDate"]),
                        UserId = Convert.ToInt32(reader["UserID"]),
                        MealId = Convert.ToInt32(reader["MealID"]),
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

    public async Task<bool> EditMeal(List<Serving> request)
    {
        string query = $@"";
        foreach (var s in request)
        {
            query +=
                $@"update Servings
                    set Quantity = {s.Quantity}
                from Servings s, Meals m
                where s.MealID = m.MealID
                    and ServingID = {s.ServingId}
                    and s.BookedDate + convert(datetime, m.[Time]) > sysdatetime()";
        }
        return await DbContext.ExecuteNonQueryAsync(query);
    }

    public async Task<bool> EditMeal3rdShift(List<Serving> request)
    {
        string query = $@"declare @sum int";
        var dateList = request.DistinctBy(s => s.BookedDate).ToList();
        foreach (var d in dateList)
        {
            var tmp = request.Where(s => s.BookedDate == d.BookedDate).ToList();
            var quantitySum = tmp.Select(s => s.Quantity).Sum();
            query +=
                $@"select @sum = 
                    (select sum(Quantity) from Servings
                    where MealID = 3 and BookedDate = {d})";
            foreach (var s in tmp)
            {
                query +=
                    $@"if @sum = {quantitySum}
                        begin
                            update Servings
                                set Quantity = {s.Quantity}
                            from Servings s, Meals m
                            where s.MealID = m.MealID
                                and ServingID = {s.ServingId}
                                and s.BookedDate + convert(datetime, m.[Time]) > sysdatetime()
                        end";
            }
        }
        return await DbContext.ExecuteNonQueryAsync(query);
    }

    public async Task<bool> DeleteMeal(int servingID)
    {
        string query =
            $@"delete Servings
            from Servings s join Meals m
                on s.MealID = m.MealID
            where s.ServingID = {servingID}
                and s.BookedDate + convert(datetime, m.[Time]) > sysdatetime()";
        return await DbContext.ExecuteNonQueryAsync(query);
    }
}

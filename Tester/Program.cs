using DataAccessLayer.DAO;
using DataAccessLayer.Domain;

MealDAO dao = new();
List<Serving> list = await dao.GetPersonalMonthlyStats(1);
Console.WriteLine(list.Count);

using DataAccessLayer.DAO.Implementations;
using DataAccessLayer.Domain;

internal class Program
{
    private static async Task Main(string[] args)
    {
        UserDAO dao = new();
        List<User> list = await dao.GetAllUsers();
        Console.WriteLine(list.Count);
    }
}
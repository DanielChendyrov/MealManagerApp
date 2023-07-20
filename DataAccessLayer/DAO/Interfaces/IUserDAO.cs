using DataAccessLayer.Domain;

namespace DataAccessLayer.DAO.Interfaces;

public interface IUserDAO
{
    Task<List<User>> GetAllUsers();
    Task<List<User>> GetUsersByDep(int depId);
    Task<User> LogIn(User request);
    Task<bool> SignUp(User request);
    Task<bool> EditUser(User request);
    Task<User> GetUserByID(int uid);
}

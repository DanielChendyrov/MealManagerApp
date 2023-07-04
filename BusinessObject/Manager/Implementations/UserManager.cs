using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.Request;
using BusinessObject.Manager.Interfaces;
using DataAccessLayer.DAO.Implementations;
using DataAccessLayer.DAO.Interfaces;
using DataAccessLayer.Domain;

namespace BusinessObject.Manager.Implementations;

public class UserManager : IUserManager
{
    private IUserDAO UserDAO { get; }
    private IMapper Mapper { get; }

    public UserManager(IUserDAO userDAO, IMapper mapper)
    {
        UserDAO = userDAO;
        Mapper = mapper;
    }

    public async Task<bool> EditUser(UserDTO request)
    {
        var response = await UserDAO.EditUser(Mapper.Map<User>(request));
        return response;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
        var response = await UserDAO.GetAllUsers();
        return Mapper.Map<IEnumerable<UserDTO>>(response);
    }

    public async Task<IEnumerable<UserDTO>> GetUsersByDep(int depID)
    {
        var response = await UserDAO.GetUsersByDep(depID);
        return Mapper.Map<IEnumerable<UserDTO>>(response);
    }

    public async Task<UserDTO> LogIn(LogInDTO request)
    {
        var response = await UserDAO.LogIn(Mapper.Map<User>(request));
        return Mapper.Map<UserDTO>(response);
    }

    public async Task<UserDTO> SignUp(SignUpDTO request)
    {
        await UserDAO.SignUp(Mapper.Map<User>(request));
        LogInDTO forward = new() { Username = request.Username, Password = request.Password };
        return await LogIn(forward);
    }
}

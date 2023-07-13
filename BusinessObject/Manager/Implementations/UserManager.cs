using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.Request;
using BusinessObject.Manager.Interfaces;
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
        return await UserDAO.EditUser(Mapper.Map<User>(request));
    }

    public async Task<List<UserDTO>> GetAllUsers()
    {
        return Mapper.Map<List<UserDTO>>(await UserDAO.GetAllUsers());
    }

    public async Task<List<UserDTO>> GetUsersByDep(int depID)
    {
        return Mapper.Map<List<UserDTO>>(await UserDAO.GetUsersByDep(depID));
    }

    public async Task<UserDTO> LogIn(LogInDTO request)
    {
        return Mapper.Map<UserDTO>(await UserDAO.LogIn(Mapper.Map<User>(request)));
    }

    public async Task<UserDTO> SignUp(SignUpDTO request)
    {
        if (await UserDAO.SignUp(Mapper.Map<User>(request)))
        {
            return await LogIn(new() { Username = request.Username, Password = request.Password });
        }
        return new();
    }
}

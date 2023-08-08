using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.Request;
using BusinessObject.Manager.Interfaces;
using ClientApp.Utils.Implementations;
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

    public async Task<List<UserDTO>> GetAllUsers()
    {
        return Mapper.Map<List<UserDTO>>(await UserDAO.GetAllUsers());
    }

    public async Task<UserDTO> GetUserByID(int uid)
    {
        return Mapper.Map<UserDTO>(await UserDAO.GetUserByID(uid));
    }

    public async Task<List<UserDTO>> GetUsersByDep(int depID)
    {
        return Mapper.Map<List<UserDTO>>(await UserDAO.GetUsersByDep(depID));
    }

    public async Task<UserDTO> LogIn(LogInDTO request)
    {
        var response = await UserDAO.LogIn(Mapper.Map<User>(request));
        if (StringHasher.VerifyHash(request.Password!, "SHA512", response.Password))
        {
            return Mapper.Map<UserDTO>(response);
        }
        return new();
    }

    public async Task<UserDTO> SignUp(SignUpDTO request)
    {
        SignUpDTO encrypt = new()
        {
            Username = request.Username,
            Password = StringHasher.ComputeHash(request.Password, "SHA512", null!),
            FullName = request.FullName,
            DepID = request.DepID,
            CompRoleID = request.CompRoleID,
            SysRoleID = request.SysRoleID,
        };
        if (await UserDAO.SignUp(Mapper.Map<User>(encrypt)))
        {
            return await LogIn(new() { Username = request.Username, Password = request.Password });
        }
        return new();
    }

    public async Task<bool> EditUsers(List<UserDTO> requests)
    {
        return await UserDAO.EditUsers(Mapper.Map<List<User>>(requests));
    }

    public async Task<bool> ChangePassword(ChangePasswordDTO request)
    {
        var response = await UserDAO.GetUserByID(request.UserID);
        if (StringHasher.VerifyHash(request.OldPassword!, "SHA512", response.Password))
        {
            request.NewPassword = StringHasher.ComputeHash(request.NewPassword!, "SHA512", null!);
            return await UserDAO.ChangePassword(Mapper.Map<User>(request));
        }
        return false;
    }

    public async Task<bool> DeleteUser(int userID)
    {
        return await UserDAO.DeleteUser(userID);
    }
}

using BusinessObject.DTO;
using BusinessObject.DTO.Request;
using BusinessObject.DTO.Response;

namespace BusinessObject.Manager.Interfaces
{
    public interface IUserManager
    {
        Task<List<UserDTO>> GetAllUsers();
        Task<List<UserDTO>> GetUsersByDep(int depID);
        Task<UserDTO> GetUserByID(int uid);
        Task<UserDTO> LogIn(LogInDTO request);
        Task<UserDTO> SignUp(SignUpDTO request);
        Task<bool> ChangePassword(ChangePasswordDTO request);
        Task<bool> EditUser(UserDTO request);
    }
}

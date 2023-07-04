using BusinessObject.DTO;
using BusinessObject.DTO.Request;

namespace BusinessObject.Manager.Interfaces
{
    public interface IUserManager
    {
        Task<UserDTO> LogIn(LogInDTO request);
        Task<UserDTO> SignUp(SignUpDTO request);
        Task<bool> EditUser(UserDTO request);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<IEnumerable<UserDTO>> GetUsersByDep(int depID);
    }
}

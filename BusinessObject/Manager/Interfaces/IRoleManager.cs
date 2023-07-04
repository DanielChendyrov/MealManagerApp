using BusinessObject.DTO;
using DataAccessLayer.Domain;

namespace BusinessObject.Manager.Interfaces;

public interface IRoleManager
{
    Task<List<CompanyRoleDTO>> GetAllCompanyRoles();
    Task<List<SystemRoleDTO>> GetAllSystemRoles();
    Task<bool> AddRole<T>(T request);
    Task<bool> EditRole<T>(T request);
}

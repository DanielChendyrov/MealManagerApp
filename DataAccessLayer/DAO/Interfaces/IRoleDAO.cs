using DataAccessLayer.Domain;

namespace DataAccessLayer.DAO.Interfaces;

public interface IRoleDAO
{
    Task<List<CompanyRole>> GetAllCompanyRoles();
    Task<List<SystemRole>> GetAllSystemRoles();
    Task<bool> AddRole<T>(T request);
    Task<bool> EditRole<T>(T request);
}

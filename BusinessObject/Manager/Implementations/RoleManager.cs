using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Manager.Interfaces;
using DataAccessLayer.DAO.Interfaces;
using DataAccessLayer.Domain;

namespace BusinessObject.Manager.Implementations;

public class RoleManager : IRoleManager
{
    private IRoleDAO RoleDAO { get; }
    private IMapper Mapper { get; }

    public RoleManager(IRoleDAO roleDAO, IMapper mapper)
    {
        RoleDAO = roleDAO;
        Mapper = mapper;
    }

    public async Task<bool> AddRole<T>(T request)
    {
        if (request != null)
        {
            if (request.GetType() == typeof(CompanyRoleDTO))
            {
                return await RoleDAO.AddRole(Mapper.Map<CompanyRole>(request));
            }
            else if (request.GetType() == typeof(SystemRoleDTO))
            {
                return await RoleDAO.AddRole(Mapper.Map<SystemRole>(request));
            }
        }
        return false;
    }

    public async Task<bool> EditRole<T>(T request)
    {
        if (request != null)
        {
            if (request.GetType() == typeof(CompanyRoleDTO))
            {
                return await RoleDAO.EditRole(Mapper.Map<CompanyRole>(request));
            }
            else if (request.GetType() == typeof(SystemRoleDTO))
            {
                return await RoleDAO.EditRole(Mapper.Map<SystemRole>(request));
            }
        }
        return false;
    }

    public async Task<List<CompanyRoleDTO>> GetAllCompanyRoles()
    {
        return Mapper.Map<List<CompanyRoleDTO>>(await RoleDAO.GetAllCompanyRoles());
    }

    public async Task<List<SystemRoleDTO>> GetAllSystemRoles()
    {
        return Mapper.Map<List<SystemRoleDTO>>(await RoleDAO.GetAllSystemRoles());
    }
}

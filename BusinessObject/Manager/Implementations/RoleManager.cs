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
                var response = await RoleDAO.AddRole(Mapper.Map<CompanyRole>(request));
                return response;
            }
            else if (request.GetType() == typeof(SystemRoleDTO))
            {
                var response = await RoleDAO.AddRole(Mapper.Map<SystemRole>(request));
                return response;
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
                var response = await RoleDAO.EditRole(Mapper.Map<CompanyRole>(request));
                return response;
            }
            else if (request.GetType() == typeof(SystemRoleDTO))
            {
                var response = await RoleDAO.EditRole(Mapper.Map<SystemRole>(request));
                return response;
            }
        }
        return false;
    }

    public async Task<List<CompanyRoleDTO>> GetAllCompanyRoles()
    {
        var response = await RoleDAO.GetAllCompanyRoles();
        return Mapper.Map<List<CompanyRoleDTO>>(response);
    }

    public async Task<List<SystemRoleDTO>> GetAllSystemRoles()
    {
        var response = await RoleDAO.GetAllSystemRoles();
        return Mapper.Map<List<SystemRoleDTO>>(response);
    }
}

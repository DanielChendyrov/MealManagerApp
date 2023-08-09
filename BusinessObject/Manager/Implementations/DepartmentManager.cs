using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Manager.Interfaces;
using DataAccessLayer.DAO.Interfaces;
using DataAccessLayer.Domain;

namespace BusinessObject.Manager.Implementations;

public class DepartmentManager : IDepartmentManager
{
    private IDepartmentDAO DepartmentDAO { get; }
    private IMapper Mapper { get; }

    public DepartmentManager(IDepartmentDAO departmentDAO, IMapper mapper)
    {
        DepartmentDAO = departmentDAO;
        Mapper = mapper;
    }

    public async Task<List<DepartmentDTO>> GetAllDeps()
    {
        return Mapper.Map<List<DepartmentDTO>>(await DepartmentDAO.GetAllDeps());
    }

    public async Task<bool> CreateNewDep(List<DepartmentDTO> request)
    {
        return await DepartmentDAO.CreateNewDep(Mapper.Map<List<Department>>(request));
    }

    public async Task<bool> EditDep(List<DepartmentDTO> request)
    {
        return await DepartmentDAO.EditDep(Mapper.Map<List<Department>>(request));
    }

    public async Task<bool> DeleteDep(int depID)
    {
        return await DepartmentDAO.DeleteDep(depID);
    }
}

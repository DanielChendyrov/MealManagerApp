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

    public async Task<bool> CreateNewDep(List<DepartmentDTO> request)
    {
        var response = await DepartmentDAO.CreateNewDep(Mapper.Map<List<Department>>(request));
        return response;
    }

    public async Task<bool> EditDep(List<DepartmentDTO> depList)
    {
        var response = await DepartmentDAO.EditDep(Mapper.Map<List<Department>>(depList));
        return response;
    }

    public async Task<List<DepartmentDTO>> GetAllDeps()
    {
        var response = await DepartmentDAO.GetAllDeps();
        return Mapper.Map<List<DepartmentDTO>>(response);
    }
}

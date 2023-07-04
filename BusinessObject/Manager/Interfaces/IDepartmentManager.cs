using BusinessObject.DTO;

namespace BusinessObject.Manager.Interfaces;

public interface IDepartmentManager
{
    Task<List<DepartmentDTO>> GetAllDeps();
    Task<bool> CreateNewDep(List<DepartmentDTO> request);
    Task<bool> EditDep(List<DepartmentDTO> depList);
}

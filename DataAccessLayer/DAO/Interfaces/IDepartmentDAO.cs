using DataAccessLayer.Domain;

namespace DataAccessLayer.DAO.Interfaces;

public interface IDepartmentDAO
{
    Task<List<Department>> GetAllDeps();
    Task<bool> CreateNewDep(List<Department> depList);
    Task<bool> EditDep(List<Department> depList);
}

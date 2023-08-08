using ClientApp.Models.Transfer;

namespace ClientApp.Models.Binding;

public class UserManagerModel
{
    public Dictionary<string, string> SortOrders { get; private set; } =
        new() { { "Phòng ban", "Department" }, { "Họ tên", "FullName" } };
    public string? SortOrder { get; set; }
    public List<UserModel> Users { get; set; } = new();
    public List<DepartmentModel> Departments { get; set; } = new();
    public List<CompRoleModel> CompRoles { get; set; } = new();
    public List<SysRoleModel> SysRoles { get; set; } = new();
}

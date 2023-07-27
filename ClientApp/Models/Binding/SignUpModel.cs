using ClientApp.Models.Transfer;

namespace ClientApp.Models.Binding;

public class SignUpModel
{
    public string? FullName { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public int DepID { get; set; }
    public List<DepartmentModel> Departments { get; set; } = new();
    public int CompRoleID { get; set; }
    public List<CompRoleModel> CompRoles { get; set; } = new();
    public int SysRoleID { get; set; }
    public List<SysRoleModel> SysRoles { get; set; } = new();
}

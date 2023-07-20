namespace ClientApp.Models;

public class UserModel
{
    public int UserID { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public int DepID { get; set; }
    public DepartmentModel? Dep { get; set; }
    public int CompRoleID { get; set; }
    public CompRoleModel? CompRole { get; set; }
    public int SysRoleID { get; set; }
    public SysRoleModel? SysRole { get; set; }
    public string? Jwt { get; set; }
}

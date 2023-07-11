namespace ClientApp.Models;

public class UserModel
{
    public int UserID { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public DepartmentModel Dep { get; set; }
    public CompRoleModel CompRole { get; set; }
    public SysRoleModel SysRole { get; set; }
    public string Jwt { get; set; }
}

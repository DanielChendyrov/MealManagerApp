namespace ClientApp.Models.Request;

public class SignUpModel
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public DepartmentModel Dep { get; set; }
    public CompRoleModel CompRole { get; set; }
    public SysRoleModel SysRole { get; set; }
}

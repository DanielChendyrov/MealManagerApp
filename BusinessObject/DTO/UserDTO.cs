namespace BusinessObject.DTO;

public class UserDTO
{
    public int UserID { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public int DepID { get; set; }
    public virtual DepartmentDTO? Dep { get; set; }
    public int CompRoleID { get; set; }
    public virtual CompanyRoleDTO? CompRole { get; set; }
    public int SysRoleID { get; set; }
    public virtual SystemRoleDTO? SysRole { get; set; }
    public string? Jwt { get; set; }
}

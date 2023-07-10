namespace BusinessObject.DTO
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public int DepID { get; set; }
        public virtual DepartmentDTO Dep { get; set; } = null!;
        public int CompRoleID { get; set; }
        public virtual CompanyRoleDTO CompRole { get; set; } = null!;
        public int SysRoleID { get; set; }
        public virtual SystemRoleDTO SysRole { get; set; } = null!;
        public string Jwt { get; set; }
    }
}

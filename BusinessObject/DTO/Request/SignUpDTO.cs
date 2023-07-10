namespace BusinessObject.DTO.Request
{
    public class SignUpDTO
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int DepID { get; set; }
        public virtual DepartmentDTO Dep { get; set; } = null!;
        public int CompRoleID { get; set; }
        public virtual CompanyRoleDTO CompRole { get; set; } = null!;
        public int SysRoleID { get; set; }
        public virtual SystemRoleDTO SysRole { get; set; } = null!;
    }
}

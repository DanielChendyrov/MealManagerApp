namespace DataAccessLayer.Domain;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int DepId { get; set; }

    public int CompRoleId { get; set; }

    public int SysRoleId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual CompanyRole CompRole { get; set; } = null!;

    public virtual Department Dep { get; set; } = null!;

    public virtual ICollection<Form> Forms { get; set; } = new List<Form>();

    public virtual ICollection<Serving> Servings { get; set; } = new List<Serving>();

    public virtual SystemRole SysRole { get; set; } = null!;
}

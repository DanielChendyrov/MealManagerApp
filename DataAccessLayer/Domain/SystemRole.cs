using System;
using System.Collections.Generic;

namespace DataAccessLayer.Domain;

public partial class SystemRole
{
    public int SysRoleId { get; set; }

    public string SysRoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

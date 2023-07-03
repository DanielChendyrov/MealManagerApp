using System;
using System.Collections.Generic;

namespace DataAccessLayer.Domain;

public partial class CompanyRole
{
    public int CompRoleId { get; set; }

    public string CompRoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

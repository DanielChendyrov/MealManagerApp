using System;
using System.Collections.Generic;

namespace DataAccessLayer.Domain;

public partial class Department
{
    public int DepId { get; set; }

    public string DepName { get; set; } = null!;

    public virtual ICollection<Form> Forms { get; set; } = new List<Form>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

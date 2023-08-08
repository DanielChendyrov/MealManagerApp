using System;
using System.Collections.Generic;

namespace DataAccessLayer.Domain;

public partial class Form
{
    public int FormId { get; set; }

    public DateTime RegisteredDate { get; set; }

    public int UserId { get; set; }

    public int DepId { get; set; }

    public virtual Department Dep { get; set; } = null!;

    public virtual ICollection<Serving> Servings { get; set; } = new List<Serving>();

    public virtual User User { get; set; } = null!;
}

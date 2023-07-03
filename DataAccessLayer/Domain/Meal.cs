using System;
using System.Collections.Generic;

namespace DataAccessLayer.Domain;

public partial class Meal
{
    public int MealId { get; set; }

    public string MealName { get; set; } = null!;

    public TimeSpan Time { get; set; }

    public virtual ICollection<Serving> Servings { get; set; } = new List<Serving>();
}

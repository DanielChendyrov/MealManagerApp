using System;
using System.Collections.Generic;

namespace DataAccessLayer.Domain;

public partial class Serving
{
    public int ServingId { get; set; }

    public int Quantity { get; set; }

    public DateTime BookedDate { get; set; }

    public int FormId { get; set; }

    public int MealId { get; set; }

    public int UserId { get; set; }

    public virtual Form Form { get; set; } = null!;

    public virtual Meal Meal { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

namespace BusinessObject.DTO.Request;

public class FormDTO
{
    public int UserID { get; set; }
    public virtual UserDTO? User { get; set; }
    public int DepID { get; set; }
    public virtual DepartmentDTO? Dep { get; set; }
    public List<ServingDTO> Servings { get; set; } = new();
}

public class ServingDTO
{
    public int Quantity { get; set; }
    public DateTime BookedDate { get; set; }
    public int MealID { get; set; }
    public virtual MealDTO? Meal { get; set; }
    public int UserID { get; set; }
    public virtual UserDTO? User { get; set; }
}

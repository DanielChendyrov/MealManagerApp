namespace ClientApp.Models.Request;

public class FormModel
{
    public int UserID { get; set; }
    public UserModel? User { get; set; }
    public int DepID { get; set; }
    public DepartmentModel? Dep { get; set; }
    public List<ServingModel> Servings { get; set; } = new();
}

public class ServingModel
{
    public int Quantity { get; set; }
    public DateTime BookedDate { get; set; }
    public int MealID { get; set; }
    public MealModel? Meal { get; set; }
    public int UserID { get; set; }
    public UserModel? User { get; set; }
}

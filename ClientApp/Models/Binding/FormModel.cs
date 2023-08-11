using ClientApp.Models.Transfer;

namespace ClientApp.Models.Binding;

public class FormModel
{
    public int UserID { get; set; }
    public UserModel? User { get; set; }
    public int DepID { get; set; }
    public DepartmentModel? Dep { get; set; }
    public DateTime? BookedDate { get; set; }
    public List<MealModel> Meals { get; set; } = new();
    public List<UserModel> Users { get; set; } = new();
    public List<ServingModel> Servings { get; set; } = new();
}

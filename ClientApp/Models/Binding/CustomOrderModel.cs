using ClientApp.Models.Transfer;

namespace ClientApp.Models.Binding;

public class CustomOrderModel
{
    public DateTime BookedDate { get; set; }
    public List<ServingModel> Servings { get; set; } = new();
    public List<MealModel> Meals { get; set; } = new();
}

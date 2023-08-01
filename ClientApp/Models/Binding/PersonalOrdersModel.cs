using ClientApp.Models.Transfer;

namespace ClientApp.Models.Binding;

public class PersonalOrdersModel
{
    public List<MealModel> Meals { get; set; } = new();
    public List<CustomOrder> Orders { get; set; } = new();
}

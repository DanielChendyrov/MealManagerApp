using ClientApp.Models.Transfer;

namespace ClientApp.Models.Binding;

public class PersonalStatsModel
{
    public List<PersonalMonthlyStats> Statistics { get; set; } = new();
    public List<MealModel> Meals { get; set; } = new();
    public List<int> Totals { get; set; } = new();
}

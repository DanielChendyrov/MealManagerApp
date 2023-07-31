using ClientApp.Models.Transfer;

namespace ClientApp.Models.Binding;

public class CompanyDailyStatsModel
{
    public string? ChosenDate { get; set; }
    public List<MealModel> Meals { get; set; } = new();
    public List<DepartmentModel> Departments { get; set; } = new();
    public List<CompanyDailyStats> Statistics { get; set; } = new();
}

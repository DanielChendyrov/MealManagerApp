using ClientApp.Models.Transfer;

namespace ClientApp.Models.Binding;

public class CompanyMonthlyStatsModel
{
    public List<DepartmentModel> Departments { get; set; } = new();
    public List<MealModel> Meals { get; set; } = new();
    public List<CompanyMonthlyStats> Statistics { get; set; } = new();
    public string? ChosenDate { get; set; }
}

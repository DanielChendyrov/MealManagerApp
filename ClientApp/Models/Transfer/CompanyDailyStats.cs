namespace ClientApp.Models.Transfer;

public class CompanyDailyStats
{
    public int MealID { get; set; }
    public string? MealName { get; set; }
    public List<ServingModel> Servings { get; set; } = new();
    public int Total { get; set; }
}

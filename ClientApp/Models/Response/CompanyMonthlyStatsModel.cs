namespace ClientApp.Models.Response;

public class CompanyMonthlyStatsModel
{
    public int UserID { get; set; }
    public UserModel? User { get; set; }
    public List<CustomMealStatsModel> MealStats { get; set; } = new();
    public int Total { get; set; }
}

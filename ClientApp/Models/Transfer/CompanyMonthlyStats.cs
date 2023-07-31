namespace ClientApp.Models.Transfer;

public class CompanyMonthlyStats
{
    public int UserID { get; set; }
    public UserModel? User { get; set; }
    public List<CustomMealStatsModel> MealStats { get; set; } = new();
    public int Total { get; set; }
}

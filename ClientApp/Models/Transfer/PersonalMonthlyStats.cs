namespace ClientApp.Models.Transfer;

public class PersonalMonthlyStats
{
    public int UserID { get; set; }
    public DateTime BookedDate { get; set; }
    public List<CustomMealStatsModel> MealStats { get; set; } = new();
}

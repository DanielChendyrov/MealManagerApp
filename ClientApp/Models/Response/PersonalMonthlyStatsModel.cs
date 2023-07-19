namespace ClientApp.Models.Response;

public class PersonalMonthlyStatsModel
{
    public int UserID { get; set; }
    public DateTime BookedDate { get; set; }
    public List<CustomMealStatsModel> MealStats { get; set; } = new();
}

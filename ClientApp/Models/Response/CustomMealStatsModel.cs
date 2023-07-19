namespace ClientApp.Models.Response;

public class CustomMealStatsModel
{
    public int MealID { get; set; }
    public virtual MealModel? Meal { get; set; }
    public int TotalServing { get; set; }
}

namespace ClientApp.Models.Transfer;

public class CustomMealStatsModel
{
    public int MealID { get; set; }
    public virtual MealModel? Meal { get; set; }
    public int TotalServing { get; set; }
}

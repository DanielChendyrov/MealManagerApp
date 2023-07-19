namespace BusinessObject.DTO.Response;

public class CustomMealStatsDTO
{
    public int MealID { get; set; }
    public virtual MealDTO? Meal { get; set; }
    public int TotalServing { get; set; }
}
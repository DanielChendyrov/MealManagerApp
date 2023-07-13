namespace BusinessObject.DTO.Response;

public class CompanyMonthlyStatsDTO
{
    public int UserID { get; set; }
    public virtual UserDTO? User { get; set; }
    public List<CustomMealStatsDTO> MealStats { get; set; } = new();
}

public class CustomMealStatsDTO
{
    public int MealID { get; set; }
    public virtual MealDTO? Meal { get; set; }
    public int TotalServing { get; set; }
}
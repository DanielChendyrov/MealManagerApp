namespace BusinessObject.DTO.Response;

public class CompanyMonthlyStatsDTO
{
    public int UserID { get; set; }
    public virtual UserDTO? User { get; set; }
    public List<CustomMealStatsDTO> MealStats { get; set; } = new();
    public int Total { get; set; }
}

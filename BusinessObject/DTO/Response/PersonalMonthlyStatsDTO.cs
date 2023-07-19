namespace BusinessObject.DTO.Response;

public class PersonalMonthlyStatsDTO
{
    public int UserID { get; set; }
    public DateTime BookedDate { get; set; }
    public List<CustomMealStatsDTO> MealStats { get; set; } = new();
}

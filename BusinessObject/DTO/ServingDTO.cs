namespace BusinessObject.DTO;

public class ServingDTO
{
    public int ServingID { get; set; }
    public int Quantity { get; set; }
    public DateTime BookedDate { get; set; }
    public int MealID { get; set; }
    public virtual MealDTO? Meal { get; set; }
    public int UserID { get; set; }
    public virtual UserDTO? User { get; set; }
}

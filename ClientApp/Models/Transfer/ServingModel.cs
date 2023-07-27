namespace ClientApp.Models.Transfer;

public class ServingModel
{
    public int ServingID { get; set; }
    public int Quantity { get; set; }
    public DateTime BookedDate { get; set; }
    public int MealID { get; set; }
    public MealModel? Meal { get; set; }
    public int UserID { get; set; }
    public UserModel? User { get; set; }
}

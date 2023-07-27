namespace ClientApp.Models.Transfer;

public class CustomOrderModel
{
    public DateTime BookedDate { get; set; }
    public List<ServingModel> Servings { get; set; } = new();
}

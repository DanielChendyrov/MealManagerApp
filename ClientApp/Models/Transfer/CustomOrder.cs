namespace ClientApp.Models.Transfer;

public class CustomOrder
{
    public DateTime BookedDate { get; set; }
    public List<ServingModel> Servings { get; set; } = new();
}

namespace ClientApp.Models.Request;

public class ChangePasswordModel
{
    public int UserID { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}

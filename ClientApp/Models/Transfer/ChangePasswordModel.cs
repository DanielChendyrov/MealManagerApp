namespace ClientApp.Models.Transfer;

public class ChangePasswordModel
{
    public int UserID { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}

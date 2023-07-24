namespace BusinessObject.DTO.Request;

public class ChangePasswordDTO
{
    public int UserID { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}

namespace ClientApp.Models.Request;

public class SignUpModel
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int DepID { get; set; }
    public int CompRoleID { get; set; }
    public int SysRoleID { get; set; }
}

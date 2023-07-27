namespace BusinessObject.DTO.Request;

public class FormDTO
{
    public int UserID { get; set; }
    public virtual UserDTO? User { get; set; }
    public int DepID { get; set; }
    public virtual DepartmentDTO? Dep { get; set; }
    public List<ServingDTO> Servings { get; set; } = new();
}

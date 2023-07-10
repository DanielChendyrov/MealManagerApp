namespace BusinessObject.DTO.Request
{
    public class FormDTO
    {
        public int UserID { get; set; }
        public virtual UserDTO User { get; set; } = null!;
        public int DepID { get; set; }
        public virtual DepartmentDTO Dep { get; set; } = null!;
        public DateTime RegisteredDate { get; set; }
        public List<ServingDTO> Servings { get; set; } = new();
    }
}

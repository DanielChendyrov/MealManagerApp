namespace BusinessObject.DTO.Request
{
    public class FormDTO
    {
        public int UserID { get; set; }
        public int DepID { get; set; }
        public DateTime RegisteredDate { get; set; }
        public List<ServingDTO> Servings { get; set; } = new();
    }
}

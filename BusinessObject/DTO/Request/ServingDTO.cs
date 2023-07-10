namespace BusinessObject.DTO.Request
{
    public class ServingDTO
    {
        public int Quantity { get; set; }
        public DateTime BookedDate { get; set; }
        public int MealID { get; set; }
        public virtual MealDTO Meal { get; set; } = null!;
        public int UserID { get; set; }
        public virtual UserDTO User { get; set; } = null!;
    }
}

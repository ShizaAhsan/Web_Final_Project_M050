namespace Web_Final_Project_sem_5.Models
{
    public class Farm
    {
        public int FarmId { get; set; }
        public string FarmerId { get; set; } = string.Empty;
        public string FarmName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal LandArea { get; set; }
        public string AreaUnit { get; set; } = "Acres";
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
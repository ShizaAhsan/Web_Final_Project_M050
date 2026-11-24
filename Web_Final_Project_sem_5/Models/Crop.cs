namespace Web_Final_Project_sem_5.Models
{
    public class Crop
    {
        public int CropId { get; set; }
        public int FarmId { get; set; }
        public int CropTypeId { get; set; }

        public DateTime SowingDate { get; set; }
        public DateTime ExpectedHarvestDate { get; set; }
        public DateTime? ActualHarvestDate { get; set; }

        public string Status { get; set; } = "Planned";
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // Display properties
        public string? CropName { get; set; }
        public string? FarmName { get; set; }
    }
}
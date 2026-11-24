using Web_Final_Project_sem_5.Models;

namespace Web_Final_Project_sem_5.Interfaces
{
    public interface ICropRepository
    {
        Task<IEnumerable<Crop>> GetAllCropsByFarmerId(string farmerId);

        Task<Crop?> GetCropById(int cropId, string farmerId);

        Task<IEnumerable<CropType>> GetCropTypes();

        Task<int> AddCrop(Crop crop, string farmerId);

        Task<int> UpdateCrop(Crop crop, string farmerId);

        Task<int> UpdateCropStatus(
            int cropId,
            string status,
            string farmerId);

        Task<IEnumerable<Crop>> GetFarmHistory(
            int farmId,
            string farmerId);
    }
}
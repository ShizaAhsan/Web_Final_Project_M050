
using Web_Final_Project_sem_5.Models;

namespace Web_Final_Project_sem_5.Interfaces
{
    public interface IFarmRepository
    {
        // Get all farms of a particular farmer
        Task<IEnumerable<Farm>> GetAllFarmsByFarmerId(string farmerId);

        // Get one farm by its ID
        Task<Farm?> GetFarmById(int farmId, string farmerId);

        // Add a new farm
        Task<int> AddFarm(Farm farm);

        // Update existing farm
        Task<int> UpdateFarm(Farm farm);

        // Deactivate a farm instead of permanently deleting it
        Task<int> DeleteFarm(int farmId, string farmerId);
    }
}

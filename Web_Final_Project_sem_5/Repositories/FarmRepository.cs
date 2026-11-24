
using Dapper;
using Microsoft.Data.SqlClient;
using Web_Final_Project_sem_5.Interfaces;
using Web_Final_Project_sem_5.Models;

namespace Web_Final_Project_sem_5.Repositories
{
    public class FarmRepository : IFarmRepository
    {
        private readonly string _connectionString;

        public FarmRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Farm>> GetAllFarmsByFarmerId(string farmerId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                SELECT *
                FROM Farms
                WHERE FarmerId = @FarmerId AND IsActive = 1";

            return await connection.QueryAsync<Farm>(
                query, new { FarmerId = farmerId });
        }

        public async Task<Farm?> GetFarmById(int farmId, string farmerId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                SELECT *
                FROM Farms
                WHERE FarmId = @FarmId
                AND FarmerId = @FarmerId
                AND IsActive = 1";

            return await connection.QueryFirstOrDefaultAsync<Farm>(
                query, new { FarmId = farmId, FarmerId = farmerId });
        }

        public async Task<int> AddFarm(Farm farm)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                INSERT INTO Farms
                (FarmerId, FarmName, Location, LandArea, AreaUnit, Description)
                VALUES
                (@FarmerId, @FarmName, @Location, @LandArea, @AreaUnit, @Description)";

            return await connection.ExecuteAsync(query, farm);
        }

        public async Task<int> UpdateFarm(Farm farm)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                UPDATE Farms
                SET FarmName = @FarmName,
                    Location = @Location,
                    LandArea = @LandArea,
                    AreaUnit = @AreaUnit,
                    Description = @Description
                WHERE FarmId = @FarmId
                AND FarmerId = @FarmerId
                AND IsActive = 1";

            return await connection.ExecuteAsync(query, farm);
        }

        public async Task<int> DeleteFarm(int farmId, string farmerId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                UPDATE Farms
                SET IsActive = 0
                WHERE FarmId = @FarmId
                AND FarmerId = @FarmerId";

            return await connection.ExecuteAsync(
                query, new { FarmId = farmId, FarmerId = farmerId });
        }
    }
}

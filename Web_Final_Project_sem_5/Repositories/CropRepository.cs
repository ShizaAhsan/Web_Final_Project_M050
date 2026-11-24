using Dapper;
using Microsoft.Data.SqlClient;
using Web_Final_Project_sem_5.Interfaces;
using Web_Final_Project_sem_5.Models;

namespace Web_Final_Project_sem_5.Repositories
{
    public class CropRepository : ICropRepository
    {
        private readonly string _connectionString;

        public CropRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Crop>> GetAllCropsByFarmerId(
            string farmerId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                SELECT
                    c.*,
                    ct.CropName,
                    f.FarmName
                FROM Crops c
                INNER JOIN CropTypes ct
                    ON c.CropTypeId = ct.CropTypeId
                INNER JOIN Farms f
                    ON c.FarmId = f.FarmId
                WHERE f.FarmerId = @FarmerId
                AND f.IsActive = 1
                ORDER BY c.CreatedAt DESC";

            return await connection.QueryAsync<Crop>(
                query,
                new { FarmerId = farmerId });
        }

        public async Task<Crop?> GetCropById(
            int cropId,
            string farmerId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                SELECT
                    c.*,
                    ct.CropName,
                    f.FarmName
                FROM Crops c
                INNER JOIN CropTypes ct
                    ON c.CropTypeId = ct.CropTypeId
                INNER JOIN Farms f
                    ON c.FarmId = f.FarmId
                WHERE c.CropId = @CropId
                AND f.FarmerId = @FarmerId
                AND f.IsActive = 1";

            return await connection.QueryFirstOrDefaultAsync<Crop>(
                query,
                new
                {
                    CropId = cropId,
                    FarmerId = farmerId
                });
        }

        public async Task<IEnumerable<CropType>> GetCropTypes()
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                SELECT CropTypeId, CropName, Description
                FROM CropTypes
                ORDER BY CropName";

            return await connection.QueryAsync<CropType>(query);
        }

        public async Task<int> AddCrop(
            Crop crop,
            string farmerId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                INSERT INTO Crops
                (
                    FarmId,
                    CropTypeId,
                    SowingDate,
                    ExpectedHarvestDate,
                    Status,
                    Notes
                )
                SELECT
                    @FarmId,
                    @CropTypeId,
                    @SowingDate,
                    @ExpectedHarvestDate,
                    @Status,
                    @Notes
                FROM Farms
                WHERE FarmId = @FarmId
                AND FarmerId = @FarmerId
                AND IsActive = 1";

            return await connection.ExecuteAsync(query, new
            {
                crop.FarmId,
                crop.CropTypeId,
                crop.SowingDate,
                crop.ExpectedHarvestDate,
                crop.Status,
                crop.Notes,
                FarmerId = farmerId
            });
        }

        public async Task<int> UpdateCrop(
            Crop crop,
            string farmerId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                UPDATE c
                SET
                    c.FarmId = @FarmId,
                    c.CropTypeId = @CropTypeId,
                    c.SowingDate = @SowingDate,
                    c.ExpectedHarvestDate = @ExpectedHarvestDate,
                    c.Notes = @Notes
                FROM Crops c
                INNER JOIN Farms oldFarm
                    ON c.FarmId = oldFarm.FarmId
                WHERE c.CropId = @CropId
                AND oldFarm.FarmerId = @FarmerId
                AND oldFarm.IsActive = 1
                AND EXISTS
                (
                    SELECT 1
                    FROM Farms newFarm
                    WHERE newFarm.FarmId = @FarmId
                    AND newFarm.FarmerId = @FarmerId
                    AND newFarm.IsActive = 1
                )";

            return await connection.ExecuteAsync(query, new
            {
                crop.CropId,
                crop.FarmId,
                crop.CropTypeId,
                crop.SowingDate,
                crop.ExpectedHarvestDate,
                crop.Notes,
                FarmerId = farmerId
            });
        }

        public async Task<int> UpdateCropStatus(
            int cropId,
            string status,
            string farmerId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                UPDATE c
                SET
                    c.Status = @Status,
                    c.ActualHarvestDate =
                        CASE
                            WHEN @Status = 'Harvested'
                            THEN CAST(GETDATE() AS DATE)
                            ELSE c.ActualHarvestDate
                        END
                FROM Crops c
                INNER JOIN Farms f
                    ON c.FarmId = f.FarmId
                WHERE c.CropId = @CropId
                AND f.FarmerId = @FarmerId
                AND f.IsActive = 1";

            return await connection.ExecuteAsync(query, new
            {
                CropId = cropId,
                Status = status,
                FarmerId = farmerId
            });
        }

        public async Task<IEnumerable<Crop>> GetFarmHistory(
            int farmId,
            string farmerId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                SELECT
                    c.*,
                    ct.CropName,
                    f.FarmName
                FROM Crops c
                INNER JOIN CropTypes ct
                    ON c.CropTypeId = ct.CropTypeId
                INNER JOIN Farms f
                    ON c.FarmId = f.FarmId
                WHERE c.FarmId = @FarmId
                AND f.FarmerId = @FarmerId
                AND f.IsActive = 1
                ORDER BY c.SowingDate DESC";

            return await connection.QueryAsync<Crop>(
                query,
                new
                {
                    FarmId = farmId,
                    FarmerId = farmerId
                });
        }
    }
}
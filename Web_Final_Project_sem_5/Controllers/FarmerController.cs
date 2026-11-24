using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web_Final_Project_sem_5.Interfaces;
using Web_Final_Project_sem_5.Models;

namespace Web_Final_Project_sem_5.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class FarmerController : Controller
    {
        private readonly IFarmRepository _farmRepository;
        private readonly ICropRepository _cropRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public FarmerController(
            IFarmRepository farmRepository,
            ICropRepository cropRepository,
            UserManager<ApplicationUser> userManager)
        {
            _farmRepository = farmRepository;
            _cropRepository = cropRepository;
            _userManager = userManager;
        }

        private string GetFarmerId()
        {
            return _userManager.GetUserId(User)!;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        // ---------------- MY FARMS ----------------

        [HttpGet]
        public async Task<IActionResult> MyFarms()
        {
            string farmerId = GetFarmerId();

            var farms =
                await _farmRepository.GetAllFarmsByFarmerId(farmerId);

            return View(farms);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFarm(Farm farm)
        {
            if (string.IsNullOrWhiteSpace(farm.FarmName) ||
                string.IsNullOrWhiteSpace(farm.Location) ||
                farm.LandArea <= 0 ||
                string.IsNullOrWhiteSpace(farm.AreaUnit))
            {
                TempData["Error"] = "Please enter valid farm details.";
                return RedirectToAction(nameof(MyFarms));
            }

            farm.FarmerId = GetFarmerId();

            await _farmRepository.AddFarm(farm);

            TempData["Success"] = "Farm added successfully!";

            return RedirectToAction(nameof(MyFarms));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFarm(Farm farm)
        {
            string farmerId = GetFarmerId();

            var existingFarm =
                await _farmRepository.GetFarmById(
                    farm.FarmId,
                    farmerId);

            if (existingFarm == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(farm.FarmName) ||
                string.IsNullOrWhiteSpace(farm.Location) ||
                farm.LandArea <= 0 ||
                string.IsNullOrWhiteSpace(farm.AreaUnit))
            {
                TempData["Error"] = "Please enter valid farm details.";
                return RedirectToAction(nameof(MyFarms));
            }

            farm.FarmerId = farmerId;

            await _farmRepository.UpdateFarm(farm);

            TempData["Success"] = "Farm updated successfully!";

            return RedirectToAction(nameof(MyFarms));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFarm(int id)
        {
            string farmerId = GetFarmerId();

            int result =
                await _farmRepository.DeleteFarm(id, farmerId);

            TempData[result > 0 ? "Success" : "Error"] =
                result > 0
                    ? "Farm removed successfully!"
                    : "Farm not found.";

            return RedirectToAction(nameof(MyFarms));
        }

        // ---------------- MY CROPS ----------------

        [HttpGet]
        public async Task<IActionResult> MyCrops()
        {
            string farmerId = GetFarmerId();

            var crops =
                await _cropRepository.GetAllCropsByFarmerId(farmerId);

            ViewBag.Farms =
                await _farmRepository.GetAllFarmsByFarmerId(farmerId);

            ViewBag.CropTypes =
                await _cropRepository.GetCropTypes();

            return View(crops);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCrop(Crop crop)
        {
            if (crop.FarmId <= 0 ||
                crop.CropTypeId <= 0 ||
                crop.SowingDate == default ||
                crop.ExpectedHarvestDate == default ||
                crop.ExpectedHarvestDate < crop.SowingDate)
            {
                TempData["Error"] = "Please enter valid crop details.";
                return RedirectToAction(nameof(MyCrops));
            }

            crop.Status = "Planned";

            int result = await _cropRepository.AddCrop(
                crop,
                GetFarmerId());

            TempData[result > 0 ? "Success" : "Error"] =
                result > 0
                    ? "Crop added successfully!"
                    : "Farm not found. Please select your own farm.";

            return RedirectToAction(nameof(MyCrops));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCrop(Crop crop)
        {
            string farmerId = GetFarmerId();

            var existingCrop =
                await _cropRepository.GetCropById(
                    crop.CropId,
                    farmerId);

            if (existingCrop == null)
            {
                return NotFound();
            }

            if (crop.FarmId <= 0 ||
                crop.CropTypeId <= 0 ||
                crop.SowingDate == default ||
                crop.ExpectedHarvestDate == default ||
                crop.ExpectedHarvestDate < crop.SowingDate)
            {
                TempData["Error"] = "Please enter valid crop details.";
                return RedirectToAction(nameof(MyCrops));
            }

            int result =
                await _cropRepository.UpdateCrop(crop, farmerId);

            TempData[result > 0 ? "Success" : "Error"] =
                result > 0
                    ? "Crop updated successfully!"
                    : "Crop could not be updated.";

            return RedirectToAction(nameof(MyCrops));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCropStatus(
            int id,
            string status)
        {
            string[] allowedStatuses =
            {
                "Planned",
                "Growing",
                "Harvested"
            };

            if (!allowedStatuses.Contains(status))
            {
                TempData["Error"] = "Invalid crop status.";
                return RedirectToAction(nameof(MyCrops));
            }

            int result =
                await _cropRepository.UpdateCropStatus(
                    id,
                    status,
                    GetFarmerId());

            TempData[result > 0 ? "Success" : "Error"] =
                result > 0
                    ? "Crop status updated!"
                    : "Crop not found.";

            return RedirectToAction(nameof(MyCrops));
        }
    }
}
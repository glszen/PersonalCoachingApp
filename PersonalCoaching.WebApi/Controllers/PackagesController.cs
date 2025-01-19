using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalCoaching.WebApi.Filter;
using PersonalCoaching.WebApi.Models;
using PersonalCoachingApp.Business.Operations.Package;
using PersonalCoachingApp.Business.Operations.Package.Dtos;
using System.Diagnostics.Eventing.Reader;

namespace PersonalCoaching.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackagesController(IPackageService packageService) 
        { 
            _packageService = packageService;
        }

        [HttpGet("{id}/PackagesById")]
        public async Task<IActionResult> GetPackage (int id)
        {
            var package = await _packageService.GetPackage(id);

            if (package is null)
                return NotFound();
            else
                return Ok(package);
        }

        [HttpGet("AllPackages")]
        public async Task<IActionResult> GetPackages()
        {
            var packages = await _packageService.GetAllPackages();

            return Ok(packages);
        }


        [HttpPost("AddPackage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPackage(AddPackageRequest request)
        {
            var addPackageDto = new AddPackageDto
            {
                Name = request.Name,
                Description = request.Description,
                TrainingDuration = request.TrainingDuration,
                PackagePrice = request.PackagePrice,
                PackageType = request.PackageType,
                FeatureIds = request.FeatureIds,
                StockQuantity= request.StockQuantity
            };

            var result = await _packageService.AddPackage(addPackageDto);

            if (!result.IsSucceed)
            {
                return BadRequest(result.Message);
            }
            else
            {
                return Ok();
            }
        }

        [HttpPatch("{id}/UpdatePrice")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>AdjustPackagePrice (int id, int changeBy)
        {
            var result = await _packageService.AdjustPackagePrice(id, changeBy);

            if (!result.IsSucceed)
                return NotFound(result.Message);  
            else
                return Ok();    
        }

        [HttpDelete("{id}/DeletePackage")]
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> DeletePackage(int id)
        {
            var result = await _packageService.DeletePackage(id);

            if (!result.IsSucceed)
                return NotFound();
            else
                return Ok();
        }

        [HttpPut("{id}/UpdatePackage")]
        [Authorize(Roles = "Admin")]
        [TimeControlFilter] //Action Filter
        public async Task<IActionResult> UpdatePackage(int id, UpdatePackageRequest request)
        {
            var updatePackageDto = new UpdatePackageDto
            {
                Id = id,
                Name = request.Name,
                PackagePrice = request.PackagePrice,
                TrainingDuration = request.TrainingDuration,
                Description = request.Description,
                PackageType = request.PackageType,
                FeatureIds = request.FeatureIds,
                StockQuantity = request.StockQuantity
            };

            var result =  await _packageService.UpdatePackage(updatePackageDto);

            if (!result.IsSucceed)
                return NotFound(result.Message);
            else return await GetPackage(id);

        }
    }
}

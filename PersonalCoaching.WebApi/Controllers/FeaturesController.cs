using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalCoaching.WebApi.Models;
using PersonalCoachingApp.Business.Operations.Feature;
using PersonalCoachingApp.Business.Operations.Feature.Dtos;

namespace PersonalCoaching.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly IFeatureService _featureService;

        public FeaturesController(IFeatureService featureService)
        {
            _featureService = featureService; 
        }

        [HttpGet("AllFeature")]
        public async Task<IActionResult> GetAllFeatures()
        {
            var features = await _featureService.GetAllFeatures();

            return Ok(features);
        }

        [HttpPost("AddFeature")]
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> AddFeature(AddFeatureRequest request)
        {
            var addFeatureDto = new AddFeatureDto
            {
                Title = request.Title,
            };
            var result = await _featureService.AddFeature(addFeatureDto);

            if(result.IsSucceed)
                return Ok();
            else 
                return BadRequest(result.Message);
        }

        [HttpDelete("{id}/DeleteFeature")]
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> DeletePackage(int id)
        {
            var result = await _featureService.DeleteFeature(id);

            if (!result.IsSucceed)
                return NotFound();
            else
                return Ok();
        }


    }
}

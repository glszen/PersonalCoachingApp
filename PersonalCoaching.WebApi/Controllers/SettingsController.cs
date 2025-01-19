using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalCoachingApp.Business.Operations.Setting;
using System.Reflection.Metadata.Ecma335;

namespace PersonalCoaching.WebApi.Controllers
{
    [Route("api/[controller]/Maintenance")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> ToggleMaintenence()
        {
            await _settingService.ToggleMaintenence();

            return Ok();
        }
        
    }
}

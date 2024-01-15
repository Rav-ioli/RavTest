using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MyApplication.Dto;
//using MyApplication.Domain;
using MyApplication.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;
using AccessibilityModels;

namespace MyApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeperkingHulpmiddelController : ControllerBase
    {
        private readonly HulpmiddelService _hulpmiddelService;
        private readonly BeperkingService _beperkingService;
        public BeperkingHulpmiddelController(HulpmiddelService hulpmiddelService, BeperkingService beperkingService)
        {
            _hulpmiddelService = hulpmiddelService;
            _beperkingService = beperkingService;
        }



        [HttpGet("GetAllHulpmiddelen")]
        public async Task<ActionResult<List<Hulpmiddel>>> GetAllHulpmiddelen()
        {
            return await _hulpmiddelService.GetHulpmiddelen();
        }
        [HttpGet("GetAllBeperkingen")]
        public async Task<ActionResult<List<Beperking>>> GetAllBeperkingen()
        {
            return await _beperkingService.GetBeperkingen();
        }
    }
}
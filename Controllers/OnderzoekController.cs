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
    public class OnderzoekController : ControllerBase
    {
        private readonly OnderzoekService _onderzoekService;
        private UserService _userService;
        private BeperkingService _beperkingService;
        public OnderzoekController(OnderzoekService onderzoekService, UserService userService, BeperkingService beperkingService)
        {
            _onderzoekService = onderzoekService;
            _userService = userService;
            _beperkingService = beperkingService;
        }
[Authorize(Policy = "UserOnly")]
        [HttpGet("GetAllOnderzoeken")]
        public async Task<ActionResult<List<Onderzoek>>> GetAllOnderzoeken()
        {
            // var result = await _onderzoekService.GetOnderzoeken();
            // var  uitvoerendBedrijf = result.UitvoerendBedrijf;
            return await _onderzoekService.GetOnderzoeken();
        }
        [Authorize(Policy = "UserOnly")]
        [HttpGet("GetOnderzoekenByUserEmail")]
        public async Task<ActionResult<List<Onderzoek>>> GetOnderzoekenByUserEmail([FromHeader(Name = "Email")] string email)
        {
            // var beperkingList = await _beperkingService.GetBeperkingenByUserEmail(email);
            // var result = _onderzoekService.GetOnderzoekenByBeperking(beperkingList);
            // return await result;

            var beperkingList = await _beperkingService.GetBeperkingenByUserEmail(email);
            var result = new List<Onderzoek>();

            foreach (var beperking in beperkingList)
            {
                Console.WriteLine($"Fetching onderzoeken for beperking: {beperking}");
                var onderzoeken = await _onderzoekService.GetOnderzoekenByBeperking(beperking);
                Console.WriteLine($"Fetched {onderzoeken.Count} onderzoeken for beperking: {beperking}");
                result.AddRange(onderzoeken);
            }

            return result;
        }

        [HttpPost("CreateOnderzoek")]
        public async Task<ActionResult<Onderzoek>> CreateOnderzoek([FromBody] OnderzoekDto onderzoekDto)
        {
            return await _onderzoekService.CreateOnderzoek(onderzoekDto);
        }
        [Authorize(Policy = "UserOnly")]
        [HttpGet("GetOnderzoekByID/{id}")]
        public async Task<ActionResult<Onderzoek>> GetOnderzoekByID(int id)
        {
            System.Console.WriteLine(id);
            var result = await _onderzoekService.GetOnderzoek(id);
            System.Console.WriteLine(result);
            return result;
        }
        [HttpPost("JoinErvaringsdeskundigeToOnderzoek")]
        public async Task<ActionResult> JoinErvaringsdeskundigeToOnderzoek([FromBody] ErvaringsdeskundigeOnderzoekDto ervaringsdeskundigeOnderzoekDto)
        {
            var gebruiker = await _userService.GetUserIdByEmailAndDiscriminatorAsync(ervaringsdeskundigeOnderzoekDto.email, "Ervaringsdeskundige");

            await _onderzoekService.CreateUserOnderzoekAsync(gebruiker, ervaringsdeskundigeOnderzoekDto.onderzoek);
            return Ok();
        }
        [Authorize(Policy = "UserOnly")]
        [HttpGet("GetCountAanmeldingForEachOnderzoek")]
        public async Task<ActionResult<List<OnderzoekCount>>> GetCountAanmeldingForEachOnderzoek()
        {
            var result = await _onderzoekService.GetCountAanmeldingForEachOnderzoek();
            return result;
        }
    }
}
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
        public OnderzoekController(OnderzoekService onderzoekService)
        {
            _onderzoekService = onderzoekService;
        }

        [HttpGet("GetAllOnderzoeken")]
        public async Task<ActionResult<List<Onderzoek>>> GetAllOnderzoeken()
        {
            return await _onderzoekService.GetOnderzoeken();
        }
        [HttpPost("CreateOnderzoek")]
        public async Task<ActionResult<Onderzoek>> CreateOnderzoek([FromBody] OnderzoekDto onderzoekDto)
        {
            return await _onderzoekService.CreateOnderzoek(onderzoekDto);
        }
        [HttpGet("GetOnderzoekByID/{id}")]
        public async Task<ActionResult<Onderzoek>> GetOnderzoekByID(int id)
        {
            System.Console.WriteLine(id);
            var result=await _onderzoekService.GetOnderzoek(id);
            System.Console.WriteLine(result);
            return result;
        }
    }
}
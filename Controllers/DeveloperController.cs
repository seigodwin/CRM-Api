using CRMApi.DbContexts;
using CRMApi.Domain.DTOs;
using CRMApi.Domain.Models;
using CRMApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeveloperController(IDeveloperService developerService) : ControllerBase
    {
        private readonly IDeveloperService _developerService = developerService;

        [HttpGet]
        public async Task<IActionResult> GetAllDevelopers()
        {
            var serviceResponse = await _developerService.GetAllDevelopers();

            if (!serviceResponse.Success)
            {
                return NotFound(new { serviceResponse.Success, serviceResponse.Message });
            }

            return Ok(serviceResponse.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeveloperById(int id)
        {
            var serviceResponse = await _developerService.GetDeveloperById(id);

            if (!serviceResponse.Success)
            {
                return NotFound(new { serviceResponse.Success, serviceResponse.Message });
            }

            return Ok(serviceResponse.Data);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeveloperById(int id)
        {
            var serviceResponse = await _developerService.DeleteDeveloperById(id);

            if (!serviceResponse.Success)
            {
                return NotFound(new { serviceResponse.Success, serviceResponse.Message });
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> CreateDeveloper([FromBody] CreateDeveloperDTO NewDeveloperDTO)
        {
            
           if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceResponse = await _developerService.CreateDeveloper(NewDeveloperDTO);

            if (!serviceResponse.Success)
            {
                return BadRequest(new { serviceResponse.Success, serviceResponse.Message });
            }

            return CreatedAtAction(nameof(GetDeveloperById), new { id = serviceResponse.Data.Id }, serviceResponse.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeveloper(int id, DeveloperDTO UpdatedDeveloperDTO)
        {
            if (UpdatedDeveloperDTO is null)
            {
                return BadRequest("Developer data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceResponse = await _developerService.UpdateDeveloperById(id, UpdatedDeveloperDTO);

            if (!serviceResponse.Success)
            {
                return BadRequest(new { serviceResponse.Success, serviceResponse.Message });
            }

            return NoContent();
        }

    }
}

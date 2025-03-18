using CRMApi.DbContexts;
using CRMApi.Domain.DTOs;
using CRMApi.Domain.Models;
using CRMApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace CRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController(IProjectService projectService) : ControllerBase
    {
        private readonly IProjectService _projectService = projectService;

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var response = await _projectService.GetAllProjects();

            if (!response.Success)
            {
                return NotFound(new { response.Success, response.Message });
            }

            return Ok(response.Data);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var response = await _projectService.GetProjectById(id);

            if (!response.Success)
            {
                return NotFound(new { response.Success, response.Message });
            }

            return Ok(response.Data);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectById(int id)
        {
            var response = await _projectService.DeleteProjectById(id);

            if (!response.Success)
            {
                return NotFound(new { response.Success, response.Message });
            }

            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, ProjectDTO NewProjectDTO)
        {
           if (NewProjectDTO is null)
            {
                return BadRequest("Project data is null");
            }

           if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _projectService.UpdateProjectById(id , NewProjectDTO);

            if (!response.Success)
            {
                return BadRequest(new { response.Success, response.Message });
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDTO NewProjectDTO)
        {
            if (NewProjectDTO is null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _projectService.CreateProject(NewProjectDTO);

            if (!response.Success)
            {
                return BadRequest(new { response.Success, response.Message });
            }

            return CreatedAtAction(nameof(GetProjectById), new { id = response.Data.Id }, response.Data);

        }
    }
}

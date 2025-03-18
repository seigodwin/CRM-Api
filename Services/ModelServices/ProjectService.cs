using CRMApi.DbContexts;
using CRMApi.Domain.DTOs;
using CRMApi.Domain.Models;
using CRMApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMApi.Services.Services
{
    public class ProjectService(CRMApiDbContext context) : IProjectService
    {
        private readonly CRMApiDbContext _context = context;
        public async Task<ServiceResponse<CreateProjectDTO>> CreateProject(CreateProjectDTO projectDTO)
        {
            var response = new ServiceResponse<CreateProjectDTO>();

            if (projectDTO is null) 
            {
                response.Message = "Developer DTO is null!";
                response.Success = false;
                return response;
            }

            var project = new Project
            {
                Title = projectDTO.Title,
                Description = projectDTO.Description,
                ClientName = projectDTO.ClientName,
                Status = (ProjectStatus)projectDTO.Status,
                DevelopersAssigned = projectDTO.DevelopersAssigned

            };

            try
            {
                await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();

                response.Data = new CreateProjectDTO
                {
                    Id = project.Id,
                    Title = project.Title,
                    Description = project.Description,
                    ClientName = project.ClientName,
                    Status = project.Status,
                    DevelopersAssigned = project.DevelopersAssigned
                };

                response.Message = "Developer Created successfully";
                response.Success = true;

            }

            catch (DbUpdateException dbEx)
            {
                response.Message = $"Database error: {dbEx.Message}";
                response.Success = false;

            }

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteProjectById(int id)
        {
            var response = new ServiceResponse<bool>();

            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

            if (project is null)
            {
                response.Message = $"Project with Id: {id} not found!";
                response.Success = false;
                return response;
            }

            try
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                response.Message = "Project deleted Successfully";
                response.Success = true;
            }

            catch( DbUpdateException dbEx)
            {
                response.Message = $"A database error occured while deleting project: {dbEx.Message}";
                response.Success = false;
            }

            return response;
        }

        public async Task<ServiceResponse<List<ProjectDTO>>> GetAllProjects()
        {
            var response = new ServiceResponse<List<ProjectDTO>>();

            var projectsDTO = new List<ProjectDTO>();

            var projects = await _context.Projects.ToListAsync();

            if (projects is null)
            {
                response.Message = "No records found!";
                response.Success = false;
                return response;
            }

            foreach(var project in projects)
            {
                projectsDTO.Add(new ProjectDTO
                {
                    Title = project.Title,
                    Description = project.Description,
                    ClientName = project.ClientName,
                    DevelopersAssigned = project.DevelopersAssigned,
                    Status = project.Status
                });
            }

            response.Data = projectsDTO;
            response.Message = "Projects retrieved successfully";
            response.Success = true;

            return response;

        }

        public async Task<ServiceResponse<ProjectDTO>> GetProjectById(int id)
        {
            var response = new ServiceResponse<ProjectDTO>();

            var project = await _context.Projects.FirstOrDefaultAsync(d => d.Id == id);

            if (project is null)
            {
                response.Message = $"Developer with id: {id} not found!";
                response.Success = false;
                return response; 
            }

            response.Data = new ProjectDTO
            {
                Title = project.Title,
                Description = project.Description,
                ClientName = project.ClientName,
                DevelopersAssigned = project.DevelopersAssigned,
                Status = project.Status
            };
            response.Message = "Project retrieved Successfully";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateProjectById(int id, ProjectDTO projectDTO)
        {
            var response = new ServiceResponse<bool>();

            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

            if (project is null)
            {
                response.Message = $"Project with id: {id} not found!";
                response.Success = false;
                return response;
            }

            project.Title = projectDTO.Title;
            project.Description = projectDTO.Description;
            project.ClientName = projectDTO.ClientName;
            project.DevelopersAssigned = projectDTO.DevelopersAssigned;
            project.Status = (ProjectStatus)projectDTO.Status;

            try
            {
                await _context.SaveChangesAsync();

                response.Message = "Developer Updated Successfully";
                response.Success = true;
            }

            catch (DbUpdateException dbEx)
            {
                response.Message = $"Database error: {dbEx.Message}";
                response.Success = false;
            }

            return response;
        }
    }
}

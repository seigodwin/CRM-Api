using CRMApi.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CRMApi.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ServiceResponse<List<ProjectDTO>>> GetAllProjects();   
        Task<ServiceResponse<ProjectDTO>> GetProjectById(int id);
        Task<ServiceResponse<bool>> DeleteProjectById(int id);
        Task<ServiceResponse<bool>> UpdateProjectById(int id, ProjectDTO projectDTO);
        Task<ServiceResponse<CreateProjectDTO>> CreateProject(CreateProjectDTO projectDTO);

    }
}

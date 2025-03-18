using CRMApi.Domain.DTOs;
using CRMApi.Services;
using CRMApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRMApi.Services.Interfaces
{
    public interface IDeveloperService
    {
        Task<ServiceResponse<List<DeveloperDTO>>> GetAllDevelopers();
        Task<ServiceResponse<DeveloperDTO>> GetDeveloperById(int id);
        Task<ServiceResponse<bool>> DeleteDeveloperById(int id);
        Task<ServiceResponse<bool>> UpdateDeveloperById(int id, DeveloperDTO developerDTO);
        Task<ServiceResponse<CreateDeveloperDTO>> CreateDeveloper(CreateDeveloperDTO developerDTO);

    }
}

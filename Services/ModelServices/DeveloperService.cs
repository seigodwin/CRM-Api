using System.Linq.Expressions;
using CRMApi.DbContexts;
using CRMApi.Domain.DTOs;
using CRMApi.Domain.Models;
using CRMApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMApi.Services.Services
{
    public class DeveloperService(CRMApiDbContext context) : IDeveloperService
    {
        private readonly CRMApiDbContext _context = context;
        public async Task<ServiceResponse<CreateDeveloperDTO>> CreateDeveloper(CreateDeveloperDTO developerDTO)
        {
            var response = new ServiceResponse<CreateDeveloperDTO>();


            if (developerDTO is null)
            {
                response.Message = "Developer data is null";
                response.Success = false;
                return response;
            }

            var developer = new Developer
            {
                Name = developerDTO.Name,
                Email = developerDTO.Email,
                Stack = developerDTO.Stack,
                ProjectsAssigned = developerDTO.ProjectsAssigned
            };
            try
            {

                await _context.Developers.AddAsync(developer);
                await _context.SaveChangesAsync();

                response.Data = new CreateDeveloperDTO
                {
                    Id = developer.Id,
                    Name = developer.Name,
                    Email = developer.Email,
                    Stack = developer.Stack,
                    ProjectsAssigned = developer.ProjectsAssigned

                };
                response.Message = "Developer Created Successfully";
                response.Success = true;

            }
            catch (DbUpdateException dbEx)
            {
                response.Message = $"Database error: {dbEx.Message}";
                response.Success = false;
            }

            catch (Exception ex)
            {
                response.Message = $"An error occured while creating developer : {ex.Message}";
                response.Success = false;
            }
       
        
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteDeveloperById(int id)
        {
            var response = new ServiceResponse<bool>();

            var developer = await _context.Developers.FirstOrDefaultAsync( d => d.Id == id);
            if (developer == null)
            {
                response.Message = $"Developer with Id {id} not found";
                response.Success = false;
                return response;
            }

            try
            {
                _context.Developers.Remove(developer);
                await _context.SaveChangesAsync();

                response.Message = "Developer deleted Successfully";
                response.Success = true;
            }
            
            catch (Exception ex)
            {
                response.Message = $"An error occured while deleting developer: {ex.Message}";
                response.Success = false;
            }

            return response;
        }

        public async Task<ServiceResponse<List<DeveloperDTO>>> GetAllDevelopers()
        {
            var response = new ServiceResponse<List<DeveloperDTO>>();

            var developers = await _context.Developers.ToListAsync();

            var developersDTO = new List<DeveloperDTO>();

            if ( developers is null)
            {
                response.Message = "No records found";
                response.Success = false;
                return response;
            }

            foreach (var developer in developers)
            {
                developersDTO.Add(new DeveloperDTO
                {
                   Name = developer.Name,
                   Email = developer.Email,
                   Stack = developer.Stack,
                   ProjectsAssigned = developer.ProjectsAssigned
                 });
            }

            response.Data = developersDTO;
            response.Message = "Developers retrieved successfully";
            response.Success = true;
            
            return response;
        }
        public async Task<ServiceResponse<DeveloperDTO>> GetDeveloperById(int id)
        {
            var response = new ServiceResponse<DeveloperDTO>();

            var developer = await _context.Developers.FirstOrDefaultAsync(d => d.Id == id);

            if (developer is null)
            {
                response.Message = $"Developer with id {id} not found!";
                response.Success = false;
                return response;
            }

            response.Data = new DeveloperDTO
            {
                Name = developer.Name,
                Email = developer.Email,
                Stack = developer.Stack,
                ProjectsAssigned = developer.ProjectsAssigned
            };
            response.Message = "Developer retrieved Successfully";
            response.Success = true;

            return response;
            
        }

        public async Task<ServiceResponse<bool>> UpdateDeveloperById(int id, DeveloperDTO developerDTO)
        {
            var response = new ServiceResponse<bool>();
            
            var developer = await _context.Developers.FirstOrDefaultAsync(d => d.Id == id);
            
            if (developer is null)
            {
                response.Message = "Developer with id: {id} not found!";
                response.Success = false;
                return response;
            }

            developer.Name = developerDTO.Name;
            developer.Email = developerDTO.Email;
            developer.Stack = developerDTO.Stack;
            developer.ProjectsAssigned = developerDTO.ProjectsAssigned;

            try
            {
                await _context.SaveChangesAsync();

                response.Message = "Developer Updated Successfully";
                response.Success = true;
            }

            catch(DbUpdateException dbEx)
            {
                response.Message = $"Database error: {dbEx.Message}";
                response.Success = false;
            }

            return response;
        }

        
    }
}

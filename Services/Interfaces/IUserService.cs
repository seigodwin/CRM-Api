using CRMApi.Domain.DTOs;
using CRMApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRMApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<bool>> Register(UserDTO userDTO);
        Task<ServiceResponse<string>> Login(LoginDTO loginDTO);
    }
}

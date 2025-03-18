using CRMApi.Domain.Models;

namespace CRMApi.Domain.DTOs
{
    public class ProjectDTO 
    {
        public string? Title { get; set; }
        public string? ClientName { get; set; }
        public string? Description { get; set; }
        public List<Developer>? DevelopersAssigned { get; set; }
        public ProjectStatus? Status { get; set; } = ProjectStatus.Pending;
    }
}

namespace CRMApi.Domain.Models
{
    public enum ProjectStatus
    {
        Pending,
        Started,
        Finished,
        Cancelled
    }

    public class Project
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ClientName { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Pending;
        public List<Developer>? DevelopersAssigned { get; set; } 
    }
}

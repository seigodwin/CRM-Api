namespace CRMApi.Domain.Models
{
    public class Developer
    {
        public int Id { get; set; } 
        public string? Name { get; set; }   
        public string? Email { get; set; }  
        public List<string>? Stack { get; set; }
        public List<Project>? ProjectsAssigned { get; set; } 
    }
}

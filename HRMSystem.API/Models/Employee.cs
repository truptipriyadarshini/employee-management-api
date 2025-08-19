using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HRMSystem.API.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int DepartmentId { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfJoining { get; set; }
        [NotMapped]   // 👈 prevents EF from expecting a column
        public string? DepartmentName { get; set; }
        [JsonIgnore] // 👈 hides from JSON output
        public Department? Department { get; set; }
    }
}

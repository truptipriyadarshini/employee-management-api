using System.Text.Json.Serialization;

namespace HRMSystem.API.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        [JsonIgnore]
        public List<Employee>? Employees { get; set; }
    }
}

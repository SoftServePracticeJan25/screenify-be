using System.Text.Json.Serialization;

namespace Domain.DTOs.Data
{
    public class ActorDto
    {
        public required string Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] 
        public string? Bio { get; set; }

        public DateTime? BirthDate { get; set; }
        public string? PhotoUrl { get; set; }
    }
}

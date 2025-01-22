using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ActorRole
    {
        public int Id { get; set; }
        public string? RoleName { get; set; } = string.Empty;

        public List<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}

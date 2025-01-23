using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SeatsAmount { get; set; }
        public int? CinemaTypeId { get; set; }
        public CinemaType? CinemaType { get; set; }

        public List<Session> Sessions { get; set; } = new List<Session>();
    }
}

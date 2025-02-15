using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Helpers.QueryObject
{
    public class TicketQueryObject
    {
        public string? UserId { get; set; }  
        public int? MovieId { get; set; }    
    }
}
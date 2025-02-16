using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Helpers.QueryObject
{
    public class SessionQueryObject
    {
        public DateTime? StartDate { get; set; }  
        public DateTime? EndDate { get; set; }    
        public TimeSpan? StartTime { get; set; }  
        public TimeSpan? EndTime { get; set; }    
        public int? GenreId { get; set; }
        public int? MovieId { get; set; }
    }
}
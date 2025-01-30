using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Helpers.QueryObject
{
    public class ReviewQueryObject
    {
        public int? MovieId { get; set; }
        public string? AppUserId { get; set; }
    }
}
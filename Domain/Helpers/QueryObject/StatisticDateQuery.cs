using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Helpers.QueryObject
{
    public class StatisticDateQuery
    {
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}
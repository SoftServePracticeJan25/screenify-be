using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.StatisticsDtos
{
    public class StatisticBestSoldMoviesDto
    {
        public required int MovieId { get; init; }
        public required int TicketsSold { get; init; }
    }
}
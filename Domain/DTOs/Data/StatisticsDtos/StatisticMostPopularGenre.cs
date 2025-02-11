using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.StatisticsDtos
{
    public class StatisticMostPopularGenre
    {
        public required int GenreId { get; init; }
        public required int TicketsSold { get; init; }
    }
}
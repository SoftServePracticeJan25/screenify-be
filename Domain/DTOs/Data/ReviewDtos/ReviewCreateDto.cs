using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.ReviewDtos
{
    public class ReviewCreateDto
    {
        public required int Rating { get; init; }
        public required string Comment { get; init; }
        public required DateTime CreationTime { get; init; }
        public required int Likes { get; init; }

        public required int MovieId { get; init; }
        public required string AppUserId { get; set; }
    }
}
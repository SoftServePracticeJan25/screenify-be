using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.ReviewDtos
{
    public class ReviewPatchDto
    {
        public int? Rating { get; set; }
        public string? Comment { get; set; }
    }

}

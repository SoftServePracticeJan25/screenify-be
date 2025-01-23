using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data
{
    public class TransactionDto
    {
        public decimal Sum { get; set; }
        public DateTime CreationTime { get; set; }
        public string? AppUserId { get; set; } // must be string type
    }
}

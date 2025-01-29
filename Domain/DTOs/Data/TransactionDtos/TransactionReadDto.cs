using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.TransactionDtos
{
    public class TransactionReadDto
    {
        public required int Id { get; set; }
        public required decimal Sum { get; init; }
        public required DateTime CreationTime { get; init; }
    }
}
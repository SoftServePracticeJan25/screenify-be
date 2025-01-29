using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.ActorRoleDtos
{
    public class ActorRoleReadDto
    {
        public required int Id { get; set; }
        public required string RoleName { get; init; }
    }
}
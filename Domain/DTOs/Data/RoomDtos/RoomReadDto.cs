﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.RoomDtos
{
    public class RoomReadDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int SeatsAmount { get; set; }
        public required string CinemaTypeName { get; set; }
    }
}

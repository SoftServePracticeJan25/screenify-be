﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data.GenreDtos
{
    public class GenreDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}

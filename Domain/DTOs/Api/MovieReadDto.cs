﻿using Domain.DTOs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Api
{
    public class MovieReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public List<GenreDto> Genres { get; set; }
        public List<MovieActorDto> Actors { get; set; }
    }
}

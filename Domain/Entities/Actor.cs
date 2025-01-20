﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public string? PhotoUrl { get; set; } = string.Empty;
    }
}
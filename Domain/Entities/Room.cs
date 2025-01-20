﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int MyProperty { get; set; }

        public int? CinemaTypeId { get; set; }
        public CinemaType? CinemaType { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Persistance.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nick { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}

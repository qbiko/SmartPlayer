﻿using SmartPlayerAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Persistance.Models
{
    public class AccelerometerAndGyroscopeResult : IAggregate
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public DateTimeOffset TimeOfOccur { get; set; }
        public int PlayerInGameId { get; set; }
        public virtual PlayerInGame PlayerInGame { get; set; }
    }
}

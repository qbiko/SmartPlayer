using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Persistance.Models
{
    public class Mock
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Text { get; set; }
        public double Number { get; set; }
    }
}

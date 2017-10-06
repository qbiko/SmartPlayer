using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.ViewModels
{
    public class ClubViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateOfCreate { get; set; }
    }
}

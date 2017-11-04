using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.ViewModels
{

    public class PlayerInClubViewModelOut : PlayerInClubViewModelIn
    {
        public int Id { get; set; }
    }
    public class PlayerInClubViewModelIn
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public int HeightOfUser { get; set; }
        public int WeightOfUser { get; set; }
    }
}

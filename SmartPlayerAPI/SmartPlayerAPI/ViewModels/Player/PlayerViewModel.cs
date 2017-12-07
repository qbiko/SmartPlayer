using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.ViewModels
{
    public class PlayerViewModelIn
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public int HeightOfUser { get; set; }
        public int WeightOfUser { get; set; }
        public int ClubId { get; set; }
    }
    public class PlayerViewModelOut : PlayerViewModelIn
    {
        public int Id { get; set; }
    }

    public class UpdatePlayer
    {
        public int? HeightOfUser { get; set; }
        public int? WeightOfUser { get; set; }
        public int PlayerId { get; set; }
    }

}

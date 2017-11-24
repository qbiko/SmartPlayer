using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.ViewModels
{
    public class PulseSensorIn
    {
        public int PlayerId { get; set; }
        public int Value { get; set; }
        public int GameId { get; set; }
    }

    public class PulseSensorOut : PulseSensorIn
    {
        public int Id { get; set; }
        public DateTimeOffset TimeOfOccur { get; set; }
    }

    public class PulseInMatch
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
    }

    public class PulseSensorViewModel
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset TimeOfOccur { get; set; }
    }

    public class PulseSensorInBatch
    {
        public int Value { get; set; }
        public double TimeOfOccurLong { get; set; }
    }

    public class PulseSensorOutBatch : PulseSensorInBatch
    {
        public DateTimeOffset TimeOfOccur { get; set; }
        public int Id { get; set; }
    }
    public class PulseSensorBatch<T>
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public List<T> PulseList { get; set; } = new List<T>();
    }
}

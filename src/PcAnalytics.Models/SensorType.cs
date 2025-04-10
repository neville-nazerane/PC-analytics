using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.Models
{

    [Index(nameof(Name))]
    public class SensorType
    {

        public int Id { get; set; }

        public required string Name { get; set; }

        public IEnumerable<Record>? Records { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.Models
{

    public class SensorInput
    {
        public required string HardwareName { get; set; }

        public required string SensorGroupName { get; set; }

        public required string SensorType { get; set; }
        
        public required DateTime CreatedOn { get; set; }

        public float? Value { get; set; }
    }
}

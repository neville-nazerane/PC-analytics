using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.Models
{
    public class SensorGroup
    {

        public int Id { get; set; }

        public string? Name { get; set; }

        public int HardwareId { get; set; }

        [Required]
        public Hardware? Hardware { get; set; }

    }
}

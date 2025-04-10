using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.Models.Entities
{

    [Index(nameof(Name))]
    public class Hardware
    {

        public int Id { get; set; }

        public required string Name { get; set; }

        public int ComputerId { get; set; }

        [Required]
        public Computer? Computer { get; set; }

        public IEnumerable<SensorGroup>? SensorGroups { get; set; }

    }
}

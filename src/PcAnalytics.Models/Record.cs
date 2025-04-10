using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.Models
{
    public class Record
    {
        public Record()
        {
            CreatedOn = DateTime.UtcNow;
        }

        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public int SensorTypeId { get; set; }

        [Required]
        public SensorType? SensorType { get; set; }

        public int SensorGroupId { get; set; }

        [Required]
        public SensorGroup? SensorGroup { get; set; }

    }
}

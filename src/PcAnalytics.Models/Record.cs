using System;
using System.Collections.Generic;
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

        public int ComputerId { get; set; }
        public Computer? Computer { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;
using PcAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.ServerLogic
{


    public class AppDbContext : DbContext
    {

        public DbSet<Computer> Computers { get; set; }

        public DbSet<Hardware> Hardwares { get; set; }

        public DbSet<SensorGroup> SensorGroups { get; set; }

        public DbSet<SensorType> SensorTypes { get; set; }

        public DbSet<Record> Records { get; set; }



    }
}

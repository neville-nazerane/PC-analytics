using Microsoft.EntityFrameworkCore;
using PcAnalytics.Models;

namespace PcAnalytics.ClientApi.Service
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<SensorInput> SensorInputs { get; set; }

    }
}

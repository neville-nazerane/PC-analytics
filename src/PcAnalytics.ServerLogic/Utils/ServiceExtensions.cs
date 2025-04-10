using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.ServerLogic.Utils
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddLogicServices(this IServiceCollection services,
                                                          IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(configuration["sqlConnString"]));
            return services;
        }

    }
}

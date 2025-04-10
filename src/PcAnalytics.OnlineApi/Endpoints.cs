using Microsoft.EntityFrameworkCore;
using PcAnalytics.Models;
using PcAnalytics.ServerLogic;

namespace PcAnalytics.OnlineApi
{
    public static class Endpoints
    {


        public static async Task AddIncomingAsync(HttpRequest request,
                                                  IncomingSensorInput input,
                                                  AppDbContext dbContext,
                                                  CancellationToken cancellationToken = default)
        {

            if (request.Headers.TryGetValue("computerSerial", out var computerIds))
            {
                string serial = computerIds.First() ?? throw new Exception("No computer Id");

                int computerId = await dbContext.Computers
                                                .Where(c => c.Identifier == serial)
                                                .Select(c => c.Id)
                                                .SingleOrDefaultAsync(cancellationToken: cancellationToken);


            }
        }

    }
}

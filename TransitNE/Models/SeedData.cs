using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransitNE.Data;
using System;
using System.Linq;

namespace TransitNE.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TransitNEContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<TransitNEContext>>()))
            {
                if (context.Routes.Any())
                {
                    return; //DB has been seeded
                }
                context.Routes.AddRange(
                    new Routes
                    {
                        Name = "Route 1"
                    },
                    new Routes
                    {
                        Name = "Route 2"
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}

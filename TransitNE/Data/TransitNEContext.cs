using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TransitNE.Models;

namespace TransitNE.Data
{
    public class TransitNEContext : DbContext
    {
        public TransitNEContext (DbContextOptions<TransitNEContext> options)
            : base(options)
        {
        }

        public DbSet<TransitNE.Models.Routes> Routes { get; set; } = default!;
    }
}

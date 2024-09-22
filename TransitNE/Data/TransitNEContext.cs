using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TransitNE.Models;

namespace TransitNE.Data;

public class TransitNEContext : IdentityDbContext<TransitNEUser>
{
    public TransitNEContext(DbContextOptions<TransitNEContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }

public DbSet<TransitNE.Models.TrainModel> TrainModel { get; set; } = default!;
}

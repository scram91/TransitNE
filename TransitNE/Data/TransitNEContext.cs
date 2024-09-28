using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TransitNE.Data;

public class TransitNEContext : IdentityDbContext
{
    public TransitNEContext(DbContextOptions<TransitNEContext> options)
        : base(options)
    {
    }

    public DbSet<TransitNEUser> transitNEUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

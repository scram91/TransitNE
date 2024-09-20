using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
}

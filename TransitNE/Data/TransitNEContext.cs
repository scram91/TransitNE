using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using TransitNE.Models;

namespace TransitNE.Data;

public class TransitNEContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
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

    public DbSet<TransitNE.Models.TrainModel> TrainModel { get; set; } = default!;

    public DbSet<TransitNE.Models.TransitNEUser> TransitNEUsers { get; set; } = default!;

    public DbSet<TransitNE.Models.RailScheduleModel> RailScheduleModels { get; set; } = default!;

    public DbSet<TransitNE.Models.BusTrolleyModel> BusTrolleyModels { get; private set; } = default!;
    public DbSet<TransitNE.Models.BusTrolleySchedule> BusTrolleySchedules { get; private set; } = default!;

    public DbSet<TransitNE.Models.SeptaRailLines> SeptaRailLinesModels { get; internal set; } = default!;

    public DbSet<TransitNE.Models.BusTrolleyRouteModel> BusTrolleyRouteModels { get; protected set; } = default!;
    public DbSet<TransitNE.Models.StopModel> StopModels { get; set; }
    public DbSet<TransitNE.Models.BusInputModel> BusInputModels { get; protected set;} = default!;
}

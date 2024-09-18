using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransitNE.Models;
using Microsoft.AspNetCore.Identity;
using TransitNE.Data;
using TransitNE.Areas.Identity.Data;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TransitNEContextConnection") ?? throw new InvalidOperationException("Connection string 'TransitNEContextConnection' not found.");

builder.Services.AddDbContext<TransitNEContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<TransitNEUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<TransitNEContext>();

builder.Services.AddControllersWithViews();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

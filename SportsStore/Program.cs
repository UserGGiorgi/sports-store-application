using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
  
builder.Services.AddDbContext<StoreDbContext>(opts =>
{
     opts.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
 });

var app = builder.Build();

app.UseStaticFiles();

app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);
app.Run();

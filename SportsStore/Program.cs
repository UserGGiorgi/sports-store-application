using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddDbContext<StoreDbContext>(opts =>
{
     opts.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
 });

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page{productPage:int}",
    defaults: new { controller = "Home", Action = "Index", productPage = 1 });
app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);
app.Run();

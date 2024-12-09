using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart>(SessionCart.GetCart);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<StoreDbContext>(opts =>
{
     opts.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
 });

var app = builder.Build();

app.UseStaticFiles();
app.UseSession();

app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page{productPage:int}",
    defaults: new { controller = "Home", Action = "Index", productPage = 1 });

app.MapControllerRoute(
     name: "categoryPage",
     pattern: "{category}/Page{productPage:int}",
     defaults: new { Controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "category",
    pattern: "Products/{category}",
    defaults: new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute(
    name: "shoppingCart",
    pattern: "Cart",
    defaults: new { Controller = "Cart", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "/",
    defaults: new { Controller = "Home", action = "Index" });

app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);
app.Run();

using TwistFood.DataAccess.Interfaces;
using TwistFood.DataAccess.Repositories;
using TwistFood.Service.Interfaces.Accounts;
using TwistFood.Service.Interfaces.Admins;
using TwistFood.Service.Interfaces.Categories;
using TwistFood.Service.Interfaces.Common;
using TwistFood.Service.Interfaces.Delivers;
using TwistFood.Service.Interfaces.Discounts;
using TwistFood.Service.Interfaces.Operators;
using TwistFood.Service.Interfaces.Orders;
using TwistFood.Service.Interfaces.Products;
using TwistFood.Service.Interfaces;
using TwistFood.Service.Security;
using TwistFood.Service.Services.Accounts;
using TwistFood.Service.Services.Admins;
using TwistFood.Service.Services.Categories;
using TwistFood.Service.Services.Common;
using TwistFood.Service.Services.Delivers;
using TwistFood.Service.Services.Discounts;
using TwistFood.Service.Services.Operators;
using TwistFood.Service.Services.Orders;
using TwistFood.Service.Services.Products;
using TwistFood.Service.Services;
using Microsoft.EntityFrameworkCore;
using TwistFood.Api.DbContexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

string connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IPaginatorService, PaginatorService>();
builder.Services.AddScoped<IProductService, ProductService>();


var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
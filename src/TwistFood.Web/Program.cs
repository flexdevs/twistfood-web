using Microsoft.EntityFrameworkCore;
using System.Net;
using TwistFood.Api.DbContexts;
using TwistFood.DataAccess.Interfaces;
using TwistFood.DataAccess.Repositories;
using TwistFood.Service.Interfaces;
using TwistFood.Service.Interfaces.Accounts;
using TwistFood.Service.Interfaces.Categories;
using TwistFood.Service.Interfaces.Common;
using TwistFood.Service.Interfaces.Discounts;
using TwistFood.Service.Interfaces.Orders;
using TwistFood.Service.Interfaces.Products;
using TwistFood.Service.Security;
using TwistFood.Service.Services;
using TwistFood.Service.Services.Accounts;
using TwistFood.Service.Services.Categories;
using TwistFood.Service.Services.Common;
using TwistFood.Service.Services.Discounts;
using TwistFood.Service.Services.Orders;
using TwistFood.Service.Services.Products;
using TwistFood.Web.Configurations.LayerConfigarions;
using TwistFood.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddWeb(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

string connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IVerifyPhoneNumberService, VerifyPhoneNumberService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDeteilsService, OrderDeteilService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();


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
app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        context.HttpContext.Response.Redirect("accounts/login");
    }
});
app.UseMiddleware<TokenRedirectMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "admins/{controller=products}/{action=Index}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();

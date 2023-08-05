using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Areas.Identity.Data;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BookShopContextConnection") ?? throw new InvalidOperationException("Connection string 'BookShopContextConnection' not found.");


builder.Services.AddDbContext<BookShopContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BookShopContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IBookRepository, BookRepository>();
//builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

// Cấu hình phân quyền
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

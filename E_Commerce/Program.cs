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

builder.Services.AddDefaultIdentity<ApplicationUser>(/*options => options.SignIn.RequireConfirmedAccount = true*/)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BookShopContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDistributedMemoryCache(); // Chọn cách lưu trữ phiên (ở đây dùng bộ nhớ)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian tồn tại của phiên
});

builder.Services.AddAutoMapper(typeof(Program));    
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddHttpContextAccessor();
// Cấu hình phân quyền
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=GetAll}/{id?}");

app.Run();

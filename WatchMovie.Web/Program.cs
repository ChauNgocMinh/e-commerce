using WatchMovie.Infastructure.DataSeed;
using WatchMovie.Infastructure.Install;
using WatchMovie.Infastructure.Persistence;
using System;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true);

builder.Services.InstallServicesInAssembly(builder.Configuration); 

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1024 * 1024 * 1024;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        DbInitializer.InitializeDatabase(app);

        await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

app.Run();

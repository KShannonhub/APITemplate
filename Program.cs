using APITemplate;
using APITemplate.Filters;
using APITemplate.Infrastructure;
using APITemplate.Models;
using APITemplate.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        // These should be the defaults, but we can be explicit:
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
        options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;

    });


// add routing options of lowercase urls
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddMvc(options => options.Filters.Add<LinkRewritingFilter>());

builder.Services.Configure<HotelInfo>(
    builder.Configuration.GetSection("Info"));

builder.Services.Configure<HotelOptions>(builder.Configuration);

builder.Services.AddDbContext<HotelApiDbContext>(
    options => options.UseInMemoryDatabase("HotelApi"));

builder.Services.AddScoped<IRoomService, DefaultRoomService>();
builder.Services.AddScoped<IOpeningService, DefaultOpeningService>();
builder.Services.AddScoped<IBookingService, DefaultBookingService>();
builder.Services.AddScoped<IDateLogicService, DefaultDateLogicService>();

builder.Services.AddAutoMapper(
    options => options.AddProfile<MappingProfile>());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


InitializeDatabase(app);
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Define the InitializeDatabase method
void InitializeDatabase(WebApplication host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            SeedData.InitializeAsync(services).Wait();
            // Add your database initialization logic here
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}

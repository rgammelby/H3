using LagerSystemApi.CustomLogger;
using LagerSystemApi.Interfaces;
using LagerSystemApi.Mappings;
using LagerSystemApi.Repository;
using LagerSystemApi.Services;
using Microsoft.EntityFrameworkCore;

namespace LagerSystemApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Logger in dependency
            builder.Logging.AddProvider(new FileLoggerProvider("C://Users/rune1/Desktop/LagerStyringsLog.txt"));

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

            // Enforce HTTPS
            builder.Services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443; // Default HTTPS port
            });

            // Register DbContext with SQL Server
            builder.Services.AddDbContext<Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));


            // Registers all repositories with an instance of DBcontext
            builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<PasswordService>();
            builder.Services.AddScoped<IDeviceOverviewRepository, DeviceOverviewRepository>();
            builder.Services.AddScoped<IStatusTypeRepository, StatusTypeRepository>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();

            // Registers all services with an instance of their repositories
            //builder.Services.AddScoped<IActivityService, ActivityService>();
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<IDeviceOverviewService, DeviceOverviewService>();
            builder.Services.AddScoped<IStatusTypeService, StatusTypeService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IImageService, ImageService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173") // Allow React frontend
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowReactApp"); // Apply the CORS policy
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

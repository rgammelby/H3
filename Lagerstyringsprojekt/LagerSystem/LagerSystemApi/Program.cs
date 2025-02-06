using LagerSystemApi.Mappings;
using LagerSystemApi.Repository;
using LagerSystemApi.Services;
using Microsoft.EntityFrameworkCore;
using static LagerSystemApi.Repository.DeviceOverviewRepository;

namespace LagerSystemApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
            //builder.Services.AddScoped<IDeviceOverviewRepository, DeviceOverviewRepository>();

            // Registers all services with an instance of their repositories
            builder.Services.AddScoped<IActivityService, ActivityService>();
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILogService, LogService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

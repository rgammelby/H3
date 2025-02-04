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

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //  Configure DbContext to use SQL Server and ensure migrations work
            builder.Services.AddDbContext<Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly("LagerSystemApi")) // Ensure migrations go to API
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information) // Debug logs
            );


            // Registers all repositories with an instance of DBcontext
            builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<PasswordService>();

            // Registers all services with an instance of their repositories
            builder.Services.AddScoped<IActivityService, ActivityService>();
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILogService, LogService>();

            var app = builder.Build();

            //  Ensure database is created and migrated at startup
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                dbContext.Database.Migrate(); // This will auto-migrate DB at startup
            }

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

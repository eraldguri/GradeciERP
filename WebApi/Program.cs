
using Application;
using Infrastructure;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Gradeci ERP App", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers();

            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddJwtAuthentication(builder.Services.GetJwtSettings(builder.Configuration));

            builder.Services.AddApplicationServices();

            var app = builder.Build();

            await app.Services.AddDatabaseInitializerAsync();

            app.UseHttpsRedirection();

            app.UseCors("Gradeci ERP App");

            app.UseInfrastructure();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}

using JobManagement.API.DependencyInjection;
using JobManagement.API.Hubs;
using JobManagement.Infrastructure.DependencyInjection;
using JobManagement.Persistence.DependencyInjection;

namespace JobManagement.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureApplication(builder.Configuration);
            builder.Services.ConfigurePersistence(builder.Configuration);
            builder.Services.ConfigureInfrastructure(builder.Configuration);
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Services.SeedLocalExecutables();
            await app.Services.RunPersistence(app.Lifetime.ApplicationStopping);

            app.UseCors();
            //app.UseHttpsRedirection();
            app.MapHub<JobManagerHub>("/api/JobManagerHub");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

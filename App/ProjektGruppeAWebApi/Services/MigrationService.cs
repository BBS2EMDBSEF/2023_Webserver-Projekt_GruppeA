using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjektGruppeWebApi;
using System;


namespace ProjektGruppeAWebApi.Services
{
    public class MigrationService 
    {
        private readonly IHost _host;

        public MigrationService(IHost host)
        {
            _host = host;
        }

        public void ApplyMigrations()
        {
            var serviceProvider = _host.Services.CreateScope().ServiceProvider;

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ProjektGruppeAContext>(); // Replace with your actual DbContext type
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<MigrationService>>();

                int maxAttempts = 3;
                int attempts = 0;

                while (attempts < maxAttempts)
                {
                    try
                    {
                        dbContext.Database.Migrate();
                        logger.LogInformation("Migrations applied successfully.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        attempts++;
                        logger.LogError($"Migration attempt {attempts} failed: {ex.Message}");
                        if (attempts >= maxAttempts)
                        {
                            logger.LogError("Maximum migration attempts reached. The application will now exit.");
                            Environment.Exit(1);
                        }
                    }
                }
            }
        }
    }
}

namespace ProjektGruppeAWebApi.Services.BackgroundServices
{
    public class MigrationBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public MigrationBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();
                migrationService.ApplyMigrations();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

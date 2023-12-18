using ArchiAgile.Server.Data;
using ArchiAgile.Server.Interfaces;

namespace ArchiAgile.Server.Services
{
    public class BackgroundAppService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BackgroundAppService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
                cacheService.Load();

                //while (!stoppingToken.IsCancellationRequested)
                //{
                //    await Task.Delay(10);
                //}
            }
        }
    }
}

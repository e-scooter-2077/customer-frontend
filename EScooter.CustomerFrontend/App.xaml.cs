using EScooter.CustomerFrontend.Data;
using EScooter.CustomerFrontend.Data.Mocks;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace EScooter.CustomerFrontend
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var container = new ServiceCollection();
            ConfigureServices(container);
            _serviceProvider = container.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<HomeView>();

            services.AddSingleton<MockDataAccess>();
            services.AddSingleton<IQueryService>(p => p.GetRequiredService<MockDataAccess>());
            services.AddSingleton<IRentService>(p => p.GetRequiredService<MockDataAccess>());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _serviceProvider.GetRequiredService<HomeView>().Show();
        }
    }
}

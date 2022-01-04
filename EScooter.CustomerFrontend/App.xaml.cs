using Azure.DigitalTwins.Core;
using Azure.Identity;
using EScooter.CustomerFrontend.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
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
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddUserSecrets<App>(optional: true)
                .Build();

            var container = new ServiceCollection();
            ConfigureServices(container, configuration);
            _serviceProvider = container.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HomeView>();

            ////services.AddSingleton<MockDataAccess>();
            ////services.AddSingleton<IQueryService>(p => p.GetRequiredService<MockDataAccess>());
            ////services.AddSingleton<IRentService>(p => p.GetRequiredService<MockDataAccess>());
            services.AddSingleton<IQueryService, DigitalTwinQueryService>();

            services.AddHttpClient<IRentService, HttpRentService>(client =>
            {
                client.BaseAddress = new Uri(configuration["RentServiceUri"]);
            });

            services.AddSingleton(_ => CreateDigitalTwinsClient(configuration));
            services.AddSingleton(_ => new HttpClient());
        }

        private DigitalTwinsClient CreateDigitalTwinsClient(IConfiguration configuration)
        {
            var instanceUri = new Uri(configuration["DigitalTwinsUri"]);
            var credentials = new DefaultAzureCredential();
            return new DigitalTwinsClient(instanceUri, credentials);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _serviceProvider.GetRequiredService<HomeView>().Show();
        }
    }
}

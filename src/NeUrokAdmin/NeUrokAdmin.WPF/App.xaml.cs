using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NeUrokAdmin.Infrastructure.Persistance;
using NeUrokAdmin.WPF.Views.ModalWindows;
using NeUrokAdmin.WPF.Views.ModalWindows.ViewModals;

namespace NeUrokAdmin.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        private readonly IConfiguration _configuration;

        public App()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();


            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                })
                .Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(_configuration.GetConnectionString("Main"),
                ServerVersion.Parse("8.0.36-mysql")));

            services.AddTransient<MainWindow>();

            services.AddTransient<LoginViewModal>();
            services.AddTransient<LoginWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
            var viewModel = _host.Services.GetRequiredService<LoginViewModal>();

            loginWindow.DataContext = viewModel;
            loginWindow.Show();


            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            base.OnExit(e);
        }
    }
}
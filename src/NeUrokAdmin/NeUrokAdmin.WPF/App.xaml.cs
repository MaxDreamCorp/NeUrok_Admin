using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NeUrokAdmin.Application.Features.Authorization.Commands;
using NeUrokAdmin.Domain.Interfaces;
using NeUrokAdmin.Domain.Interfaces.Repositories;
using NeUrokAdmin.Infrastructure.Persistance;
using NeUrokAdmin.Infrastructure.Persistance.Repositories;
using NeUrokAdmin.Infrastructure.Services.Security;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows.ViewModels;
using NeUrokAdmin.WPF.Views.ModalWindows;
using NeUrokAdmin.WPF.Views.ModalWindows.ViewModels;
using NeUrokAdmin.WPF.Views.UserControls;
using NeUrokAdmin.WPF.Views.UserControls.ViewModels;
using NeUrokAdmin.WPF.Views.Windows.ViewModels;

namespace NeUrokAdmin.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
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

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegistrateCommand).Assembly));

            services.AddSingleton<IHasher, SHA512Hasher>();
            services.AddSingleton(provider => new NavigationService(provider));
            services.AddSingleton<IDialogService, WindowsDialogService>();

            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MainWindow>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<LoginWindow>();
            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<RegistrationWindow>();

            services.AddTransient<ClientViewViewModel>();
            services.AddTransient<ClientsView>();

            services.AddTransient<StandartCardViewModel>();
            services.AddTransient<ClientCardViewModel>();
            services.AddTransient<MultiplySelectorViewModel>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IClientStatusRepository, ClientStatusRepository>();

        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
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
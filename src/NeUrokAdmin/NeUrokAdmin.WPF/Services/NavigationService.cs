using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace NeUrokAdmin.WPF.Services
{
    public class NavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetWindow<T>() where T : Window => _serviceProvider.GetRequiredService<T>();

        public T GetViewModel<T>() where T : ObservableObject => _serviceProvider.GetRequiredService<T>();
    }
}

using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using NeUrokAdmin.Application.Interfaces;
using NeUrokAdmin.WPF.Enums;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.CardWindows.ViewModels;
using NeUrokAdmin.WPF.Views.ModalWindows;
using NeUrokAdmin.WPF.Views.ModalWindows.ViewModels;

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

        public async Task ShowCard<TViewModel>(TViewModel contentViewModel, object owner, OperationType operationType, string title) where TViewModel : BaseCardViewModel
        {
            if (contentViewModel is IAsyncInitializer initializer)
                await initializer.InitializeAsync();

            var wrapper = GetViewModel<StandartCardViewModel>();

            wrapper.CurrentCard = contentViewModel;
            wrapper.Title = title;
            wrapper.HeaderText = title;
            wrapper.OperationType = operationType;

            var window = new StandartCard
            {
                DataContext = wrapper,
                Owner = System.Windows.Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.DataContext == owner)
                ?? System.Windows.Application.Current.MainWindow
            };

            wrapper.Closed += window.Close;

            window.ShowDialog();
        }

        public async Task ShowMultiplySelector<TViewModel>(TViewModel contentViewModel, object owner, string title) where TViewModel : BaseMultiplySelectorViewModel
        {
            if (contentViewModel is IAsyncInitializer initializer)
                await initializer.InitializeAsync();

            var wrapper = GetViewModel<MultiplySelectorViewModel>();

            wrapper.CurrentSelector = contentViewModel;
            wrapper.Title = title;

            var window = new MultiplySelectorWindow
            {
                DataContext = wrapper,
                Owner = System.Windows.Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.DataContext == owner)
                ?? System.Windows.Application.Current.MainWindow
            };

            wrapper.Closed += window.Close;

            window.ShowDialog();
        }
    }
}

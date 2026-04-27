using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using NeUrokAdmin.WPF.Enums;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.CardWindows.ViewModels;

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

        public void ShowCard<TViewModel>(TViewModel contentViewModel, Window owner, OperationType operationType, string title) where TViewModel : BaseCardViewModel
        {
            var wrapper = GetViewModel<StandartCardViewModel>();

            wrapper.CurrentCard = contentViewModel;
            wrapper.Title = title;
            wrapper.HeaderText = title;
            wrapper.OperationType = operationType;

            var window = new StandartCard
            {
                DataContext = contentViewModel,
                Owner = owner
            };

            wrapper.Closed += window.Close;

            window.ShowDialog();
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Application.Interfaces;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.CardWindows.ViewModels
{
    public partial class ClientCardViewModel : BaseCardViewModel, IAsyncInitializer
    {
        [ObservableProperty]
        private ClientDTO? _client;

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public override Task Delete()
        {
            throw new NotImplementedException();
        }

        public override Task Ok()
        {
            throw new NotImplementedException();
        }
    }
}

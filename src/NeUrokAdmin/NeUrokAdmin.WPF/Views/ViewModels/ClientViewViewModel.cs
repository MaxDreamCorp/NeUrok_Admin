using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;

namespace NeUrokAdmin.WPF.Views.ViewModels
{
    public partial class ClientViewViewModel : ObservableObject
    {
        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;

        public ClientViewViewModel(IMediator mediator, NavigationService navigationService)
        {
            _mediator = mediator;
            _navigationService = navigationService;
        }

        [ObservableProperty]
        private ObservableCollection<ClientDTO> _clients = new ObservableCollection<ClientDTO>();

        [RelayCommand]
        public Task ClearSearchings()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public async Task Create()
        {
            var cardVM = new ClientCardViewModel(Enums.OperationType.Create, "Создать клиента");
            var card = _navigationService.GetWindow<ClientCard>();
            card.ViewModel = cardVM;
            card.ShowDialog();
            await PrintAll();
        }

        [RelayCommand]
        public Task Filter()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public async Task PrintAll()
        {
            var qry = new GetAllClientsQuery();

            Clients = new(await _mediator.Send(qry));
        }

        [RelayCommand]
        public Task QuickSearch()
        {
            throw new NotImplementedException();
        }
    }
}

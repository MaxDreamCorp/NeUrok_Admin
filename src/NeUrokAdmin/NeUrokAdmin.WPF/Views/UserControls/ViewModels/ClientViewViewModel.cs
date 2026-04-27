using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows.ViewModels;
using NeUrokAdmin.WPF.Views.Elements.ViewModels;

namespace NeUrokAdmin.WPF.Views.UserControls.ViewModels
{
    public partial class ClientViewViewModel : BaseDataViewModel
    {
        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;

        public ClientViewViewModel(IMediator mediator, NavigationService navigationService)
        {
            _mediator = mediator;
            _navigationService = navigationService;
        }

        [ObservableProperty]
        private ObservableCollection<ClientDTO> _clients = new();


        public override Task ClearSearchings()
        {
            throw new NotImplementedException();
        }

        public override async Task Create()
        {
            var clientVM = _navigationService.GetViewModel<ClientCardViewModel>();
            clientVM.IsEditable = true;

            await _navigationService.ShowCard(clientVM, this, Enums.OperationType.Read, "Создать клиента");
        }

        public override Task Filter()
        {
            throw new NotImplementedException();
        }

        public override async Task PrintAll()
        {
            var qry = new GetAllClientsQuery();

            Clients = new(await _mediator.Send(qry));
        }

        public override Task QuickSearch()
        {
            throw new NotImplementedException();
        }
    }
}

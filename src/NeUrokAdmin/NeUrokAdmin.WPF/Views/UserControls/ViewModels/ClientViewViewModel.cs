using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.Elements.ViewModels;

namespace NeUrokAdmin.WPF.Views.UserControls.ViewModels
{
    public partial class ClientViewViewModel : BaseDataViewModel
    {
        private readonly IMediator _mediator;

        public ClientViewViewModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ObservableProperty]
        private ObservableCollection<ClientDTO> _clients = new();


        public override Task ClearSearchings()
        {
            throw new NotImplementedException();
        }

        public override Task Create()
        {
            throw new NotImplementedException();
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

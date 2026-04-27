using CommunityToolkit.Mvvm.ComponentModel;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Application.Interfaces;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.CardWindows.ViewModels
{
    public partial class ClientCardViewModel : BaseCardViewModel, IAsyncInitializer
    {
        private readonly IMediator _mediator;

        [ObservableProperty]
        private ClientDTO? _client;

        [ObservableProperty]
        private List<string> _clientStatuses = new();

        public ClientCardViewModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task InitializeAsync()
        {
            var clientStatuses = await _mediator.Send(new GetAllClientsStatusesQuery());
            ClientStatuses = clientStatuses.Select(cs => cs.Status).ToList();
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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Application.Interfaces;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.ModalWindows.ViewModels;

namespace NeUrokAdmin.WPF.Views.CardWindows.ViewModels
{
    public partial class ClientCardViewModel : BaseCardViewModel, IAsyncInitializer
    {
        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;
        [ObservableProperty]
        private ClientDTO? _client;

        [ObservableProperty]
        private List<string> _clientStatuses = new();

        public ClientCardViewModel(IMediator mediator, NavigationService navigationService)
        {
            _mediator = mediator;
            _navigationService = navigationService;
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

        [RelayCommand]
        public async Task Tr()
        {
            var vm = new CourseSelectorViewModel(
                new()
                {
                    new(1, "ffg"),
                    new(2, "ffffffffg"),
                }, new());
            await _navigationService.ShowMultiplySelector(vm, this, "");
        }
    }
}

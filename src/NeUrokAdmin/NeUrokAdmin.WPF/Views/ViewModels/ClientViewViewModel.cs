using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;

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
        public Task Create()
        {
            throw new NotImplementedException();
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

using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.ClientOperations.Handlers.Queries
{
    public class GetUpcomingBirthdaysQueryHandler : IRequestHandler<GetUpcomingBirthdaysQuery, UpcomingBirthdaysDTO>
    {
        private readonly IClientRepository _clientRepository;

        public GetUpcomingBirthdaysQueryHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<UpcomingBirthdaysDTO> Handle(GetUpcomingBirthdaysQuery request, CancellationToken cancellationToken)
        {
            Dictionary<string, string> todaysRes = new();
            Dictionary<string, string> tomorrowsRes = new();
            Dictionary<string, string> postTomorrowsRes = new();

            var now = DateOnly.FromDateTime(DateTime.Now);

            var todays = await _clientRepository.GetByBirthDayAsync(now, cancellationToken);
            foreach (var item in todays)
            {
                if (item.BirthDate.HasValue)
                    todaysRes.Add(item.ChildFullname,
                        GetNumberWithSuf(now.Year - item.BirthDate.Value.Year));
            }

            var tomorrows = await _clientRepository.GetByBirthDayAsync(now.AddDays(1), cancellationToken);
            foreach (var item in tomorrows)
            {
                if (item.BirthDate.HasValue)
                    tomorrowsRes.Add(item.ChildFullname,
                        GetNumberWithSuf(now.AddDays(1).Year - item.BirthDate.Value.Year));
            }

            var postTomorrows = await _clientRepository.GetByBirthDayAsync(now.AddDays(2), cancellationToken);
            foreach (var item in postTomorrows)
            {
                if (item.BirthDate.HasValue)
                    postTomorrowsRes.Add(item.ChildFullname,
                        GetNumberWithSuf(now.AddDays(2).Year - item.BirthDate.Value.Year));
            }

            return new UpcomingBirthdaysDTO(
                todaysRes,
                tomorrowsRes,
                postTomorrowsRes);
        }

        private string GetNumberWithSuf(int a)
        {
            string res = a.ToString();
            if (a % 10 == 1)
                res += " год";
            else if (a % 10 <= 4)
                res += " года";
            else
                res += " лет";
            return res;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeUrokAdmin.Domain.DTOs.SearchDTOs
{
    public record SubscriptionSearchDTO(
        int? Id = null,
        string? Name = null,
        string? ClassesType = null,
        decimal? Cost = null,
        int? ClassesAmount = null,
        int? IdFrom = null,
        int? IdTo = null,
        decimal? CostFrom = null,
        decimal? CostTo = null,
        int? ClassesAmountFrom = null,
        int? ClassesAmountTo = null,
        List<int>? ClassesTypeIds = null,
        List<ClassesTypeDTO>? ClassesTypes = null
    );
}

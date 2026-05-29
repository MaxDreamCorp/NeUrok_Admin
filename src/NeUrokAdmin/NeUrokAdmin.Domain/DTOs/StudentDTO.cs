using NeUrokAdmin.Domain.Enums;

namespace NeUrokAdmin.Domain.DTOs
{
    public record StudentDTO(
        int Id,
        ClientDTO Client,
        List<StudentSubscriptionDTO> StudentSubscriptions)
    {
        public List<StudentSubscriptionDTO> ActiveSubscriptions => StudentSubscriptions.Where(ss => ss.SubscriptionStatus.Id == (int)SubscriptionStatusEnum.Active).ToList();
        public int ActiveSubscriptionsCount
        {
            get => StudentSubscriptions.Count(ss =>
            ss.SubscriptionStatus.Id == (int)SubscriptionStatusEnum.Active);
        }

        public int PaidCount
        {
            get => StudentSubscriptions.Count(ss =>
            ss.SubscriptionStatus.Id == (int)SubscriptionStatusEnum.Active &&
            ss.IsPaid);
        }

        public int NotPaidCount
        {
            get => StudentSubscriptions.Count(ss =>
            ss.SubscriptionStatus.Id == (int)SubscriptionStatusEnum.Active &&
            !ss.IsPaid);
        }
    }
}

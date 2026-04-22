using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Domain.Entities;

public partial class StudentSubscription
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int SubscriptionId { get; set; }

    public sbyte IsPaid { get; set; }

    public int CourseId { get; set; }

    public int SubscriptlonStatusId { get; set; }

    public DateOnly SubscriptionStartDate { get; set; }

    public DateOnly SubscriptionFinishDate { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Subscribtion Subscription { get; set; } = null!;

    public virtual SubscriptlonStatus SubscriptlonStatus { get; set; } = null!;
}

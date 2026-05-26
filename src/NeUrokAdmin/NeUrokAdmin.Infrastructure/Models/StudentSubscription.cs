using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Infrastructure.Models;

public partial class StudentSubscription
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int ClassesTypeId { get; set; }

    public decimal Cost { get; set; }

    public int ClassesAmount { get; set; }

    public sbyte IsPaid { get; set; }

    public int CourseId { get; set; }

    public int SubscriptlonStatusId { get; set; }

    public DateOnly SubscriptionStartDate { get; set; }

    public DateOnly SubscriptionFinishDate { get; set; }

    public virtual ClassType ClassesType { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();

    public virtual Student Student { get; set; } = null!;

    public virtual SubscriptlonStatus SubscriptlonStatus { get; set; } = null!;
}

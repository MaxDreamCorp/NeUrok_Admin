namespace NeUrokAdmin.Domain.Entities;

public partial class StudentSubscription
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int ClassesTypeId { get; set; }

    public decimal Cost { get; set; }

    public int ClassesAmount { get; set; }

    public virtual ClassType ClassesType { get; set; } = null!;

    public sbyte IsPaid { get; set; }

    public int CourseId { get; set; }

    public int SubscriptlonStatusId { get; set; }

    public DateOnly SubscriptionStartDate { get; set; }

    public DateOnly SubscriptionFinishDate { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual SubscriptlonStatus SubscriptlonStatus { get; set; } = null!;

    private StudentSubscription() { }

    public static StudentSubscription Create(int id,
                                             int studentId,
                                             int classesTypeId,
                                             decimal cost,
                                             int classesAmount,
                                             sbyte isPaid,
                                             int courseId,
                                             int subscriptlonStatusId,
                                             DateOnly subscriptionStartDate,
                                             DateOnly subscriptionFinishDate)
    {
        return new StudentSubscription()
        {
            Id = id,
            StudentId = studentId,
            ClassesTypeId = classesTypeId,
            Cost = cost,
            ClassesAmount = classesAmount,
            IsPaid = isPaid,
            CourseId = courseId,
            SubscriptlonStatusId = subscriptlonStatusId,
            SubscriptionStartDate = subscriptionStartDate,
            SubscriptionFinishDate = subscriptionFinishDate,
        };
    }
}

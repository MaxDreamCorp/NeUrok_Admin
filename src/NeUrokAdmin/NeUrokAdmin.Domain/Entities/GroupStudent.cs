namespace NeUrokAdmin.Domain.Entities;

public partial class GroupStudent
{
    public int GroupId { get; set; }

    public int StudentId { get; set; }

    public int ActiveSubscriptionId { get; set; }

    public virtual StudentSubscription ActiveSubscription { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    private GroupStudent() { }

    public static GroupStudent Create(int groupId,
        int studentId,
        int studentSubscriptionId)
    {
        return new GroupStudent()
        {
            GroupId = groupId,
            StudentId = studentId,
            ActiveSubscriptionId = studentSubscriptionId
        };
    }
}

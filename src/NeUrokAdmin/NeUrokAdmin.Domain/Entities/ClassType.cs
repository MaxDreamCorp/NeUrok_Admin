namespace NeUrokAdmin.Domain.Entities;

public partial class ClassType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<StudentSubscription> StudentSubscribtions { get; set; } = new List<StudentSubscription>();
}

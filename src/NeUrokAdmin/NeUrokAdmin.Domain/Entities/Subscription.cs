namespace NeUrokAdmin.Domain.Entities;

public partial class Subscription
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ClassesTypeId { get; set; }

    public decimal Cost { get; set; }

    public int ClassesAmount { get; set; }

    public virtual ClassType ClassesType { get; set; } = null!;

    public virtual ICollection<StudentSubscription> StudentSubscriptions { get; set; } = new List<StudentSubscription>();

    private Subscription() { }

    public static Subscription Create(
        int id,
        string name,
        int classesTypeId,
        decimal cost,
        int classesAmount)
    {
        return new Subscription
        {
            Id = id,
            Name = name,
            ClassesTypeId = classesTypeId,
            Cost = cost,
            ClassesAmount = classesAmount
        };
    }
}

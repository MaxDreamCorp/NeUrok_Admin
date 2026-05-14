namespace NeUrokAdmin.Domain.Entities;

public partial class Student
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<StudentSubscription> StudentSubscriptions { get; set; } = new List<StudentSubscription>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    private Student() { }

    public static Student Create(int id,
        int clientId) =>
        new Student()
        {
            Id = id,
            ClientId = clientId,
        };
}

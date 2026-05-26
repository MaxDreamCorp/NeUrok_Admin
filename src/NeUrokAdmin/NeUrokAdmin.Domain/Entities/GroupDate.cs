namespace NeUrokAdmin.Domain.Entities;

public partial class GroupDate
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public DateTime Datetime { get; set; }

    public virtual Group Group { get; set; } = null!;

    private GroupDate() { }

    public static GroupDate Create(int id,
        int groupId,
        DateTime dateTime)
    {
        return new GroupDate
        {
            Id = id,
            GroupId = groupId,
            Datetime = dateTime
        };
    }
}

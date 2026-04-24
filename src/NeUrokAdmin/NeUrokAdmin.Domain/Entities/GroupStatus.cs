namespace NeUrokAdmin.Domain.Entities;

public partial class GroupStatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}

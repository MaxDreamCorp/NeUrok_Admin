namespace NeUrokAdmin.Domain.Entities;

public partial class Teacher
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public decimal IndividualLessonsShare { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    private Teacher() { }

    public static Teacher Create(int Id, string fullname, decimal individualLessonsShare, string? notes)
    {
        return new Teacher
        {
            Id = Id,
            Fullname = fullname,
            IndividualLessonsShare = individualLessonsShare,
            Notes = notes
        };
    }
}

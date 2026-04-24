using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Infrastructure.Persistance;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<AttendanceStatus> AttendanceStatuses { get; set; }

    public virtual DbSet<AttendanceType> AttendanceTypes { get; set; }

    public virtual DbSet<ClassType> ClassTypes { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientStatus> ClientStatuses { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupDate> GroupDates { get; set; }

    public virtual DbSet<GroupStatus> GroupStatuses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentSubscription> StudentSubscriptions { get; set; }

    public virtual DbSet<Subscribtion> Subscribtions { get; set; }

    public virtual DbSet<SubscriptlonStatus> SubscriptlonStatuses { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("attendances");

            entity.HasIndex(e => e.AttendanceStatusId, "FK_attendance_attendance_status_idx");

            entity.HasIndex(e => e.AttendanceTypeId, "FK_attendance_attendance_status_idx1");

            entity.HasIndex(e => e.ClassTypeId, "FK_attendance_class_type_idx");

            entity.HasIndex(e => e.ClientId, "FK_attendance_client_idx");

            entity.HasIndex(e => e.CourseId, "FK_attendance_course_idx");

            entity.HasIndex(e => e.TeacherId, "FK_attendance_teacher_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AttendanceStatusId).HasColumnName("attendance_status_id");
            entity.Property(e => e.AttendanceTypeId).HasColumnName("attendance_type_id");
            entity.Property(e => e.ClassTypeId).HasColumnName("class_type_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.IsCompleted).HasColumnName("is_completed");
            entity.Property(e => e.Price)
                .HasPrecision(10)
                .HasColumnName("price");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.AttendanceStatus).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.AttendanceStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_attendance_attendance_status");

            entity.HasOne(d => d.AttendanceType).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.AttendanceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_attendance_attendance_type");

            entity.HasOne(d => d.ClassType).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.ClassTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_attendance_class_type");

            entity.HasOne(d => d.Client).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_attendance_client");

            entity.HasOne(d => d.Course).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_attendance_course");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_attendance_teacher");
        });

        modelBuilder.Entity<AttendanceStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("attendance_statuses");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");
        });

        modelBuilder.Entity<AttendanceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("attendance_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .HasColumnName("type");
        });

        modelBuilder.Entity<ClassType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("class_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(40)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clients");

            entity.HasIndex(e => e.StatusId, "FK_client_client_status_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AdditionalPhones)
                .HasMaxLength(150)
                .HasColumnName("additional_phones");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.ChildFullname)
                .HasMaxLength(255)
                .HasColumnName("child_fullname");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
            entity.Property(e => e.ParentName)
                .HasMaxLength(255)
                .HasColumnName("parent_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(40)
                .HasColumnName("phone");
            entity.Property(e => e.RegistrationDate).HasColumnName("registration_date");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Status).WithMany(p => p.Clients)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_client_client_status");

            entity.HasMany(d => d.Courses).WithMany(p => p.Clients)
                .UsingEntity<Dictionary<string, object>>(
                    "ClientCourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .HasConstraintName("FK_client_course_course"),
                    l => l.HasOne<Client>().WithMany()
                        .HasForeignKey("ClientId")
                        .HasConstraintName("FK_client_course_client"),
                    j =>
                    {
                        j.HasKey("ClientId", "CourseId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("client_courses");
                        j.HasIndex(new[] { "CourseId" }, "FK_client_course_course_idx");
                        j.IndexerProperty<int>("ClientId").HasColumnName("client_id");
                        j.IndexerProperty<int>("CourseId").HasColumnName("course_id");
                    });
        });

        modelBuilder.Entity<ClientStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("client_statuses");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("courses");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("groups");

            entity.HasIndex(e => e.CourseId, "FK_group_course_idx");

            entity.HasIndex(e => e.GroupStatusId, "FK_group_group_status_idx");

            entity.HasIndex(e => e.TeacherId, "FK_group_teacher_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.GroupStatusId).HasColumnName("group_status_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.Time)
                .HasColumnType("time")
                .HasColumnName("time");
            entity.Property(e => e.WeekDays)
                .HasMaxLength(100)
                .HasColumnName("week_days");

            entity.HasOne(d => d.Course).WithMany(p => p.Groups)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_group_course");

            entity.HasOne(d => d.GroupStatus).WithMany(p => p.Groups)
                .HasForeignKey(d => d.GroupStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_group_group_status");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Groups)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_group_teacher");

            entity.HasMany(d => d.Students).WithMany(p => p.Groups)
                .UsingEntity<Dictionary<string, object>>(
                    "GroupStudent",
                    r => r.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK_group_student_student"),
                    l => l.HasOne<Group>().WithMany()
                        .HasForeignKey("GroupId")
                        .HasConstraintName("FK_group_student_group"),
                    j =>
                    {
                        j.HasKey("GroupId", "StudentId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("group_students");
                        j.HasIndex(new[] { "StudentId" }, "FK_group_student_student_idx");
                        j.IndexerProperty<int>("GroupId").HasColumnName("group_id");
                        j.IndexerProperty<int>("StudentId").HasColumnName("student_id");
                    });
        });

        modelBuilder.Entity<GroupDate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("group_dates");

            entity.HasIndex(e => e.GroupId, "FK_group_date_group_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Datetime)
                .HasColumnType("datetime")
                .HasColumnName("datetime");
            entity.Property(e => e.GroupId).HasColumnName("group_id");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupDates)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_group_date_group");
        });

        modelBuilder.Entity<GroupStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("group_statuses");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("students");

            entity.HasIndex(e => e.ClientId, "FK_student_client_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");

            entity.HasOne(d => d.Client).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_student_client");
        });

        modelBuilder.Entity<StudentSubscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("student_subscriptions");

            entity.HasIndex(e => e.CourseId, "FK_student_subscription_course_idx");

            entity.HasIndex(e => e.StudentId, "FK_student_subscription_student_idx");

            entity.HasIndex(e => e.SubscriptionId, "FK_student_subscription_subscription_idx");

            entity.HasIndex(e => e.SubscriptlonStatusId, "FK_student_subscription_subscription_status_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.IsPaid).HasColumnName("is_paid");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.SubscriptionFinishDate).HasColumnName("subscription_finish_date");
            entity.Property(e => e.SubscriptionId).HasColumnName("subscription_id");
            entity.Property(e => e.SubscriptionStartDate).HasColumnName("subscription_start_date");
            entity.Property(e => e.SubscriptlonStatusId).HasColumnName("subscriptlon_status_id");

            entity.HasOne(d => d.Course).WithMany(p => p.StudentSubscriptions)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_student_subscription_course");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentSubscriptions)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_student_subscription_student");

            entity.HasOne(d => d.Subscription).WithMany(p => p.StudentSubscriptions)
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("FK_student_subscription_subscription");

            entity.HasOne(d => d.SubscriptlonStatus).WithMany(p => p.StudentSubscriptions)
                .HasForeignKey(d => d.SubscriptlonStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_student_subscription_subscription_status");
        });

        modelBuilder.Entity<Subscribtion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("subscribtions");

            entity.HasIndex(e => e.ClassesTypeId, "FK_subscribtion_class_type_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ClassesAmount).HasColumnName("classes_amount");
            entity.Property(e => e.ClassesTypeId).HasColumnName("classes_type_id");
            entity.Property(e => e.Cost)
                .HasPrecision(10)
                .HasColumnName("cost");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");

            entity.HasOne(d => d.ClassesType).WithMany(p => p.Subscribtions)
                .HasForeignKey(d => d.ClassesTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_subscribtion_class_type");
        });

        modelBuilder.Entity<SubscriptlonStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("subscriptlon_statuses");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("teachers");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(32)
                .HasColumnName("password_salt");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

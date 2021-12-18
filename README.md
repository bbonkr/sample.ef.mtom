## Tables

### Student

| Name  | Nullable | Constaint |
| :---- | :------: | :-------: |
| Id    | NN       | PK        |
| Name  | NN       |           |

### Cource

| Name  | Nullable | Constaint |
| :---- | :-----: | :--------: |
| Id    | NN      | PK         |
| Title | NN      |            |

### Enrollment

| Name      | Nullable | Constaint |
| :-------- | :------: | :-------: |
| StudentId | NN       | PK        |
| CourseId  | NN       | PK        |

## EntityTypeConfiguration 

EntityTypeConfiguration of Enrollment entity

### .net core 3.1

```csharp
public class EnrollmentEntityTypeConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(x => new { x.StudentId, x.CourseId });

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.CourseId)
            .IsRequired();

        builder.HasOne(x => x.Student)
            .WithMany(x => x.Enrollments)
            .HasForeignKey(x => x.StudentId);

        builder.HasOne(x => x.Course)
            .WithMany(x => x.Enrollments)
            .HasForeignKey(x => x.CourseId);                
    }
}
```

### .NET 5

### .NET 6

## Usages

### .net core 3.1

Get student and their enrolled courses

```csharp
var students = await Context.Students
    .Include(x => x.Enrollments)
    .Select(x => new
    {
        Name = x.Name,
        Courses = x.Enrollments.Select(x => new
        {
            Title = x.Course.Title,
        }),
    });
```

### .NET 5

### .NET 6

## Others


### Add migrations

```bash
$ cd src/Sample.Data
$ dotnet ef migrations add "Migrations name" --context AppDbContext --startup-project ../Sample.App --project ../Sample.Data.SqlServer 
```
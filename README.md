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

No change

### .NET 6

Remove EnrollmentEntityTypeConfiguration class, configure in course entity type configuration

```csharp
public class CourseEntityTypeConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Title)
            .IsRequired();

        builder.HasMany(x => x.Students)
            .WithMany(x => x.Courses)
            .UsingEntity<Enrollment>(
                j => j.HasOne(x => x.Student).WithMany(x => x.Enrollments).HasForeignKey(x => x.StudentId),
                j => j.HasOne(x => x.Course).WithMany(x => x.Enrollments).HasForeignKey(x => x.CourseId),
                j =>
                {
                    j.HasKey(x => new { x.StudentId, x.CourseId });
                });
    }
}
```

There is No change in migrations codes after Changed entity type configuration. [20211218051811_Change mtom](https://github.com/bbonkr/sample.ef.mtom/blob/features/net6/src/Sample.Data.SqlServer/Migrations/20211218051811_Change%20mtom.cs)

## Usages

### .net core 3.1

Get student and their enrolled courses

```csharp
var students = await Context.Students
    .Include(x => x.Enrollments)
    .Select(student => new
    {
        Name = student.Name,
        Courses = x.Enrollments.Select(enrollment => new
        {
            Title = enrollment.Course.Title,
        }),
    });
```

```csharp
var enrollments = Context.Courses
    .ToList()
    .Where((_, index) => index < value)
    .Select(x => new Enrollment
    {
        CourseId = x.Id,
    });

student.Enrollments
    .AddRange(enrollments);
```

### .NET 5

No change

### .NET 6

Can access course of student enrollment directly. Does not need to access through enrollments.

```csharp
var students = await Context.Students
    .Include(x => x.Courses)
    .Select(student => new
    {
        Name = student.Name,
        Courses = student.Courses.Select(course => new
        {
            Title = course.Title,
        }),
    })
```

Also, can insert course of student directly. Does not need to access through enrollments.

```csharp
var coursesToEnroll = Context.Courses
    .ToList()
    .Where((_, index) => index < value);

student.Courses.AddRange(coursesToEnroll);
```

## Others

### dotnet-ef tool

If install dotnet tool locally, make sure to create manifest file first.

```bash
$ dotnet new tool-manifest
```

Pelase see [nuget: dotnet-ef](https://www.nuget.org/packages/dotnet-ef/) versions tab.

.net core 3.1

```bash
$ dotnet tool install --local dotnet-ef --version 3.1.22
```
.net 5

```bash
$ dotnet tool update --local dotnet-ef --version 5.0.13
```

.net 6

```bash
$ dotnet tool update --local dotnet-ef --version 6.0.1
```

### Add migrations

```bash
$ cd src/Sample.Data
$ dotnet ef migrations add "Migrations name" --context AppDbContext --startup-project ../Sample.App --project ../Sample.Data.SqlServer 
```
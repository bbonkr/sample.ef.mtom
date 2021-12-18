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

No change

### .NET 6

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
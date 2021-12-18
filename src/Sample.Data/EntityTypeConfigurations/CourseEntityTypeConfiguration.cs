
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sample.Entities;

namespace Sample.Data.EntityTypeConfigurations;

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

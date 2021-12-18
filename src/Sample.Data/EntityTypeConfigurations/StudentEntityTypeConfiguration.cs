using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sample.Entities;

namespace Sample.Data.EntityTypeConfigurations;

public class StudentEntityTypeConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired();
    }
}

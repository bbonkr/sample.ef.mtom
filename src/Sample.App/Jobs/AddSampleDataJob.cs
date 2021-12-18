using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Sample.Data;
using Sample.Entities;

namespace Sample.App.Jobs;

public class AddSampleDataJob : JobBase
{
    public AddSampleDataJob(AppDbContext context, ILogger<AddSampleDataJob> logger) : base(context, logger)
    {
    }

    public override async Task ExecuteAsync()
    {
        var studentNames = new List<string>
            {
                "Brad",
                "Jake",
                "James",
                "Micheal",
                "Tomas"
            };

        var courseTitles = new List<string>
            {
                "History",
                "Mathematics",
                "Physics",
                "Physics"
            };

        var students = studentNames.Select((name) => new Student { Name = name });
        var courses = courseTitles.Select((title) => new Course { Title = title });

        using (var transaction = Context.Database.BeginTransaction())
        {
            try
            {
                Context.Students.AddRange(students);
                Context.Courses.AddRange(courses);

                await Context.SaveChangesAsync();

                Logger.LogInformation("Student: {student} rows added", students.Count());
                Logger.LogInformation("Course: {course} rows added", students.Count());

                var random = new Random();
                // Enrollment row required
                //foreach (var student in Context.Students.Include(x => x.Enrollments).ToList())
                //{
                //    var value = random.Next(1, courses.Count());

                //    var enrollments = Context.Courses
                //        .ToList()
                //        .Where((_, index) => index < value)
                //        .Select(x => new Enrollment
                //        {
                //            CourseId = x.Id,
                //        });

                //    student.Enrollments
                //        .AddRange(enrollments);
                //}

                // Add courses of student directly. /-o-)/
                foreach (var student in Context.Students.Include(x => x.Courses).ToList())
                {
                    var value = random.Next(1, courses.Count());

                    var coursesToEnroll = Context.Courses
                        .ToList()
                        .Where((_, index) => index < value);

                    student.Courses.AddRange(coursesToEnroll);
                }

                await Context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }
    }
}

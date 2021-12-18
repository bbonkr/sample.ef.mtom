using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Sample.App.Features;
using Sample.Data;

namespace Sample.App.Jobs;

public class QuerySampleDataJob : JobBase
{
    public QuerySampleDataJob(ObjectViewer viewer, AppDbContext context, ILogger<QuerySampleDataJob> logger) : base(context, logger)
    {
        this.viewer = viewer;
    }

    public override async Task ExecuteAsync()
    {
        var students = await Context.Students
            .Include(x => x.Courses)
            .Select(student => new
            {
                Name = student.Name,
                Courses = student.Courses.Select(course => new
                {
                    Title = course.Title,
                }),
            }).ToListAsync();

        //Logger.LogInformation("Student: {@students}", students);

        //System.Console.WriteLine(viewer.Print(students));

        Logger.LogInformation("Student: {students}", viewer.PringJson(students));
    }

    private readonly ObjectViewer viewer;
}

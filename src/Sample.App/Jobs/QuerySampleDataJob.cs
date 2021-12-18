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
            .Include(x => x.Enrollments)
            .Select(x => new
            {
                Name = x.Name,
                Courses = x.Enrollments.Select(x => new
                {
                    Title = x.Course.Title,
                }),
            }).ToListAsync();

        //Logger.LogInformation("Student: {@students}", students);

        //System.Console.WriteLine(viewer.Print(students));

        Logger.LogInformation("Student: {students}", viewer.PringJson(students));
    }

    private readonly ObjectViewer viewer;
}

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Sample.Data;

namespace Sample.App.Jobs;

public class ClearSampleDataJob : JobBase
{
    public ClearSampleDataJob(AppDbContext context, ILogger<ClearSampleDataJob> logger) : base(context, logger)
    {
    }

    public override async Task ExecuteAsync()
    {
        using (var transaction = Context.Database.BeginTransaction())
        {
            try
            {
                Context.Students.RemoveRange(Context.Students.ToList());
                await Context.SaveChangesAsync();

                Context.Courses.RemoveRange(Context.Courses.ToList());
                await Context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Logger.LogError(ex, ex.Message);

            }
        }
    }
}

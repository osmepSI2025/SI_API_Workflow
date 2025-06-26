using Microsoft.EntityFrameworkCore;
using SME_API_Workflow.Entities;
using SME_API_Workflow.Models;

public class JobSchedulerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public JobSchedulerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Si_WorkflowDBContext>();
                var now = DateTime.Now;
                var jobs = await db.MScheduledJobs
                    .Where(j => j.IsActive == true && j.RunHour == now.Hour && j.RunMinute == now.Minute)
                    .ToListAsync(stoppingToken);

                foreach (var job in jobs)
                {
                    _ = RunJobAsync(job.JobName, scope.ServiceProvider);
                }
            }

            // Check every minute
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task RunJobAsync(string jobName, IServiceProvider serviceProvider)
    {
        switch (jobName)
        {
            case "Workflow":
                SearchWorkflowApiResponse models1 = new SearchWorkflowApiResponse();
                await serviceProvider.GetRequiredService<MWorkflowService>().BatchEndOfDay_MWorkflow(models1);
                break;
            case "WorkflowActivity":
                searchWorkflowActivityDataModel models2 = new searchWorkflowActivityDataModel();
                await serviceProvider.GetRequiredService<MWorkflowActivityService>().BatchEndOfDay_MWorkflowActivity(models2);
                break;
   
            case "WorkflowControlPoint":
                searchWorkflowControlPointDataModel models3 = new searchWorkflowControlPointDataModel();
                await serviceProvider.GetRequiredService<MWorkflowControlPointService>().BatchEndOfDay_MWorkflowControlPoints(models3);
                break;
            case "WorkflowLeadingLagging":
                searchWorkflowLeadingLaggingDataModel models4 = new searchWorkflowLeadingLaggingDataModel();
                await serviceProvider.GetRequiredService<MWorkflowLeadingLaggingService>().BatchEndOfDay_MWorkflowLeadingLagging(models4);
                break;
            default:
                // Optionally log unknown job
                break;
        }
    }
}
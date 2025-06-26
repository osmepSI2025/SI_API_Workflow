using Quartz;
using SME_API_Workflow.Models;

public class ScheduledJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    public ScheduledJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var jobName = context.JobDetail.JobDataMap.GetString("JobName");

        using var scope = _serviceProvider.CreateScope();
        switch (jobName)
        {
            case "Workflow":
                var models1 = new SearchWorkflowApiResponse();
                await scope.ServiceProvider.GetRequiredService<MWorkflowService>().BatchEndOfDay_MWorkflow(models1);
                break;
            case "WorkflowActivity":
                var models2 = new searchWorkflowActivityDataModel();
                await scope.ServiceProvider.GetRequiredService<MWorkflowActivityService>().BatchEndOfDay_MWorkflowActivity(models2);
                break;
            case "WorkflowControlPoint":
                var models3 = new searchWorkflowControlPointDataModel();
                await scope.ServiceProvider.GetRequiredService<MWorkflowControlPointService>().BatchEndOfDay_MWorkflowControlPoints(models3);
                break;
            case "WorkflowLeadingLagging":
                var models4 = new searchWorkflowLeadingLaggingDataModel();
                await scope.ServiceProvider.GetRequiredService<MWorkflowLeadingLaggingService>().BatchEndOfDay_MWorkflowLeadingLagging(models4);
                break;
            default:
                // Optionally log unknown job
                break;
        }
    }
}
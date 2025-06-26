using Quartz;

public class SampleJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        // Your job logic here
        return Task.CompletedTask;
    }
}
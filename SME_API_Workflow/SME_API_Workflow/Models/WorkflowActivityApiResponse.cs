public class WorkflowActivityApiResponse
{
    public string? ResponseCode { get; set; }
    public string? ResponseMsg { get; set; }
    public List<WorkflowActivityDataModel>? Data { get; set; }
}

public class WorkflowActivityDataModel
{
    public string? WorkflowCode { get; set; }
    public string? WorkflowName { get; set; }
    public string? WorkflowType { get; set; }
    public string? WorkflowGoupCode { get; set; }
    public string? Period { get; set; }

    public List<ActivityDetailModel>? ActivityDetails { get; set; }
}

public class ActivityDetailModel
{
    public string? ControlPoint { get; set; }
    public string? Activity { get; set; }
    public string? Description { get; set; }
}
public class searchWorkflowActivityDataModel
{
    public string? WorkflowCode { get; set; }
    
}
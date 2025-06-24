public class WorkflowControlPointApiResponse
{
    public string? ResponseCode { get; set; }
    public string? ResponseMsg { get; set; }
    public List<WorkflowControlPointDataModel>? Data { get; set; }
}

public class WorkflowControlPointDataModel
{
    public string? WorkflowCode { get; set; }
    public string? WorkflowName { get; set; }
    public string? WorkflowType { get; set; }
    public string? WorkflowGoupCode { get; set; }
    public string? Period { get; set; }
    public List<WorkflowControlPointActivityDetail>? ActivityDetails { get; set; }
}

public class WorkflowControlPointActivityDetail
{
    public string? ControlPoint { get; set; }
    public string? Activity { get; set; }
    public string? Description { get; set; }
}

public class searchWorkflowControlPointDataModel
{
    public string? WorkflowCode { get; set; }
  
}

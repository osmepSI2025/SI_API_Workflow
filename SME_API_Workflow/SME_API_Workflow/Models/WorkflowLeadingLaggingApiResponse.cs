namespace SME_API_Workflow.Models
{
    public class WorkflowLeadingLaggingApiResponse
    {
        public string? ResponseCode { get; set; }
        public string? ResponseMsg { get; set; }
        public List<WorkflowLeadingLaggingDataModel>? Data { get; set; }
    }

    public class WorkflowLeadingLaggingDataModel
    {
        public string? WorkflowCode { get; set; }
        public string? WorkflowName { get; set; }
        public string? WorkflowType { get; set; }
        public string? WorkflowGoupCode { get; set; }
        public string? Period { get; set; }
        public string? HaveDigital { get; set; }
        public string? HaveWorkflow { get; set; }
        public string? CreateWorkflow { get; set; }
        public string? PerformanceIndicator { get; set; }
        public string? URLS { get; set; }
        public string? Leading { get; set; }
        public string? Lagging { get; set; }
    }
    public class searchWorkflowLeadingLaggingDataModel
    {
        public string? WorkflowCode { get; set; }
      
    }
}

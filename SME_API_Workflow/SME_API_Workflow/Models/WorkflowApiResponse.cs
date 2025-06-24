namespace SME_API_Workflow.Models
{
    public class WorkflowApiResponse
    {
        public string? ResponseCode { get; set; }
        public string? ResponseMsg { get; set; }
        public List<WorkflowDataModel>? Data { get; set; }
    }

    public class WorkflowDataModel
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
    }
    public class SearchWorkflowApiResponse
    {
        public string? workflowCode { get; set; }
     
    }
}

using System;
using System.Collections.Generic;

namespace SME_API_Workflow.Entities;

public partial class MWorkflowLeadingLagging
{
    public int Id { get; set; }

    public string? WorkflowCode { get; set; }

    public string? WorkflowName { get; set; }

    public string? WorkflowType { get; set; }

    public string? WorkflowGroupCode { get; set; }

    public string? Period { get; set; }

    public string? Leading { get; set; }

    public string? Lagging { get; set; }
}

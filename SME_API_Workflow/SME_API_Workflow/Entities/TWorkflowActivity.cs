using System;
using System.Collections.Generic;

namespace SME_API_Workflow.Entities;

public partial class TWorkflowActivity
{
    public int Id { get; set; }

    public int? WorkflowId { get; set; }

    public string? ControlPoint { get; set; }

    public string? Activity { get; set; }

    public string? Description { get; set; }

    public virtual MWorkflowActivity? Workflow { get; set; }
}

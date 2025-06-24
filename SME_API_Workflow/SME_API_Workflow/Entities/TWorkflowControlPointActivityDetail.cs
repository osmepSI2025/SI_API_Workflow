using System;
using System.Collections.Generic;

namespace SME_API_Workflow.Entities;

public partial class TWorkflowControlPointActivityDetail
{
    public int Id { get; set; }

    public int? WorkflowId { get; set; }

    public string? ControlPoint { get; set; }

    public string? Activity { get; set; }

    public string? Description { get; set; }

    public virtual MWorkflowControlPoint? Workflow { get; set; }
}

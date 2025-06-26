using Microsoft.AspNetCore.Mvc;
using SME_API_Workflow.Entities;
using SME_API_Workflow.Models;

namespace SME_API_Workflow.Controllers
{
    [ApiController]
    [Route("api/SYS-WORKFLOW")]
    public class WorkflowController : ControllerBase
    {
         private readonly MWorkflowService _workflowService;
        private readonly MWorkflowLeadingLaggingService _workflowLeadingLaggingService;
        private readonly MWorkflowActivityService _workflowActivityService;
        private readonly MWorkflowControlPointService _workflowControlPointService;
        private readonly ILogger<WorkflowController> _logger;
        public WorkflowController(ILogger<WorkflowController> logger
            , MWorkflowService workflowService,
MWorkflowLeadingLaggingService workflowLeadingLaggingService 
            ,MWorkflowActivityService mWorkflowActivityService
            ,MWorkflowControlPointService mWorkflowControlPointService)
        {
            _logger = logger;
            _workflowService = workflowService;
            _workflowLeadingLaggingService = workflowLeadingLaggingService;
            _workflowActivityService = mWorkflowActivityService;
            _workflowControlPointService = mWorkflowControlPointService;
        }
        [HttpPost("Workflow")]
        public async Task<IActionResult> GetWorkflow(SearchWorkflowApiResponse models)
        {
            var result = await _workflowService.GetAllAsyncSearch_MWorkflow(models);
            if (result.ResponseCode == null || result.Data == null|| result.Data.Count == 0) 
            {
                result = new WorkflowApiResponse {
                 Data = new List<WorkflowDataModel>(),
                 ResponseMsg = "No data found",
                 ResponseCode = "200"
                };
            }
            return Ok(result);
        }
        [HttpGet("Workflow-Batch")]
        public async Task<IActionResult> GetWorkflowBatch()
        {
            SearchWorkflowApiResponse models = new SearchWorkflowApiResponse();
            await _workflowService.BatchEndOfDay_MWorkflow(models);
            return Ok();
        }
       
        [HttpPost("WorkflowActivity")]
        public async Task<IActionResult> GetWorkflowActivity(searchWorkflowActivityDataModel models)
        {
            var result = await _workflowActivityService.GetAllAsyncSearch_MWorkflowActivity(models);
            return Ok(result);
        }

        [HttpGet("WorkflowActivity-Batch")]
        public async Task<IActionResult> GetWorkflowActivityBatch()
        {
            searchWorkflowActivityDataModel models  = new searchWorkflowActivityDataModel();  
             await _workflowActivityService.BatchEndOfDay_MWorkflowActivity(models);
            return Ok();
        }
        [HttpPost("WorkflowControlPoint")]
        public async Task<IActionResult> GetWorkflowControlPoint(searchWorkflowControlPointDataModel models)
        {
            var result = await _workflowControlPointService.GetAllAsyncSearch_MWorkflowControlPoints(models);
            return Ok(result);
        }
        [HttpGet("WorkflowControlPoint-Batch")]
        public async Task<IActionResult> GetWorkflowControlPointBatch()
        {
            searchWorkflowControlPointDataModel models = new searchWorkflowControlPointDataModel();
            await _workflowControlPointService.BatchEndOfDay_MWorkflowControlPoints(models);
            return Ok();
        }



        [HttpPost("WorkflowLeadingLagging")]
        public async Task<IActionResult> GetWorkflowLeadingLagging(searchWorkflowLeadingLaggingDataModel models)
        {
            var result = await _workflowLeadingLaggingService.GetAllAsyncSearch_MWorkflowLeadingLagging(models);
            return Ok(result);
        }
        [HttpGet("WorkflowLeadingLagging-Batch")]
        public async Task<IActionResult> GetWorkflowLeadingLaggingBatch()
        {
            searchWorkflowLeadingLaggingDataModel models = new searchWorkflowLeadingLaggingDataModel();
             await _workflowLeadingLaggingService.BatchEndOfDay_MWorkflowLeadingLagging(models);
            return Ok();
        }
    }
   
}

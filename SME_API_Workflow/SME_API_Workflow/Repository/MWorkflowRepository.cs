using SME_API_Workflow.Entities;
using Microsoft.EntityFrameworkCore;
using SME_API_Workflow.Models;
public class MWorkflowRepository 
{
    private readonly Si_WorkflowDBContext _context;

    public MWorkflowRepository(Si_WorkflowDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MWorkflow>> GetAllAsync()
    {
        try
        {
            return await _context.MWorkflows.ToListAsync();
        }
        catch (Exception ex)
        {
            // Log or handle error
            throw new Exception("Error retrieving workflows", ex);
        }
    }

    public async Task<MWorkflow?> GetByIdAsync(string workflowCode)
    {
        try
        {
            return await _context.MWorkflows.FirstOrDefaultAsync(e=>e.WorkflowCode == workflowCode);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving workflow with id {workflowCode}", ex);
        }
    }

    public async Task AddAsync(MWorkflow workflow)
    {
        try
        {
            _context.MWorkflows.Add(workflow);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding workflow", ex);
        }
    }

    public async Task UpdateAsync(MWorkflow workflow)
    {
        try
        {
            _context.MWorkflows.Update(workflow);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating workflow", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var workflow = await _context.MWorkflows.FindAsync(id);
            if (workflow != null)
            {
                _context.MWorkflows.Remove(workflow);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting workflow", ex);
        }
    }
    public async Task<IEnumerable<MWorkflow>> GetAllAsyncSearch_MWorkflow(SearchWorkflowApiResponse searchModel)
    {
        try
        {
            var query = _context.MWorkflows
               
                .AsQueryable();

            if (!string.IsNullOrEmpty( searchModel.workflowCode))
            {
                query = query.Where(bu => bu.WorkflowCode == searchModel.workflowCode);
            }
            // Add more filters if needed, e.g. for dimensionid

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<MWorkflow>();
        }
    }
}

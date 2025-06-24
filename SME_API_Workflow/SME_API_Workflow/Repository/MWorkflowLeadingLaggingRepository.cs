using SME_API_Workflow.Entities;
using Microsoft.EntityFrameworkCore;
using SME_API_Workflow.Models;
public class MWorkflowLeadingLaggingRepository 
{
    private readonly Si_WorkflowDBContext _context;

    public MWorkflowLeadingLaggingRepository(Si_WorkflowDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MWorkflowLeadingLagging>> GetAllAsync()
    {
        try
        {
            return await _context.MWorkflowLeadingLaggings.ToListAsync();
        }
        catch (Exception ex)
        {
            // Log or handle error
            throw new Exception("Error retrieving workflows", ex);
        }
    }

    public async Task<MWorkflowLeadingLagging?> GetByIdAsync(string workflowCode)
    {
        try
        {
            return await _context.MWorkflowLeadingLaggings.FirstOrDefaultAsync(e=>e.WorkflowCode == workflowCode);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving workflow with id {workflowCode}", ex);
        }
    }

    public async Task AddAsync(MWorkflowLeadingLagging workflow)
    {
        try
        {
            _context.MWorkflowLeadingLaggings.Add(workflow);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding workflow", ex);
        }
    }

    public async Task UpdateAsync(MWorkflowLeadingLagging workflow)
    {
        try
        {
            _context.MWorkflowLeadingLaggings.Update(workflow);
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
            var workflow = await _context.MWorkflowLeadingLaggings.FindAsync(id);
            if (workflow != null)
            {
                _context.MWorkflowLeadingLaggings.Remove(workflow);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting workflow", ex);
        }
    }
    public async Task<IEnumerable<MWorkflowLeadingLagging>> GetAllAsyncSearch_MWorkflowLeadingLagging(searchWorkflowLeadingLaggingDataModel searchModel)
    {
        try
        {
            var query = _context.MWorkflowLeadingLaggings
               
                .AsQueryable();

            if (!string.IsNullOrEmpty( searchModel.WorkflowCode))
            {
                query = query.Where(bu => bu.WorkflowCode == searchModel.WorkflowCode);
            }
            // Add more filters if needed, e.g. for dimensionid

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<MWorkflowLeadingLagging>();
        }
    }
}

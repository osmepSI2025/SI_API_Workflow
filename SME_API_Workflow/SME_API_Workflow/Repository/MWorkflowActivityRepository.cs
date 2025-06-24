using SME_API_Workflow.Entities;
using Microsoft.EntityFrameworkCore;
using SME_API_Workflow.Models;
public class MWorkflowActivityRepository 
{
    private readonly Si_WorkflowDBContext _context;

    public MWorkflowActivityRepository(Si_WorkflowDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MWorkflowActivity>> GetAllAsync()
    {
        try
        {
            return await _context.MWorkflowActivities.ToListAsync();
        }
        catch (Exception ex)
        {
            // Log or handle error
            throw new Exception("Error retrieving workflows", ex);
        }
    }

    public async Task<MWorkflowActivity?> GetByIdAsync(string workflowCode)
    {
        try
        {
            return await _context.MWorkflowActivities
                .Include(x => x.TWorkflowActivities)
                .FirstOrDefaultAsync(e=>e.WorkflowCode == workflowCode);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving workflow with id {workflowCode}", ex);
        }
    }

    public async Task AddAsync(MWorkflowActivity workflow)
    {
        try
        {
            _context.MWorkflowActivities.Add(workflow);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding workflow", ex);
        }
    }

    public async Task UpdateAsync(MWorkflowActivity workflow)
    {
        try
        {
            _context.MWorkflowActivities.Update(workflow);
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
            var workflow = await _context.MWorkflowActivities.FindAsync(id);
            if (workflow != null)
            {
                _context.MWorkflowActivities.Remove(workflow);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting workflow", ex);
        }
    }
    public async Task<IEnumerable<MWorkflowActivity>> GetAllAsyncSearch_MWorkflowActivity(searchWorkflowActivityDataModel searchModel)
    {
        try
        {
            var query = _context.MWorkflowActivities
                .Include(x => x.TWorkflowActivities)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.WorkflowCode))
            {
                query = query.Where(bu => bu.WorkflowCode == searchModel.WorkflowCode);
            }
            // Add more filters if needed, e.g. for dimensionid

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<MWorkflowActivity>();
        }
    }
}

using SME_API_Workflow.Entities;
using Microsoft.EntityFrameworkCore;
using SME_API_Workflow.Models;
public class MWorkflowControlPointRepository 
{
    private readonly Si_WorkflowDBContext _context;

    public MWorkflowControlPointRepository(Si_WorkflowDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MWorkflowControlPoint>> GetAllAsync()
    {
        try
        {
            return await _context.MWorkflowControlPoints.ToListAsync();
        }
        catch (Exception ex)
        {
            // Log or handle error
            throw new Exception("Error retrieving workflows", ex);
        }
    }

    public async Task<MWorkflowControlPoint?> GetByIdAsync(string workflowCode)
    {
        try
        {
            return await _context.MWorkflowControlPoints
                .Include(x => x.TWorkflowControlPointActivityDetails)
                .FirstOrDefaultAsync(e=>e.WorkflowCode == workflowCode);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving workflow with id {workflowCode}", ex);
        }
    }

    public async Task AddAsync(MWorkflowControlPoint workflow)
    {
        try
        {
            _context.MWorkflowControlPoints.Add(workflow);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding workflow", ex);
        }
    }

    public async Task UpdateAsync(MWorkflowControlPoint workflow)
    {
        try
        {
            _context.MWorkflowControlPoints.Update(workflow);
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
            var workflow = await _context.MWorkflowControlPoints.FindAsync(id);
            if (workflow != null)
            {
                _context.MWorkflowControlPoints.Remove(workflow);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting workflow", ex);
        }
    }
    public async Task<IEnumerable<MWorkflowControlPoint>> GetAllAsyncSearch_MWorkflowControlPoints(searchWorkflowControlPointDataModel searchModel)
    {
        try
        {
            var query = _context.MWorkflowControlPoints
                .Include(x => x.TWorkflowControlPointActivityDetails)
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
            return Enumerable.Empty<MWorkflowControlPoint>();
        }
    }
}

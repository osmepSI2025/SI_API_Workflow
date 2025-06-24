using SME_API_Workflow.Models;

namespace SME_API_Workflow.Services
{
    public interface ICallAPIService
    {
        Task<string> GetDataApiAsync(MapiInformationModels apiModels, object xdata);

    }
}

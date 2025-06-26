using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SME_API_Workflow.Entities;
using SME_API_Workflow.Models;
using SME_API_Workflow.Repository;
using SME_API_Workflow.Services;
public class MWorkflowService 
{
    private readonly MWorkflowRepository _repository;
    private readonly ICallAPIService _serviceApi;
    private readonly IApiInformationRepository _repositoryApi;
    private readonly string _FlagDev;
    public MWorkflowService(MWorkflowRepository repository
            , IConfiguration configuration, ICallAPIService serviceApi, IApiInformationRepository repositoryApi)
    {
        _repository = repository;
        _serviceApi = serviceApi;
        _repositoryApi = repositoryApi;
        _FlagDev = configuration["Devlopment:FlagDev"] ?? throw new ArgumentNullException("FlagDev is missing in appsettings.json");

    }

    public async Task<IEnumerable<MWorkflow>> GetAllAsync()
    {
        try
        {
            return await _repository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Service error retrieving workflows", ex);
        }
    }

    public async Task<MWorkflow?> GetByIdAsync(string WorkflowCode)
    {
        try
        {
            return await _repository.GetByIdAsync(WorkflowCode);
        }
        catch (Exception ex)
        {
            throw new Exception($"Service error retrieving workflow with id {WorkflowCode}", ex);
        }
    }

    public async Task AddAsync(MWorkflow workflow)
    {
        try
        {
            await _repository.AddAsync(workflow);
        }
        catch (Exception ex)
        {
            throw new Exception("Service error adding workflow", ex);
        }
    }

    public async Task UpdateAsync(MWorkflow workflow)
    {
        try
        {
            await _repository.UpdateAsync(workflow);
        }
        catch (Exception ex)
        {
            throw new Exception("Service error updating workflow", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            await _repository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception("Service error deleting workflow", ex);
        }
    }

    public async Task BatchEndOfDay_MWorkflow(SearchWorkflowApiResponse xmodel)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        var WorkflowApiResponse = new WorkflowApiResponse();
        var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Workflow" });
        var apiParam = LApi.Select(x => new MapiInformationModels
        {
            ServiceNameCode = x.ServiceNameCode,
            ApiKey = x.ApiKey,
            AuthorizationType = x.AuthorizationType,
            ContentType = x.ContentType,
            CreateDate = x.CreateDate,
            Id = x.Id,
            MethodType = x.MethodType,
            ServiceNameTh = x.ServiceNameTh,
            Urldevelopment = x.Urldevelopment,
            Urlproduction = x.Urlproduction,
            Username = x.Username,
            Password = x.Password,
            UpdateDate = x.UpdateDate,
            Bearer = x.Bearer,
            AccessToken = x.AccessToken

        }).FirstOrDefault(); // Use FirstOrDefault to handle empty lists

        var apiResponse = await _serviceApi.GetDataApiAsync(apiParam, xmodel);
        var result = JsonSerializer.Deserialize<WorkflowApiResponse>(apiResponse, options);

        WorkflowApiResponse = result ?? new WorkflowApiResponse();

        if (WorkflowApiResponse.Data != null)
        {
            foreach (var item in WorkflowApiResponse.Data)
            {
                try
                {
                    var existing = await _repository.GetByIdAsync(item.WorkflowCode);

                    if (existing == null)
                    {
                        // Create new record
                        var newData = new MWorkflow
                        {
                            WorkflowCode = item.WorkflowCode,
                            WorkflowName = item.WorkflowName,
                            WorkflowType = item.WorkflowType,
                            WorkflowGroupCode = item.WorkflowGoupCode,
                            Period = item.Period,
                            HaveDigital = item.HaveDigital,
                            HaveWorkflow = item.HaveWorkflow,
                            CreateWorkflow = item.CreateWorkflow,
                            PerformanceIndicator = item.PerformanceIndicator,
                            Urls = item.URLS
                        };

                        await _repository.AddAsync(newData);
                        Console.WriteLine($"[INFO] Created new MWorkflow with WorkflowCode {newData.WorkflowCode}");
                    }
                    else
                    {
                        // Update existing record
                        existing.WorkflowName = item.WorkflowName;
                        existing.WorkflowType = item.WorkflowType;
                        existing.WorkflowGroupCode = item.WorkflowGoupCode;
                        existing.Period = item.Period;
                        existing.HaveDigital = item.HaveDigital;
                        existing.HaveWorkflow = item.HaveWorkflow;
                        existing.CreateWorkflow = item.CreateWorkflow;
                        existing.PerformanceIndicator = item.PerformanceIndicator;
                        existing.Urls = item.URLS;

                        await _repository.UpdateAsync(existing);
                        Console.WriteLine($"[INFO] Updated MWorkflow with WorkflowCode {existing.WorkflowCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process MWorkflow with WorkflowCode {item.WorkflowCode}: {ex.Message}");
                }
            }
        }



    }
    public async Task<WorkflowApiResponse> GetAllAsyncSearch_MWorkflow(SearchWorkflowApiResponse xmodel)
    {
        try
        {
            // Get data from repository
            var Ldata = await _repository.GetAllAsyncSearch_MWorkflow(xmodel);

            if (Ldata == null || !Ldata.Any())
            {
                await BatchEndOfDay_MWorkflow(xmodel);

                var Ldata2 = await _repository.GetAllAsyncSearch_MWorkflow(xmodel);
                if (Ldata2 == null || !Ldata2.Any())
                {
                    return new WorkflowApiResponse { 
                     ResponseMsg= "No data found",
                        ResponseCode = "200",
                         Data = new List<WorkflowDataModel>()
                    };
                }
                else
                {
                    var response = BuildApiResponse(Ldata2);
                    return response;
                }
            }
            else
            {
                var response = BuildApiResponse(Ldata);
                return response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to search MPlanKpiList: {ex.Message}");
            return new WorkflowApiResponse
            {
                ResponseCode = "500",
                ResponseMsg = "Internal Server Error",

                Data = new List<WorkflowDataModel>()
            };
        }
    }

    private WorkflowApiResponse BuildApiResponse(IEnumerable<MWorkflow> data)
    {
        return new WorkflowApiResponse
        {
            ResponseCode = "200",
            ResponseMsg = "OK",
            Data = data.Select(d => new WorkflowDataModel
            {
                WorkflowCode = d.WorkflowCode,
                WorkflowName = d.WorkflowName,
                WorkflowType = d.WorkflowType,
                WorkflowGoupCode = d.WorkflowGroupCode,
                Period = d.Period,
                HaveDigital = d.HaveDigital,
                HaveWorkflow = d.HaveWorkflow,
                CreateWorkflow = d.CreateWorkflow,
                PerformanceIndicator = d.PerformanceIndicator,
                URLS = d.Urls
            }).ToList()
        };
    }

}

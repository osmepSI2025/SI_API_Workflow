using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SME_API_Workflow.Entities;
using SME_API_Workflow.Models;
using SME_API_Workflow.Repository;
using SME_API_Workflow.Services;
public class MWorkflowLeadingLaggingService 
{
    private readonly MWorkflowLeadingLaggingRepository _repository;
    private readonly ICallAPIService _serviceApi;
    private readonly IApiInformationRepository _repositoryApi;
    private readonly string _FlagDev;
    public MWorkflowLeadingLaggingService(MWorkflowLeadingLaggingRepository repository
            , IConfiguration configuration, ICallAPIService serviceApi, IApiInformationRepository repositoryApi)
    {
        _repository = repository;
        _serviceApi = serviceApi;
        _repositoryApi = repositoryApi;
        _FlagDev = configuration["Devlopment:FlagDev"] ?? throw new ArgumentNullException("FlagDev is missing in appsettings.json");

    }

    public async Task<IEnumerable<MWorkflowLeadingLagging>> GetAllAsync()
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

    public async Task<MWorkflowLeadingLagging?> GetByIdAsync(string WorkflowCode)
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

    public async Task AddAsync(MWorkflowLeadingLagging workflow)
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

    public async Task UpdateAsync(MWorkflowLeadingLagging workflow)
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

    public async Task BatchEndOfDay_MWorkflowLeadingLagging(searchWorkflowLeadingLaggingDataModel xmodel)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        var WorkflowLeadingLaggingApiResponse = new WorkflowLeadingLaggingApiResponse();

        var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "WorkflowLeadingLagging" });
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
        var result = JsonSerializer.Deserialize<WorkflowLeadingLaggingApiResponse>(apiResponse, options);

        WorkflowLeadingLaggingApiResponse = result ?? new WorkflowLeadingLaggingApiResponse();

        if (WorkflowLeadingLaggingApiResponse.Data != null)
        {
            foreach (var item in WorkflowLeadingLaggingApiResponse.Data)
            {
                try
                {
                    var existing = await _repository.GetByIdAsync(item.WorkflowCode);

                    if (existing == null)
                    {
                        // Create new record
                        var newData = new MWorkflowLeadingLagging
                        {
                            WorkflowCode = item.WorkflowCode,
                     
                        };

                        await _repository.AddAsync(newData);
                        Console.WriteLine($"[INFO] Created new MWorkflowLeadingLagging with WorkflowCode {newData.WorkflowCode}");
                    }
                    else
                    {
                        // Update existing record
                        existing.WorkflowName = item.WorkflowName;
                      

                        await _repository.UpdateAsync(existing);
                        Console.WriteLine($"[INFO] Updated MWorkflowLeadingLagging with WorkflowCode {existing.WorkflowCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process MWorkflowLeadingLagging with WorkflowCode {item.WorkflowCode}: {ex.Message}");
                }
            }
        }



    }
    public async Task<WorkflowLeadingLaggingApiResponse> GetAllAsyncSearch_MWorkflowLeadingLagging(searchWorkflowLeadingLaggingDataModel xmodel)
    {
        try
        {
            // Get data from repository
            var Ldata = await _repository.GetAllAsyncSearch_MWorkflowLeadingLagging(xmodel);

            if (Ldata == null || !Ldata.Any())
            {
                await BatchEndOfDay_MWorkflowLeadingLagging(xmodel);

                var Ldata2 = await _repository.GetAllAsyncSearch_MWorkflowLeadingLagging(xmodel);
                if (Ldata2 == null || !Ldata2.Any())
                {
                   
                    return new WorkflowLeadingLaggingApiResponse
                    {
                        ResponseMsg = "No data found",
                        ResponseCode = "200",
                        Data = new List<WorkflowLeadingLaggingDataModel>()
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
            return new WorkflowLeadingLaggingApiResponse
            {
                ResponseCode = "500",
                ResponseMsg = "Internal Server Error",
              
                Data = new List<WorkflowLeadingLaggingDataModel>()
            };
        }
    }

    private WorkflowLeadingLaggingApiResponse BuildApiResponse(IEnumerable<MWorkflowLeadingLagging> data)
    {
        return new WorkflowLeadingLaggingApiResponse
        {
            ResponseCode = "200",
            ResponseMsg = "OK",
            Data = data.Select(d => new WorkflowLeadingLaggingDataModel
            {
                WorkflowCode = d.WorkflowCode,
                WorkflowName = d.WorkflowName,
                WorkflowType = d.WorkflowType,
                WorkflowGoupCode = d.WorkflowGroupCode,
                Period = d.Period,
                Leading = d.Leading,
                Lagging = d.Lagging
            }).ToList()
        };
    }


}

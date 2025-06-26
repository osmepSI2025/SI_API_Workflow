using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SME_API_Workflow.Entities;
using SME_API_Workflow.Models;
using SME_API_Workflow.Repository;
using SME_API_Workflow.Services;
public class MWorkflowActivityService 
{
    private readonly MWorkflowActivityRepository _repository;
    private readonly ICallAPIService _serviceApi;
    private readonly IApiInformationRepository _repositoryApi;
    private readonly string _FlagDev;
    public MWorkflowActivityService(MWorkflowActivityRepository repository
            , IConfiguration configuration, ICallAPIService serviceApi, IApiInformationRepository repositoryApi)
    {
        _repository = repository;
        _serviceApi = serviceApi;
        _repositoryApi = repositoryApi;
        _FlagDev = configuration["Devlopment:FlagDev"] ?? throw new ArgumentNullException("FlagDev is missing in appsettings.json");

    }

    public async Task<IEnumerable<MWorkflowActivity>> GetAllAsync()
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

    public async Task<MWorkflowActivity?> GetByIdAsync(string WorkflowCode)
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

    public async Task AddAsync(MWorkflowActivity workflow)
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

    public async Task UpdateAsync(MWorkflowActivity workflow)
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

    public async Task BatchEndOfDay_MWorkflowActivity(searchWorkflowActivityDataModel xmodel)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        var WorkflowActivityApiResponse = new WorkflowActivityApiResponse();
        var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "WorkflowActivity" });
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
        var result = JsonSerializer.Deserialize<WorkflowActivityApiResponse>(apiResponse, options);

        WorkflowActivityApiResponse = result ?? new WorkflowActivityApiResponse();

        if (WorkflowActivityApiResponse.Data != null)
        {
            foreach (var item in WorkflowActivityApiResponse.Data)
            {
                try
                {
                    var existing = await _repository.GetByIdAsync(item.WorkflowCode);

                    if (existing == null)
                    {
                        // Create new record
                        var newData = new MWorkflowActivity
                        {
                            WorkflowCode = item.WorkflowCode,
                            WorkflowName = item.WorkflowName,
                            WorkflowType = item.WorkflowType,
                            WorkflowGroupCode = item.WorkflowGoupCode,
                            Period = item.Period,
                            TWorkflowActivities = item.ActivityDetails?.Select(a => new TWorkflowActivity
                            {
                                ControlPoint = a.ControlPoint,
                                Activity = a.Activity,
                                Description = a.Description
                            }).ToList() ?? new List<TWorkflowActivity>()
                        };

                        await _repository.AddAsync(newData);
                        Console.WriteLine($"[INFO] Created new MWorkflowActivity with WorkflowCode {newData.WorkflowCode}");
                    }
                    else
                    {
                        // Update existing record
                        existing.WorkflowName = item.WorkflowName;
                        existing.WorkflowType = item.WorkflowType;
                        existing.WorkflowGroupCode = item.WorkflowGoupCode;
                        existing.Period = item.Period;

                        // Update TWorkflowActivities
                        existing.TWorkflowActivities.Clear();
                        if (item.ActivityDetails != null)
                        {
                            foreach (var a in item.ActivityDetails)
                            {
                                existing.TWorkflowActivities.Add(new TWorkflowActivity
                                {
                                    ControlPoint = a.ControlPoint,
                                    Activity = a.Activity,
                                    Description = a.Description
                                });
                            }
                        }

                        await _repository.UpdateAsync(existing);
                        Console.WriteLine($"[INFO] Updated MWorkflowActivity with WorkflowCode {existing.WorkflowCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process MWorkflowActivity with WorkflowCode {item.WorkflowCode}: {ex.Message}");
                }
            }
        }

    }
    public async Task<WorkflowActivityApiResponse> GetAllAsyncSearch_MWorkflowActivity(searchWorkflowActivityDataModel xmodel)
    {
        try
        {
            // Get data from repository
            var Ldata = await _repository.GetAllAsyncSearch_MWorkflowActivity(xmodel);

            if (Ldata == null || !Ldata.Any())
            {
                await BatchEndOfDay_MWorkflowActivity(xmodel);

                var Ldata2 = await _repository.GetAllAsyncSearch_MWorkflowActivity(xmodel);
                if (Ldata2 == null || !Ldata2.Any())
                {
                    
                    return new WorkflowActivityApiResponse
                    {
                        ResponseMsg = "No data found",
                        ResponseCode = "200",
                        Data= new List<WorkflowActivityDataModel>()
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
            return new WorkflowActivityApiResponse
            {
                ResponseCode = "500",
                ResponseMsg = "Internal Server Error",

                Data = new List<WorkflowActivityDataModel>()
            };
        }
    }

    private WorkflowActivityApiResponse BuildApiResponse(IEnumerable<MWorkflowActivity> data)
    {
        return new WorkflowActivityApiResponse
        {
            ResponseCode = "200",
            ResponseMsg = "OK",
            Data = data.Select(d => new WorkflowActivityDataModel
            {
                WorkflowCode = d.WorkflowCode,
                WorkflowName = d.WorkflowName,
                WorkflowType = d.WorkflowType,
                WorkflowGoupCode = d.WorkflowGroupCode,
                Period = d.Period,
               
                ActivityDetails = d.TWorkflowActivities?.Select(a => new ActivityDetailModel
                {
                    ControlPoint = a.ControlPoint,
                    Activity = a.Activity,
                    Description = a.Description
                }).ToList()
            }).ToList()
        };
    }


}

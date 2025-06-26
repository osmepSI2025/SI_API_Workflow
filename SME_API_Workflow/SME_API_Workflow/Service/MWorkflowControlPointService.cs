using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SME_API_Workflow.Entities;
using SME_API_Workflow.Models;
using SME_API_Workflow.Repository;
using SME_API_Workflow.Services;
public class MWorkflowControlPointService 
{
    private readonly MWorkflowControlPointRepository _repository;
    private readonly ICallAPIService _serviceApi;
    private readonly IApiInformationRepository _repositoryApi;
    private readonly string _FlagDev;
    public MWorkflowControlPointService(MWorkflowControlPointRepository repository
            , IConfiguration configuration, ICallAPIService serviceApi, IApiInformationRepository repositoryApi)
    {
        _repository = repository;
        _serviceApi = serviceApi;
        _repositoryApi = repositoryApi;
        _FlagDev = configuration["Devlopment:FlagDev"] ?? throw new ArgumentNullException("FlagDev is missing in appsettings.json");

    }

    public async Task<IEnumerable<MWorkflowControlPoint>> GetAllAsync()
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

    public async Task<MWorkflowControlPoint?> GetByIdAsync(string WorkflowCode)
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

    public async Task AddAsync(MWorkflowControlPoint workflow)
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

    public async Task UpdateAsync(MWorkflowControlPoint workflow)
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

    public async Task BatchEndOfDay_MWorkflowControlPoints(searchWorkflowControlPointDataModel xmodel)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        var WorkflowControlPointApiResponse = new WorkflowControlPointApiResponse();
        var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "WorkflowControlPoint" });
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
        var result = JsonSerializer.Deserialize<WorkflowControlPointApiResponse>(apiResponse, options);

        WorkflowControlPointApiResponse = result ?? new WorkflowControlPointApiResponse();

        if (WorkflowControlPointApiResponse.Data != null)
        {
            foreach (var item in WorkflowControlPointApiResponse.Data)
            {
                try
                {
                    var existing = await _repository.GetByIdAsync(item.WorkflowCode);

                    if (existing == null)
                    {
                        // Create new record
                        var newData = new MWorkflowControlPoint
                        {
                            WorkflowCode = item.WorkflowCode,
                            WorkflowName = item.WorkflowName,
                            WorkflowType = item.WorkflowType,
                            WorkflowGroupCode = item.WorkflowGoupCode,
                            Period = item.Period,
                            TWorkflowControlPointActivityDetails = item.ActivityDetails?.Select(a => new TWorkflowControlPointActivityDetail
                            {
                                ControlPoint = a.ControlPoint,
                                Activity = a.Activity,
                                Description = a.Description
                            }).ToList() ?? new List<TWorkflowControlPointActivityDetail>()
                        };

                        await _repository.AddAsync(newData);
                        Console.WriteLine($"[INFO] Created new MWorkflowControlPoints with WorkflowCode {newData.WorkflowCode}");
                    }
                    else
                    {
                        // Update existing record
                        existing.WorkflowName = item.WorkflowName;
                        existing.WorkflowType = item.WorkflowType;
                        existing.WorkflowGroupCode = item.WorkflowGoupCode;
                        existing.Period = item.Period;

                        // Update TWorkflowControlPointActivityDetails
                        existing.TWorkflowControlPointActivityDetails.Clear();
                        if (item.ActivityDetails != null)
                        {
                            foreach (var a in item.ActivityDetails)
                            {
                                existing.TWorkflowControlPointActivityDetails.Add(new TWorkflowControlPointActivityDetail
                                {
                                    ControlPoint = a.ControlPoint,
                                    Activity = a.Activity,
                                    Description = a.Description
                                });
                            }
                        }

                        await _repository.UpdateAsync(existing);
                        Console.WriteLine($"[INFO] Updated MWorkflowControlPoints with WorkflowCode {existing.WorkflowCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process MWorkflowControlPoints with WorkflowCode {item.WorkflowCode}: {ex.Message}");
                }
            }
        }


    }
    public async Task<WorkflowControlPointApiResponse> GetAllAsyncSearch_MWorkflowControlPoints(searchWorkflowControlPointDataModel xmodel)
    {
        try
        {
            // Get data from repository
            var Ldata = await _repository.GetAllAsyncSearch_MWorkflowControlPoints(xmodel);

            if (Ldata == null || !Ldata.Any())
            {
                await BatchEndOfDay_MWorkflowControlPoints(xmodel);

                var Ldata2 = await _repository.GetAllAsyncSearch_MWorkflowControlPoints(xmodel);
                if (Ldata2 == null || !Ldata2.Any())
                {
                   
                    return new WorkflowControlPointApiResponse
                    {
                        ResponseMsg = "No data found",
                        ResponseCode = "200",
                        Data  = new List<WorkflowControlPointDataModel>()
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
            return new WorkflowControlPointApiResponse
            {
                ResponseCode = "500",
                ResponseMsg = "Internal Server Error",

                Data = new List<WorkflowControlPointDataModel>()
            };
        }
    }

    private WorkflowControlPointApiResponse BuildApiResponse(IEnumerable<MWorkflowControlPoint> data)
    {
        return new WorkflowControlPointApiResponse
        {
            ResponseCode = "200",
            ResponseMsg = "OK",
            Data = data.Select(d => new WorkflowControlPointDataModel
            {
                WorkflowCode = d.WorkflowCode,
                WorkflowName = d.WorkflowName,
                WorkflowType = d.WorkflowType,
                WorkflowGoupCode = d.WorkflowGroupCode,
                Period = d.Period,
                ActivityDetails = d.TWorkflowControlPointActivityDetails?.Select(a => new WorkflowControlPointActivityDetail
                {
                    ControlPoint = a.ControlPoint,
                    Activity = a.Activity,
                    Description = a.Description
                }).ToList()
            }).ToList()
        };
    }


}

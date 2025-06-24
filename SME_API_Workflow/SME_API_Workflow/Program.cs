using Microsoft.EntityFrameworkCore;
using SME_API_Workflow.Entities;
using SME_API_Workflow.Repository;
using SME_API_Workflow.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<Si_WorkflowDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
//Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ Register NSwag (Swagger 2.0)
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "API_Worflow_v1";
    config.Title = "API SME Worflow";
    config.Version = "v1";
    config.Description = "API documentation using Swagger 2.0";
    config.SchemaType = NJsonSchema.SchemaType.Swagger2; // This makes it Swagger 2.0
});


builder.Services.AddScoped<MWorkflowRepository>();
builder.Services.AddScoped<MWorkflowService>();
builder.Services.AddScoped<MWorkflowActivityRepository>();
builder.Services.AddScoped<MWorkflowActivityService>();
builder.Services.AddScoped<MWorkflowControlPointRepository>();
builder.Services.AddScoped<MWorkflowControlPointService>();



builder.Services.AddScoped<MWorkflowLeadingLaggingRepository>();
builder.Services.AddScoped<MWorkflowLeadingLaggingService>();
 




builder.Services.AddScoped<IApiInformationRepository, ApiInformationRepository>();
builder.Services.AddScoped<ICallAPIService, CallAPIService>(); // Register ICallAPIService with CallAPIService
builder.Services.AddHttpClient<CallAPIService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseOpenApi();     // Serve the Swagger JSON
    app.UseSwaggerUi3();  // Use Swagger UI v3
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

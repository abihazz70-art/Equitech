using Microsoft.EntityFrameworkCore;
using Equitech.AiAgent.Engine;
using Equitech.AiAgent.Interfaces;
using Equitech.Api.Middleware;
using Equitech.Domain.Interfaces;
using Equitech.Infrastructure.Data;
using Equitech.Infrastructure.Persistence;
using Equitech.Infrastructure.Repositories;

// Load environment variables from .env file (traversing parent directories to find root or local .env)
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Equitech Job Search & Career Development API",
        Version = "v1",
        Description = "Enterprise REST API adhering to Implementation Blueprint v1.0 Section 8. Features ProblemDetails (RFC 7807), Correlation IDs (X-Correlation-ID), pagination cursors, and server-side schema validation."
    });
});

// Configure CORS for Vue 3 SPA frontend (reading FRONTEND_URL from .env if present)
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:5173";
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendSpa", policy =>
    {
        policy.WithOrigins(frontendUrl, "http://localhost:5173", "http://127.0.0.1:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configure Supabase PostgreSQL Connection strictly via .env / Environment Variables
var supabaseConnection = Environment.GetEnvironmentVariable("SUPABASE_CONNECTION_STRING")
    ?? Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("SupabaseConnection");

if (!string.IsNullOrWhiteSpace(supabaseConnection) && 
    !supabaseConnection.Contains("placeholder") && 
    !supabaseConnection.Contains("YOUR_"))
{
    Console.WriteLine("--> Connecting to Supabase PostgreSQL Database from .env configuration...");
    builder.Services.AddDbContext<EquitechDbContext>(options =>
    {
        options.UseNpgsql(supabaseConnection, npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
        });
    });
}
else
{
    Console.WriteLine("--> Notice: SUPABASE_CONNECTION_STRING not set or placeholder in .env. Falling back to in-memory store for local testing.");
    builder.Services.AddDbContext<EquitechDbContext>(options =>
    {
        options.UseInMemoryDatabase("EquitechDevDb");
    });
}

// Standalone AI Agent Registration with credentials loaded via .env
var openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") 
    ?? builder.Configuration["OpenAI:ApiKey"];

builder.Services.AddHttpClient<IInterviewAiAgent, InterviewAiAgentEngine>();
builder.Services.AddSingleton<IInterviewAiAgent>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    return new InterviewAiAgentEngine(httpClient, openAiApiKey);
});

// Repository and Sample Data Services
builder.Services.AddScoped<IInterviewPackRepository, InterviewPackRepository>();
builder.Services.AddSingleton<ISampleDataProvider, SampleDataProvider>();

var app = builder.Build();

// Register Correlation ID Middleware (X-Correlation-ID header on all requests and responses)
app.UseMiddleware<CorrelationIdMiddleware>();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Equitech API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowFrontendSpa");
app.UseAuthorization();
app.MapControllers();

app.Run();

using MyCv.Application.Features.Projects.Queries;
using MyCv.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProjectsQuery).Assembly));
builder.Services.AddInfrastructure(builder.Configuration);

// Derive authority from MetadataAddress (strip the discovery path) or use Authority directly
var metadataAddress = builder.Configuration["Identity:MetadataAddress"] ?? "";
var authority = builder.Configuration["Identity:Authority"]
    ?? (metadataAddress.Contains("/.well-known")
        ? metadataAddress[..metadataAddress.IndexOf("/.well-known")]
        : metadataAddress);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.Authority = authority;
        opts.Audience = "mycv";
        opts.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.MapOpenApi();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.MapGet("/api/health", () => Results.Ok(new { status = "healthy" }));
app.MapFallbackToFile("index.html");

app.Run();

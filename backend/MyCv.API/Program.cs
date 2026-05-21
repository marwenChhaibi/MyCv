using MyCv.Application.Features.Projects.Queries;
using MyCv.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProjectsQuery).Assembly));
builder.Services.AddInfrastructure(builder.Configuration);

var metadataAddress = builder.Configuration["Identity:MetadataAddress"]
    ?? "https://urfidentity.xyz/.well-known/openid-configuration";
var authority = metadataAddress.Contains("/.well-known")
    ? metadataAddress[..metadataAddress.IndexOf("/.well-known")]
    : metadataAddress;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.Authority = authority;
        opts.MetadataAddress = metadataAddress;
        opts.Audience = "mycv";
        opts.RequireHttpsMetadata = false;
        opts.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
        };
        opts.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                var log = ctx.HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>().CreateLogger("JwtAuth");
                log.LogError(ctx.Exception, "JWT validation failed on {Path}", ctx.Request.Path);
                return Task.CompletedTask;
            },
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

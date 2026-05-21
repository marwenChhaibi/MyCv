using MyCv.Application.Features.Projects.Queries;
using MyCv.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProjectsQuery).Assembly));
builder.Services.AddInfrastructure(builder.Configuration);

var metadataAddress = builder.Configuration["Identity:MetadataAddress"]!;
var jwksUrl = metadataAddress.Replace(".well-known/openid-configuration", ".well-known/jwks");

SecurityKey[] signingKeyCache = [];
DateTime cacheUntil = DateTime.MinValue;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.Audience = "mycv";
        opts.RequireHttpsMetadata = false;
        opts.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            IssuerSigningKeyResolver = (_, _, _, _) =>
            {
                if (DateTime.UtcNow > cacheUntil)
                {
                    try
                    {
                        using var http = new HttpClient();
                        var json = http.GetStringAsync(jwksUrl).GetAwaiter().GetResult();
                        signingKeyCache = new JsonWebKeySet(json).GetSigningKeys().ToArray();
                        cacheUntil = DateTime.UtcNow.AddMinutes(10);
                    }
                    catch { }
                }
                return signingKeyCache;
            }
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

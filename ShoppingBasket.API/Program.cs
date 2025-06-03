using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.API.Configurations;
using ShoppingBasket.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddSwaggerSetup();

// Persistence
builder.Services.AddPersistenceSetup(builder.Configuration);

//Endpoints
builder.Services.AddEndpoints(typeof(Program).Assembly);

// MediatR
builder.Services.AddMediatRSetup();

// Domain Services
builder.Services.AddDomaimServicesSetup();

// Add identity stuff
builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddProblemDetails();

builder.Services.AddAuthorization();

builder.Services.AddApplicationSetup();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()  
            .AllowAnyMethod()
            .AllowAnyHeader(); 
    });
});

var app = builder.Build();
app.UseCors("AllowAll");


app.UseAuthentication();
app.UseAuthorization();


// Swagger
app.UseSwaggerSetup();
app.UseHttpsRedirection();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            var problem = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "Internal Server Error",
                Detail = contextFeature.Error.Message
            };

            await context.Response.WriteAsJsonAsync(problem);
        }
    });
});

//Endpoints
app.MapEndpoints();
app.MapGroup("api/v1/identity")
    .WithTags("Identity")
    .MapIdentityApi<ApplicationUser>();

app.Run();

public partial class Program { }
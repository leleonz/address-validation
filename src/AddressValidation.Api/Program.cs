using AddressValidation.Api.DependencyInjectors;
using AddressValidation.Api.Endpoints;
using AddressValidation.Api.Middlewares;
using AddressValidation.Data.Persistences;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .Build());
});

services.AddDbContext<AddressValidationDb>(options => options.UseInMemoryDatabase("AddressValidationDatabase"));

//remove keys with null value from json responses
services.Configure<JsonOptions>(options =>
         options.SerializerOptions.DefaultIgnoreCondition
   = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull);

services.AddTransient<GenericExceptionHandler>();
services.RegisterValidators();
services.RegisterRepositories();
services.RegisterServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    //this is simplified error handling, introduce an endpoint for better implementation
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/html";

            await context.Response.WriteAsync("<p>Oops ... something went wrong!</p>");
        });
    });
    app.UseHsts();
}

app.UseStatusCodePages();
app.UseCors();
app.UseMiddleware<GenericExceptionHandler>();

app.BuildAddressValidatorWebApis();

app.Run();

public partial class Program { }

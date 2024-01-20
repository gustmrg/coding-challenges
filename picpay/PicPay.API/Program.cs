using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PicPay.API.Data;
using PicPay.API.Models.Request;
using PicPay.API.Models.Response;
using PicPay.API.Validators;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton(new RestClient(new HttpClient()));

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => JsonSerializer.Deserialize<Error>(e.ErrorMessage));
            return new BadRequestObjectResult(errors);
        };
    });

builder.Services.AddScoped<IValidator<CreateUserRequestModel>, CreateUserRequestValidator>();
builder.Services.AddScoped<IValidator<CreateTransactionRequestModel>, CreateTransactionRequestValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
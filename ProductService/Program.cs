using ECommerce.Shared.Extensions;
using ProductService.Core;
using ProductService.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ProductCore>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConsulConfig(builder.Configuration);
// Bind JWTSettings
var jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<ECommerce.Shared.Models.JWTSettings>();
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddJwtAuthentication(jwtSettings);


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ECommerce.Shared.Middleware.ExceptionHandlingMiddleware>();

app.UseConsul();
app.MapControllers();
app.Run();

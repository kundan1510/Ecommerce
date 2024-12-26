using CartService.Service.IService;
using CartService.Service;
using CartService.Kafka;
using Serilog;
using ECommerce.Shared.Middleware;
using CartService.Repositories;
using CartService.Core;

var builder = WebApplication.CreateBuilder(args);


// Configure Serilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddSingleton<IcartRepository, CartRepository>();
builder.Services.AddTransient<CartCore>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddConsulConfig(builder.Configuration);

builder.Services.AddHttpClient("ProductService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7081/"); // Replace with Product Service base URL
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();
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

//app.UseSerilogRequestLogging(); // Log HTTP requests
//app.UseMiddleware<LogHeaderMiddleware>();
app.MapControllers();


app.Run();

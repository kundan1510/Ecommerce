using ECommerce.Shared.Middleware;
using NotificationService.kafka;
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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConsulConfig(builder.Configuration);
builder.Services.AddHostedService<KafkaConsumer>();

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

//app.UseSerilogRequestLogging(); // Log HTTP requests
//app.UseMiddleware<LogHeaderMiddleware>();
app.UseConsul();
app.MapControllers();


app.Run();

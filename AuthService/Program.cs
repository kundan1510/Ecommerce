using ECommerce.Shared.Extensions;


var builder = WebApplication.CreateBuilder(args);


// Bind JWTSettings
var jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<ECommerce.Shared.Models.JWTSettings>();
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddJwtAuthentication(jwtSettings);

// Register AuthService
builder.Services.AddSingleton<ECommerce.Shared.Service.AuthService>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

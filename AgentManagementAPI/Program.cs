using AgentManagementAPI.Controllers;
using AgentManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using AgentManagementAPI.Services;
using AgentManagementAPI.Middlewares.Global;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DbContextAPI>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ServiceTrget>();
builder.Services.AddScoped<ServiceAgent>();
builder.Services.AddScoped<ServiceMissions>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalLoggingMiddleware>();


app.UseWhen(
    context =>
        context.Request.Path.StartsWithSegments("/api/attacks"),
    appBuilder =>
    {
        // appBuilder.UseMiddleware<JwtValidationMiddleware>();
        appBuilder.UseMiddleware<GlobalLoggingMiddleware>();
    });
app.UseWebSockets();

app.UseHttpsRedirection();


app.MapControllers();



app.Run();

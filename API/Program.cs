using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Services.Abstractions;
using API.Services.One_off;
using API.Services.Periodic;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//this is required for recurring background service to work, but not required for one-off
builder.Services.Configure<HostOptions>(options =>
{
    options.ServicesStartConcurrently = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("Clients"));

builder.Services.AddTransient<IWorker, Worker>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddTransient<IPeriodicTimer, DailyPeriodicTimer>();

//One-off Background Tasks With IHostedLifecycleService
builder.Services.AddHostedService<InitializationHostedLifecycleService>();

//One-off Background Tasks With IHostedService
//builder.Services.AddHostedService<InitializationHostedService>();

//One-off Background Tasks With BackgroundService
//builder.Services.AddHostedService<InitializationBackgroundService>();

//recurring Background Tasks With IHostedService
//builder.Services.AddHostedService<PeriodicHostedService>();

//recurring Background Tasks With BackgroundService
//builder.Services.AddHostedService<PeriodicBackgroundService>();

//recurring Background Tasks With IHostedLifecycleService
builder.Services.AddHostedService<PeriodicHostedLifecycleService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/clients/active", (ApplicationDbContext context) =>
{
    return context.Clients.Where(x => x.IsActive).AsNoTracking();
});

app.MapGet("/clients/archived", (ApplicationDbContext context) =>
{
    return context.Clients.Where(x => !x.IsActive).AsNoTracking();
});
app.Run();

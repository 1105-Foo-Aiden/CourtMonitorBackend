using CourtMonitorBackend.Services;
using CourtMonitorBackend.Services.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<UserService>();

var connectionString = builder.Configuration.GetConnectionString("CourtMonitorString");
//configures entity framework core to use SQL server as the database provider for a dataContext
builder.Services.AddDbContext<DataContext>(Options => Options.UseSqlServer(connectionString));

builder.Services.AddCors(options => options.AddPolicy("CourtMonitorPolicy", builder =>{
    builder.WithOrigins("http://localhost:3000", "http://localhost:5169", "https://court-monitor-two.vercel.app")
    .AllowAnyHeader()
    .AllowAnyMethod();
}));

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
// app.UseHttpsRedirection();
app.UseCors("CourtMonitorPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

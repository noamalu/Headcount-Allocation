using API.Services;
using EcommerceAPI.initialize;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;
using HeadcountAllocation.Services;
using WebSocketSharp.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<ManagerFacade>(); 
builder.Services.AddSingleton<HeadCountService>(); 
builder.Services.AddSingleton<ProjectService>(); 
builder.Services.AddSingleton<EmployeeService>(); 
builder.Services.AddSingleton<WebSocketServer>(sp =>
{
    // var configurate = sp.GetRequiredService<Configurate>();
    // string port = configurate.Parse();
    var alertServer = new WebSocketServer("ws://127.0.0.1:4562");
    alertServer.Start();
    return alertServer;
});
           
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("http://localhost:5173") // Frontend URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // Add this line to apply the CORS policy

app.UseAuthorization();
app.MapControllers();

app.Run();

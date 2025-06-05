using API.Services;
using EcommerceAPI.initialize;
using Hangfire;
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
            .WithOrigins
                ("http://localhost:5173", "http://132.73.84.247") // Frontend URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var context = HeadcountAllocation.DAL.DBcontext.GetInstance();
// context.Dispose();
// context.ClearDatabase();
context.SeedStaticTables(); // Uncomment if you need to seed static tables

builder.Services.AddHangfire(configuration => 
    configuration.UseSqlServerStorage("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HeadCountDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;Application Intent=ReadWrite;MultiSubnetFailover=False")); // TODO: replace this
builder.Services.AddHangfireServer();

var app = builder.Build();
app.UseHangfireDashboard("/jobs"); // http://localhost:port/jobs


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // Enable CORS for all requests

app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"🔥 Unhandled Exception: {ex.Message}\n{ex.StackTrace}");
        throw; // Re-throw so the default handler still kicks in
    }
});

app.UseAuthorization();
app.MapControllers();

app.Run();

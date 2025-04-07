using API.Services;
using EcommerceAPI.initialize;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;
using HeadcountAllocation.Services;
using Microsoft.AspNetCore.Diagnostics;
using WebSocketSharp.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<ManagerFacade>(); 
builder.Services.AddSingleton<HeadCountService>(); 
builder.Services.AddSingleton<ProjectService>(); 
builder.Services.AddSingleton<EmployeeService>(); 
// builder.Services.AddSingleton<WebSocketServer>(sp =>
// {
//     var configurate = sp.GetRequiredService<Configurate>();
//     string port = configurate.Parse();
//     var alertServer = new WebSocketServer("ws://127.0.0.1:" + port);
//     alertServer.Start();
//     return alertServer;
// });
           
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

// var context = HeadcountAllocation.DAL.DBcontext.GetInstance();
// context.Dispose();
// // context.ClearDatabase();

// TimeZonesDTO morning = new TimeZonesDTO(HeadcountAllocation.Domain.Enums.TimeZones.Morning);
// TimeZonesDTO noon = new TimeZonesDTO(HeadcountAllocation.Domain.Enums.TimeZones.Noon);
// TimeZonesDTO evening = new TimeZonesDTO(HeadcountAllocation.Domain.Enums.TimeZones.Evening);
// TimeZonesDTO flexible = new TimeZonesDTO(HeadcountAllocation.Domain.Enums.TimeZones.Flexible);
// context.TimeZones.Add(morning);
// context.TimeZones.Add(noon);
// context.TimeZones.Add(evening);
// context.TimeZones.Add(flexible);
// context.SaveChanges();

// LanguageTypesDTO english = new LanguageTypesDTO(HeadcountAllocation.Domain.Enums.Languages.English);
// LanguageTypesDTO hebrew = new LanguageTypesDTO(HeadcountAllocation.Domain.Enums.Languages.Hebrew);
// context.LanguageTypes.Add(english);
// context.LanguageTypes.Add(hebrew);
// context.SaveChanges();

// SkillTypesDTO python = new SkillTypesDTO(HeadcountAllocation.Domain.Enums.Skills.Python);
// SkillTypesDTO sql = new SkillTypesDTO(HeadcountAllocation.Domain.Enums.Skills.SQL);
// SkillTypesDTO api = new SkillTypesDTO(HeadcountAllocation.Domain.Enums.Skills.API);
// SkillTypesDTO java = new SkillTypesDTO(HeadcountAllocation.Domain.Enums.Skills.Java);
// SkillTypesDTO ui = new SkillTypesDTO(HeadcountAllocation.Domain.Enums.Skills.UI);
// context.SkillTypes.Add(python);
// context.SkillTypes.Add(sql);
// context.SkillTypes.Add(api);
// context.SkillTypes.Add(java);
// context.SkillTypes.Add(ui);
// context.SaveChanges();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // Enable CORS for all requests

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        Console.WriteLine($"🔥 [ExceptionHandler] {exception?.Message}\n{exception?.StackTrace}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Internal server error");
    });
});

app.UseAuthorization();
app.MapControllers();

app.Run();

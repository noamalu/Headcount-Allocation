using API.Services;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;
using HeadcountAllocation.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<ManagerFacade>(); 
builder.Services.AddSingleton<HeadCountService>(); 
builder.Services.AddSingleton<ProjectService>(); 
builder.Services.AddSingleton<EmployeeService>(); 

            // perfect employee
            List<EmployeeLanguagesDTO> emp_languages = new ();
            emp_languages.Add(new (0, 0, 1)); //english
            emp_languages.Add(new (1, 0, 3)); //hebrew
            List<EmployeeSkillsDTO> emp_skills = new ();
            emp_skills.Add(new (0, 2, 0)); //python
            emp_skills.Add(new (2, 2, 0)); //api
            emp_skills.Add(new (3, 2, 0)); //java
            EmployeeDTO employee0 = new (0, "Stas", "0545382555", "stas@intel.com", 3, emp_languages, 1, emp_skills, new (), 10, "pass", false);

            // all over employee
            List<EmployeeLanguagesDTO> emp_languages1 = new ();
            emp_languages1.Add(new (0, 1, 1)); //english
            emp_languages1.Add(new (1, 1, 3)); //hebrew
            List<EmployeeSkillsDTO> emp_skills1 = new ();
            emp_skills1.Add(new EmployeeSkillsDTO(0, 3, 0)); //python
            emp_skills1.Add(new EmployeeSkillsDTO(2, 3, 0)); //api
            emp_skills1.Add(new EmployeeSkillsDTO(3, 3, 0)); //java
            EmployeeDTO employee1 = new (1, "Chava", "0528972555", "chava@intel.com", 0, emp_languages1, 1, emp_skills1, new (), 10, "pass", false);

            // all below employee
            List<EmployeeLanguagesDTO> emp_languages2 = new ();
            emp_languages2.Add(new (0, 2, 1));
            emp_languages2.Add(new (1, 2, 3));
            List<EmployeeSkillsDTO> emp_skills2 = new ();
            emp_skills2.Add(new (0, 1, 0));
            emp_skills2.Add(new (2, 1, 0));
            emp_skills2.Add(new (3, 1, 0));
            EmployeeDTO employee2 = new (2, "Michal", "0529587456", "michal@intel.com", 0, emp_languages2, 1, emp_skills2, new (), 10, "pass", false);

            // no languages employee
            List<EmployeeLanguagesDTO> emp_languages3 = new ();
            emp_languages3.Add(new (0, 3, 1));
            emp_languages3.Add(new (1, 3, 1));
            List<EmployeeSkillsDTO> emp_skills3 = new ();
            emp_skills3.Add(new (0, 2, 0));
            emp_skills3.Add(new (2, 2, 0));
            emp_skills3.Add(new (3, 2, 0));
            EmployeeDTO employee3 = new (3, "Asaf", "0549647682", "asaf@intel.com", 0, emp_languages3, 1, emp_skills3, new (), 10, "pass", false);

            // all below employee
            List<EmployeeLanguagesDTO> emp_languages4 = new List<EmployeeLanguagesDTO>();
            emp_languages4.Add(new (0, 4, 1));
            emp_languages4.Add(new (1, 4, 3));
            List<EmployeeSkillsDTO> emp_skills4 = new ();
            emp_skills4.Add(new (0, 3, 0));
            emp_skills4.Add(new (2, 3, 0));
            EmployeeDTO employee4 = new (4, "Harel", "0523664965", "harel@intel.com", 0, emp_languages4, 1, emp_skills4, new (), 10, "pass", false);

            HeadcountAllocation.Domain.ManagerFacade managerFacade = HeadcountAllocation.Domain.ManagerFacade.GetInstance();
            HeadcountAllocation.Domain.Employee emp0 = new (employee0);
            HeadcountAllocation.Domain.Employee emp1 = new (employee1);
            HeadcountAllocation.Domain.Employee emp2 = new (employee2);
            HeadcountAllocation.Domain.Employee emp3 = new (employee3);
            HeadcountAllocation.Domain.Employee emp4 = new (employee4);
            managerFacade.Employees.TryAdd(emp0.EmployeeId, emp0);
            managerFacade.Employees.TryAdd(emp1.EmployeeId, emp1);
            managerFacade.Employees.TryAdd(emp2.EmployeeId, emp2);
            managerFacade.Employees.TryAdd(emp3.EmployeeId, emp3);
            managerFacade.Employees.TryAdd(emp4.EmployeeId, emp4);   

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // Add this line to apply the CORS policy

app.UseAuthorization();
app.MapControllers();

app.Run();

// using HeadcountAllocation.Services;
// using Newtonsoft.Json.Linq;
// using API.Services;
// using HeadcountAllocation.DAL;
// using HeadcountAllocation.DAL.Repositories;

// namespace EcommerceAPI.initialize;

// public class Configurate
// {
//         private readonly HeadCountService _service;
//         private readonly EmployeeService _employeeService;



//     public Configurate(HeadCountService service,  EmployeeService employeeService)
//     {
//         _service = service;
//         _employeeService = employeeService;
//     }

//     public string Parse(string PATH = null)
//     {
//         PATH ??= Path.Combine(Environment.CurrentDirectory, "initialize\\config.json");        

//         string textJson = "";
//         try
//         {
//             // textJson = File.ReadAllText(PATH);
//         }
//         catch (Exception e)
//         {            
//             throw new Exception("open initializing file fail");
//         }
//         JObject scenarioDtoDict = JObject.Parse(textJson);
//         try
//         {
//             // if (scenarioDtoDict["Test"].Value<bool>())
//             // {
//             //     DBcontext.SetTestDB();
//             // }
//             // else if (scenarioDtoDict["Local"].Value<bool>())
//             // {
//             //     DBcontext.SetLocalDB();
//             // }
//             // else
//             // {
//             //     MyLogger.GetLogger().Info("configured remote DB");
//             //     DBcontext.SetRemoteDB();
//             // }

//             // if (scenarioDtoDict["Initialize"].Value<string>() == InitializeOptions.File.GetDescription())
//             // {                
//             //     string initPATH = Path.Combine(Environment.CurrentDirectory, "initialize\\" + scenarioDtoDict["InitialState"]);
//             //     DBcontext.GetInstance().Dispose();
//             //     RoleRepo.Dispose();
//             //     EmployeeRepo.Dispose();
//             //     ProjectRepo.Dispose();
//             //     RoleSkillsRepo.Dispose();
//             //     EmployeeLanguagesRepo.Dispose();
//             //     EmployeeSkillsRepo.Dispose();
//             //     RoleLanguagesRepo.Dispose();
//             //     RoleSkillsRepo.Dispose();
//             //     // try{
//             //     //     new SceanarioParser(_service, _employeeService).Parse(initPATH).Wait();
//             //     //     MyLogger.GetLogger().Info("Initialize from file");

//             //     // }catch(Exception){
//             //     //     MyLogger.GetLogger().Info("Initialize from file failed");
//             //     // }
//             // }else if(scenarioDtoDict["Initialize"].Value<string>() == InitializeOptions.Empty.GetDescription()){
//             //     DBcontext.GetInstance().Dispose();
//             //     DBcontext.GetInstance().Dispose();
//             //     RoleRepo.Dispose();
//             //     EmployeeRepo.Dispose();
//             //     ProjectRepo.Dispose();
//             //     RoleSkillsRepo.Dispose();
//             //     EmployeeLanguagesRepo.Dispose();
//             //     EmployeeSkillsRepo.Dispose();
//             //     RoleLanguagesRepo.Dispose();
//             //     RoleSkillsRepo.Dispose();              
//             //     // _service.RegisterAsSystemAdmin(scenarioDtoDict["AdminUsername"].Value<string>(), scenarioDtoDict["AdminPassword"].Value<string>(), scenarioDtoDict["AdminEmail"].Value<string>(), scenarioDtoDict["AdminAge"].Value<int>());
//             //     //     MyLogger.GetLogger().Info("Initialize empty");
//             // }
//             // else
//             // {
//             //         // MyLogger.GetLogger().Info("Initialize from DB");
//             // }
            
//             // var port = scenarioDtoDict["Port"].ToString();
//             // return port;
//         }
//         catch (Exception ex)
//         {
//             // Log the exception or handle it as necessary
//             // MyLogger.GetLogger().Info("Failed during configuration");
//             throw new Exception("Failed during configuration", ex);
//         }
//     }

//     public static bool VerifyJsonStructure(string filePath)
//     {
//         string expectedJson = @"
//         {        
//             ""AdminUsername"": ""string"",
//             ""AdminPassword"": ""string"",
//             ""InitialState"": ""string"",
//             ""Port"": 0,
//             ""ExternalServices"": false,        
//             ""Local"": false,
//             ""Initialize"": true
//         }";

//         JObject expectedObject = JObject.Parse(expectedJson);
//         JObject actualObject = JObject.Parse(System.IO.File.ReadAllText(filePath));

//         foreach (var property in expectedObject.Properties())
//         {
//             if (!actualObject.ContainsKey(property.Name) ||
//                 actualObject[property.Name].Type != GetJTokenType(property.Value))
//             {
//                 return false;
//             }
//         }

//         return true;
//     }

//     private static JTokenType GetJTokenType(JToken value)
//     {
//         if (value.Type == JTokenType.String)
//         {
//             return JTokenType.String;
//         }
//         else if (value.Type == JTokenType.Boolean)
//         {
//             return JTokenType.Boolean;
//         }
//         else if (value.Type == JTokenType.Integer || value.Type == JTokenType.Float)
//         {
//             return JTokenType.Integer;
//         }

//         return JTokenType.Null;
//     }
// }

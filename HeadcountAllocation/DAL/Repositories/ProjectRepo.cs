using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;

namespace HeadcountAllocation.DAL.Repositories
{
    public class ProjectRepo
    {
        private static ConcurrentDictionary<int, Project> _projects;
        private static ProjectRepo _projectRepo = null;

        public ConcurrentDictionary<int, Project> Projects { get => _projects; set => _projects = value; }
        private object _lock ;

        private ProjectRepo()
        {
            _projects = new ConcurrentDictionary<int, Project>();
            _lock = new object();
           
        }
        public static ProjectRepo GetInstance()
        {
            if (_projectRepo == null)
                _projectRepo = new ProjectRepo();
            return _projectRepo;
        }

        public static void Dispose(){
            _projectRepo = new ProjectRepo();
        }
        public void Add(Project project)
        {
            _projects.TryAdd(project.ProjectId, project);
            try{
                lock(_lock){
                    DBcontext.GetInstance().Projects.Add(new ProjectDTO(project));
                    DBcontext.GetInstance().SaveChanges();
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Add Project");
            }
        }

        public void Delete(Project project)
        {
        try{
            lock (_lock)
            {
                bool shopInDomain = _projects.TryRemove(project.ProjectId, out _);
                DBcontext context = DBcontext.GetInstance();
                ProjectDTO projectDTO = context.Projects.Find(project.ProjectId);
                if (shopInDomain)
                {
                    context.Projects.Remove(projectDTO);
                    context.SaveChanges();
                }
                else if (projectDTO != null)
                {
                    context.Projects.Remove(projectDTO);
                    context.SaveChanges();
                }
            }
        }
        catch(Exception){
                throw new Exception("There was a problem in Database use- Delete Project");
        }

        
        }

        // public IEnumerable<Store> getAll()
        // {
        //     try{
        //         lock (_lock)
        //         {
        //             List<StoreDTO> storesList = DBcontext.GetInstance().Stores.ToList();
        //             foreach (StoreDTO storeDTO in storesList)
        //             {
        //                 List<ProductDTO> products = DBcontext.GetInstance().Products.ToList();
        //                 foreach (ProductDTO productDTO in products)
        //                 {
        //                     if (productDTO.ProductId / 10 == storeDTO.Id){
        //                         storeDTO.Products.Add(productDTO);
        //                     }
        //                 }
        //                 List<PurchaseDTO> purchases = DBcontext.GetInstance().Purchases.ToList();
        //                 foreach (PurchaseDTO purchaseDTO in purchases)
        //                 {
        //                     if (purchaseDTO.StoreId == storeDTO.Id){
        //                         storeDTO.Purchases.Add(purchaseDTO);
        //                     }
        //                 }
        //                 _projects.TryAdd(storeDTO.Id, new Store(storeDTO));
        //             }
                    
        //         }
        //     }
        //     catch(Exception){
        //         throw new Exception("There was a problem in Database use- Get all Stores");
        //     }
            
            
        //     return _projects.Values.ToList();
        // }

        public Project GetById(int id)
        {
            if (_projects.ContainsKey(id))
            {
                return _projects[id];
            }
            else{
                try{
                    lock (_lock)
                    {
                        ProjectDTO projectDTO = DBcontext.GetInstance().Projects.Find(id);
                        if (projectDTO != null)
                        {
                            Project project = new Project(projectDTO);
                            _projects.TryAdd(id, project);
                            //project.Initialize(projectDTO);
                            return project;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Get Project");
                }
                
            }
        }

        public void Update(Project project)
        {
            try{
                _projects[project.ProjectId] = project;
                lock(_lock){
                    ProjectDTO projectDTO = DBcontext.GetInstance().Projects.Find(project.ProjectId);
                    ProjectDTO newProject = new ProjectDTO(project);
                    if (projectDTO != null)
                    {
                        projectDTO.ProjectName = newProject.ProjectName;
                        projectDTO.Description = newProject.Description;
                        projectDTO.Date = newProject.Date;
                        projectDTO.RequiredHours = newProject.RequiredHours;
                        projectDTO.Roles = newProject.Roles;
                    }
                    else DBcontext.GetInstance().Projects.Add(newProject);
                    DBcontext.GetInstance().SaveChanges();
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Update Project");
            }
            
        }
    }
 }

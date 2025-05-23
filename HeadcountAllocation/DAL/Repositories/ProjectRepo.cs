using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;
using Microsoft.EntityFrameworkCore;

namespace HeadcountAllocation.DAL.Repositories
{
    public class ProjectRepo
    {
        private static ConcurrentDictionary<int, Project> _projects;
        private static ProjectRepo _projectRepo = null;

        public ConcurrentDictionary<int, Project> Projects { get => _projects; set => _projects = value; }
        private object _lock;

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

        public static void Dispose()
        {
            _projectRepo = new ProjectRepo();
        }
        public void Add(Project project)
        {
            _projects.TryAdd(project.ProjectId, project);
            try
            {
                lock (_lock)
                {
                    DBcontext.GetInstance().Projects.Add(new ProjectDTO(project));
                    DBcontext.GetInstance().SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception("There was a problem in Database use- Add Project" + $" {e}");
            }
        }

        public void Delete(int projectId)
        {
            try
            {
                lock (_lock)
                {
                    bool shopInDomain = _projects.TryRemove(projectId, out _);
                    DBcontext context = DBcontext.GetInstance();
                    ProjectDTO projectDTO = context.Projects.Find(projectId);
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
            catch (Exception)
            {
                throw new Exception("There was a problem in Database use- Delete Project");
            }


        }

        public List<Project> GetAll()
        {
            Load();
            return Projects.Values.ToList();
        }

        public Project GetById(int id)
        {
            if (_projects.ContainsKey(id))
            {
                return _projects[id];
            }
            else
            {
                try
                {
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
                catch (Exception)
                {
                    throw new Exception("There was a problem in Database use- Get Project");
                }

            }
        }

        public void Update(Project project)
        {
            try
            {
                _projects[project.ProjectId] = project;
                lock (_lock)
                {
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
            catch (Exception)
            {
                throw new Exception("There was a problem in Database use- Update Project");
            }

        }


        private void Load()
        {
            var dbContext = DBcontext.GetInstance();
            // List<ProjectDTO> projects = dbContext.Projects.Include(e => e.Roles.Include()).ToList();
            List<ProjectDTO> projects = dbContext.Projects
                .Include(p => p.Roles)
                    .ThenInclude(r => r.Skills)
                .Include(p => p.Roles)
                    .ThenInclude(r => r.ForeignLanguages)
                .ToList();

            foreach (ProjectDTO project in projects)
            {
                Projects.TryAdd(project.ProjectId, new Project(project));

            }
        }
    }
}

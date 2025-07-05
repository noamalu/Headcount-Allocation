import React, { createContext, useContext, useState } from 'react';
import { Project } from '../Types/ProjectType';
import { Role } from '../Types/RoleType';
import { Employee } from '../Types/EmployeeType';
import { Ticket } from '../Types/TicketType';

interface DataContextType {
  projects: Project[];
  roles: Role[];
  employees: Employee[];
  tickets: Ticket[];

  setProjects: React.Dispatch<React.SetStateAction<Project[]>>;
  setRoles: React.Dispatch<React.SetStateAction<Role[]>>;
  setEmployees: React.Dispatch<React.SetStateAction<Employee[]>>;
  setTickets: React.Dispatch<React.SetStateAction<Ticket[]>>;

  addProject: (project: Project) => void;
  updateProject: (project: Project) => void;
  deleteProject: (projectId: number) => void;

  addRole: (role: Role) => void;
  addRolesIfNotExist: (newRoles: Role[]) => void;
  addOrUpdateRoles: (newRoles: Role[]) => void;
  updateRole: (role: Role) => void;
  deleteRole: (roleId: number) => void;

  addEmployee: (employee: Employee) => void;
  updateEmployee: (employee: Employee) => void;
  deleteEmployee: (employeeId: number) => void;

  addTicket: (ticket: Ticket) => void;
  updateTicket: (ticket: Ticket) => void;
  deleteTicket: (ticketId: number) => void;
}

const DataContext = createContext<DataContextType | undefined>(undefined);

export const DataProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [projects, setProjects] = useState<Project[]>([]);
  const [roles, setRoles] = useState<Role[]>([]);
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [tickets, setTickets] = useState<Ticket[]>([]);

  const addProject = (project: Project) => {
    setProjects((prev) => [...prev, project]);
  };

  const updateProject = (updated: Project) => {
    setProjects((prev) =>
      prev.map((p) => (p.projectId === updated.projectId ? updated : p))
    );
  };

  const deleteProject = (id: number) => {
    setProjects((prev) => prev.filter((p) => p.projectId !== id));
  };

  const addRole = (role: Role) => {
    setRoles((prev) => [...prev, role]);
  };

  const addRolesIfNotExist = (newRoles: Role[]) => {
    setRoles((prevRoles) => {
      const existingIds = new Set(prevRoles.map(r => r.roleId));
      const filteredNew = newRoles.filter(r => !existingIds.has(r.roleId));
      return [...prevRoles, ...filteredNew];
    });
  };

  const addOrUpdateRoles = (newRoles: Role[]) => {
    setRoles((prevRoles) => {
      const rolesMap = new Map(prevRoles.map(r => [r.roleId, r]));
  
      newRoles.forEach(newRole => {
        rolesMap.set(newRole.roleId, newRole); 
      });
  
      return Array.from(rolesMap.values());
    });
  };

  const updateRole = (updated: Role) => {
    setRoles((prev) =>
      prev.map((r) => (r.roleId === updated.roleId ? updated : r))
    );


    setProjects((prev) =>
      prev.map((p) => ({
        ...p,
        roles: (p.roles ?? []).map((r) =>
          r.roleId === updated.roleId ? updated : r
        ),
      }))
    );

  };

  const deleteRole = (id: number) => {
    setRoles((prev) => prev.filter((r) => r.roleId !== id));
    setProjects((prev) =>
      prev.map((p) => ({
        ...p,
        roles: p.roles.filter((r) => r.roleId !== id),
      }))
    );
  };

  const addEmployee = (employee: Employee) => {
    setEmployees((prev) => [...prev, employee]);
  };

  const updateEmployee = (updated: Employee) => {
    setEmployees((prev) =>
      prev.map((e) => (e.employeeId === updated.employeeId ? updated : e))
    );
  };

  const deleteEmployee = (id: number) => {
    setEmployees((prev) => prev.filter((e) => e.employeeId !== id));
  };

  const addTicket = (ticket: Ticket) => {
    setTickets((prev) => [...prev, ticket]);
  };

  const updateTicket = (updated: Ticket) => {
    setTickets((prev) =>
      prev.map((t) => (t.ticketId === updated.ticketId ? updated : t))
    );
  };

  const deleteTicket = (id: number) => {
    setTickets((prev) => prev.filter((t) => t.ticketId !== id));
  };

  return (
    <DataContext.Provider
      value={{
        projects,
        roles,
        employees,
        tickets,
        setProjects,
        setRoles,
        setEmployees,
        setTickets,
        addProject,
        updateProject,
        deleteProject,
        addRole,
        addRolesIfNotExist,
        addOrUpdateRoles,
        updateRole,
        deleteRole,
        addEmployee,
        updateEmployee,
        deleteEmployee,
        addTicket,
        updateTicket,
        deleteTicket,
      }}
    >
      {children}
    </DataContext.Provider>
  );
};

export const useDataContext = () => {
  const context = useContext(DataContext);
  if (!context) throw new Error("useDataContext must be used inside DataProvider");
  return context;
};

import React, { useEffect, useState } from 'react';
import { getEmployeeRolesById } from '../../../Services/EmployeesService';
import '../../../Styles/EmployeeRolesTable.css';
import { Language } from '../../../Types/LanguageType';
import { Skill } from '../../../Types/SkillType';
import { useDataContext } from '../../../Context/DataContext';


interface EmployeeRolesTableProps {
    employeeId: number;
    onOpenProject: (projectId: number) => void;
    onOpenRole: (roleId: number) => void;
  }
  
  const EmployeeRolesTable: React.FC<EmployeeRolesTableProps> = ({ employeeId, onOpenProject, onOpenRole }) => {
    const { roles } = useDataContext();
    const employeeRoles = roles.filter((r) => r.employeeId === employeeId);

  
    return (
      <div className="employee-roles-table">
        <h3>Employee Roles</h3>
        <table>
          <thead>
            <tr>
              <th>Role Name</th>
              <th>Project ID</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {employeeRoles.map((role) => (
              <tr key={role.roleId}>
                <td>{role.roleName}</td>
                <td>{role.projectId}</td>
                <td>
                  <button onClick={() => onOpenProject(role.projectId)} className="view-project-button">
                    View Project
                  </button>
                  <button onClick={() => onOpenRole(role.roleId)} className="view-role-button">
                    View Role
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    );
  };
  
  export default EmployeeRolesTable;
  
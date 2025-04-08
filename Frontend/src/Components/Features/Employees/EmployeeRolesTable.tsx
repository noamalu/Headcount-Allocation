import React, { useEffect, useState } from 'react';
import { getEmployeeRolesById } from '../../../Services/EmployeesService';
import '../../../Styles/EmployeeRolesTable.css';
import { Language } from '../../../Types/LanguageType';
import { Skill } from '../../../Types/SkillType';

interface Role {
  roleId: number;
  roleName: string;
  projectId: number;
  employeeId: number;
  description: string;
  timeZone: number;
  foreignLanguages: Language[];
  skills: Skill[];
  yearsExperience: number;
  jobPercentage: number;
}

interface EmployeeRolesTableProps {
    employeeId: number;
    onOpenProject: (projectId: number) => void;
    onOpenRole: (roleId: number) => void;
  }
  
  const EmployeeRolesTable: React.FC<EmployeeRolesTableProps> = ({ employeeId, onOpenProject, onOpenRole }) => {
    const [roles, setRoles] = useState<Role[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
  
    useEffect(() => {
      const fetchRoles = async () => {
        try {
          const response = await getEmployeeRolesById(employeeId);
          setRoles(response);
          setLoading(false);
        } catch (err: any) {
          console.error('Error fetching employee roles:', err);
          setError('Failed to fetch roles');
          setLoading(false);
        }
      };
      fetchRoles();
    }, [employeeId]);
  
    if (loading) return <div>Loading employee roles...</div>;
    if (error) return <div>{error}</div>;
  
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
            {roles.map((role) => (
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
  
import React, { useState, useEffect } from 'react';
import { formatDate, Project } from '../../../Types/ProjectType';
import { Role } from '../../../Types/RoleType';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import EditProjectModal from './EditProjectModal';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import CreateRoleModal from '../Roles/CreateRoleModal';
import ProjectsService, { getProjectRoles } from '../../../Services/ProjectsService';
import { useAuth } from '../../../Context/AuthContext'
import { useDataContext } from '../../../Context/DataContext';


interface ProjectDetailsModalProps {
  project: Project; 
  onClose: () => void; 
  // onProjectUpdated: (project: Project) => void;
  // onProjectDeleted: (projectId: number) => void;
}

// const ProjectDetailsModal: React.FC<ProjectDetailsModalProps> = ({ project, onClose, onProjectUpdated, onProjectDeleted }) => {
const ProjectDetailsModal: React.FC<ProjectDetailsModalProps> = ({ project, onClose }) => {  
  const {isAdmin} = useAuth();
  const { roles, addRole, addRolesIfNotExist, updateRole } = useDataContext();
  const { updateProject, deleteProject } = useDataContext();
  const projectRoles = roles.filter((r) => r.projectId === project.projectId);

  // const [project, setproject] = useState<Project>(project);
  // const [roles, setRoles] = useState<Role[]>([]);
  const [selectedRole, setSelectedRole] = useState<Role | null>(null);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [isCreateRoleModalOpen, setIsCreateRoleModalOpen] = useState(false);
  const [showConfirmDelete, setShowConfirmDelete] = useState(false);
  const [apiError, setApiError] = useState<string | null>(null);
 
  // useEffect(() => {
  //   const fetchRoles = async () => {
  //     try {
  //       console.log('Fetching roles for project:', project.projectId);
  //       const roles = await getProjectRoles(project.projectId); 
  //       setRoles(roles || {}); 
  //       setproject((prev) => ({
  //         ...prev,
  //         roles: roles || []
  //       }));
  //     } catch (error) {
  //       console.error('Error fetching project roles:', error);
  //       setApiError('Failed to fetch project roles');
  //     }
  //   };
  
  //   if (project) {
  //     fetchRoles(); 
  //   }
  // }, [project]);

  useEffect(() => {
    const fetchRoles = async () => {
      try {
        const fetchedRoles = await getProjectRoles(project.projectId);
        addRolesIfNotExist(fetchedRoles);
      } catch (error) {
        console.error('Failed to fetch roles for project:', error);
        setApiError('Failed to load roles');
      }
    };
    fetchRoles();
  }, [project.projectId]);

  useEffect(() => {
    if (apiError) {
      alert(apiError);
    }
  }, [apiError]);

  const handleEditSave = (updatedProject: Project) => {
    // setproject(updatedProject);
    // onProjectUpdated(updatedProject);
    updateProject(updatedProject);
    console.log('Project updated:', updatedProject);
  };

  const handleDelete = async () => {
    try {
      await ProjectsService.deleteProject(project.projectId);
      // onProjectDeleted(project.projectId); 
      deleteProject(project.projectId);
      onClose(); 
    } catch (error) {
      alert("Failed to delete the project.");
      console.error(error);
    }
  };


  const handleRoleCreated = (newRole: Role) => {
    console.log('handleRoleCreated new role:', newRole.roleName);
    // setRoles((prevRoles) => {
    //   const updatedRoles = {
    //     ...prevRoles,
    //     [newRole.roleId]: newRole,
    //   };
    //   console.log("Updated roles:", updatedRoles);
    //   setproject(prev => ({ ...prev, roles: updatedRoles }));
    //   return updatedRoles;
    // });
    // const updatedRoles = {
    //   ...roles,
    //   [newRole.roleId]: newRole,
    // };
    // setRoles(updatedRoles);
    // setproject((prev) => ({ ...prev, roles: updatedRoles }));
    addRole(newRole);
  };

  const handleRoleEdited = (updatedRole: Role) => {
    // setRoles((prevRoles) => {
    //   const updatedRoles = {
    //     ...prevRoles,
    //     [updatedRole.roleId]: { 
    //       ...prevRoles[updatedRole.roleId], 
    //       ...updatedRole, 
    //     },
    //   };
    //   setproject(prev => ({ ...prev, roles: updatedRoles }));
    //   return updatedRoles;
    // });
    updateRole(updatedRole);
  };
      
  const handleOpenModal = (role: Role) => {
    console.log("Opening role modal for:", role.roleName, "Role data:", role); 
    setSelectedRole(role);
  };

  const handleCloseModal = () => {
    setSelectedRole(null);
  };


  const handleAssignEmployeeToRole = (roleId: number, employeeId: number) => {
    console.log("Assigning employee", employeeId, "to role", roleId);
    // setRoles((prevRoles) => {
    //   const updatedRoles = { ...prevRoles }; 
    //   if (updatedRoles[roleId]) {
    //     updatedRoles[roleId].employeeId = employeeId; 
    //   }
    //   setproject(prev => ({ ...prev, roles: updatedRoles }));
    //   return updatedRoles; 
    // });
    const roleToUpdate = roles.find((r) => r.roleId === roleId);
    if (roleToUpdate) {
      updateRole({ ...roleToUpdate, employeeId });
    }
  };


  if (selectedRole) {
    console.log("Selected role being passed to RoleDetailsModal:", selectedRole);
  }

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2>{project.projectName}</h2>
        <div className="modal-info">
          <div className="modal-info-row">
            <div className="deadline">
              <i className="fas fa-calendar-alt"></i> {formatDate(project.deadline)}
            </div>
            <div className="required-hours">
              <i className="fas fa-clock"></i> {project.requiredHours} hours
            </div>
          </div>
          <div className="description">{project.description}</div>
        </div>
        <table className="roles-table">
          <thead>
            <tr>
              <th>Role</th>
              <th>Employee ID</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {projectRoles.length > 0 ? (
              // Object.entries(roles).map(([roleId, role]) => {
              //   console.log("Rendering role:", role); // בדקי מה מוצג בטבלה
              //   return (
              //     <tr key={roleId}>
              //       <td>{role.roleName}</td>
              //       <td>{role.employeeId && role.employeeId !== -1 ? role.employeeId : "-"}</td>
              //       <td>
              //         <button className="action-button" onClick={() => handleOpenModal(role)}>
              //           🔗
              //         </button>
              //       </td>
              //     </tr>
              //   );
              // })
              projectRoles.map((role) => (
                <tr key={role.roleId}>
                  <td>{role.roleName}</td>
                  <td>{role.employeeId && role.employeeId !== -1 ? role.employeeId : "-"}</td>
                  <td>
                    <button className="action-button" onClick={() => handleOpenModal(role)}>
                      🔗
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={3}>No roles available</td>
              </tr>
            )}
          </tbody>
        </table>

        <div className="modal-actions">
        {isAdmin && (
          <button className="edit-button" onClick={() => { console.log('Opening edit modal:', !isEditModalOpen); setIsEditModalOpen(true); }}>
            <i className="fas fa-pen"></i> Edit
          </button> 
        )}
        {isAdmin && (
          <button className="addRole-button"onClick={() => { console.log('Opening add role:', !isCreateRoleModalOpen); setIsCreateRoleModalOpen(true); }}>
            <i className="fas fa-plus"></i> Add Role
          </button>
        )}
         {isAdmin && (
          <button className="delete-button" onClick={() => setShowConfirmDelete(true)}>
            <i className="fas fa-trash"></i> Delete
          </button>
        )}
        </div>

        {selectedRole && (
          <RoleDetailsModal 
          projectId={project.projectId} 
          roleId={selectedRole.roleId} 
          onClose={handleCloseModal} />
          // onAssignEmployeeToRole={handleAssignEmployeeToRole} />
        )}
        {isCreateRoleModalOpen && (
          <CreateRoleModal 
          projectId={project.projectId}
          onClose={() => setIsCreateRoleModalOpen(false)}
          // onRoleCreated={handleRoleCreated} 
          />
        )}
         {isEditModalOpen && (
          <EditProjectModal
          project={project}
          onClose={() => setIsEditModalOpen(false)} />
        )}
        {showConfirmDelete && (
          <div className="confirm-overlay">
            <div className="confirm-dialog">
              <p>Are you sure you want to delete this project?</p>
              <div className="confirm-buttons">
                <button className="confirm-button" onClick={handleDelete}>
                  <i className="fas fa-trash"></i> Delete
                </button>
                <button className="cancel-button" onClick={() => setShowConfirmDelete(false)}>
                <i className="fas fa-close"></i> Cancel
                </button>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};



export default ProjectDetailsModal;
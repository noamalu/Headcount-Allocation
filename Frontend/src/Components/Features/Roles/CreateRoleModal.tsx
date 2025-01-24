import React, { useState } from 'react';
import { Role } from '../../../Types/RoleType';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';

interface CreateRoleModalProps {
  onSave: (newRole: Role) => void;
  onClose: () => void;
}

const CreateRoleModal: React.FC<CreateRoleModalProps> = ({ onSave, onClose }) => {
  const [roleName, setRoleName] = useState('');
  const [description, setDescription] = useState('');
  const [timeZone, setTimeZone] = useState(0);
  const [yearsExperience, setYearsExperience] = useState(0);
  const [jobPercentage, setJobPercentage] = useState(0);

  const handleSave = () => {
    if (!roleName.trim() || !description.trim()) {
      alert('Please fill in all fields.');
      return;
    }
    const newRole: Role = {
      roleId: -1,
      roleName,
      projectId: 0, // Will be assigned later when linked to a project
      description,
      timeZone,
      yearsExperience,
      jobPercentage,
      foreignLanguages: [],
      skills: [],
    };
    onSave(newRole); // Pass the new role to the parent
    onClose(); // Close the modal
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h3>Add Role</h3>
        <input
          type="text"
          placeholder="Role Name"
          value={roleName}
          onChange={(e) => setRoleName(e.target.value)}
          className="input-field"
        />
        <textarea
          placeholder="Role Description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          className="textarea-field"
        ></textarea>
        <input
          type="number"
          placeholder="Time Zone"
          value={timeZone}
          onChange={(e) => setTimeZone(Number(e.target.value))}
          className="input-field"
        />
        <input
          type="number"
          placeholder="Years of Experience"
          value={yearsExperience}
          onChange={(e) => setYearsExperience(Number(e.target.value))}
          className="input-field"
        />
        <input
          type="number"
          placeholder="Job Percentage"
          value={jobPercentage}
          onChange={(e) => setJobPercentage(Number(e.target.value))}
          className="input-field"
        />
        <div className="modal-actions">
          <button onClick={handleSave} className="save-button">
            Save
          </button>
          <button onClick={onClose} className="cancel-button">
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
};

export default CreateRoleModal;

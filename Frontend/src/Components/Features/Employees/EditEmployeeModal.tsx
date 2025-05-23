import React, { useState, useEffect } from 'react';
import { Employee } from '../../../Types/EmployeeType';
import { Role } from '../../../Types/RoleType';
import { formateLanguage } from '../../../Types/LanguageType';
import { formateSkillToString } from '../../../Types/SkillType';
import EmployeesService from '../../../Services/EmployeesService';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import '../../../Styles/DetailsModal.css';
import { LanguageEnum, SkillEnum } from '../../../Types/EnumType';

interface EditEmployeeModalProps {
  employee: Employee;
  onClose: () => void;
  onSave: (updatedEmployee: Employee) => void;
}

const EditEmployeeModal: React.FC<EditEmployeeModalProps> = ({ employee, onClose, onSave }) => {
  const [editedEmployee, setEditedEmployee] = useState<Employee>({ ...employee });
  const [uiError, setUiError] = useState<string | null>(null);
  const [apiError, setApiError] = useState<string | null>(null);
  const [languages, setLanguages] = useState(
    employee.foreignLanguages.map(l => ({ language: formateLanguage(l.languageTypeId), languageTypeId: l.languageTypeId, level: l.level }))
  );
  const [skills, setSkills] = useState(
    employee.skills.map(s => ({ skill: formateSkillToString(s.skillTypeId), skillTypeId: s.skillTypeId, level: s.level }))
  );
  const [selectedLanguage, setSelectedLanguage] = useState('');
  const [selectedSkill, setSelectedSkill] = useState('');
  const [languageError, setLanguageError] = useState('');
  const [skillError, setSkillError] = useState('');


  useEffect(() => {
    if (apiError) {
      alert(apiError);
    }
  }, [apiError]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
      const { name, value } = e.target;
      if (name == 'jobPercentage') {
        const jobPercentageNewValue = Number(value) / 100;
        setEditedEmployee((prev) => ({ ...prev, [name]: jobPercentageNewValue }));
      } else {
        setEditedEmployee((prev) => ({ ...prev, [name]: value }));
      }  
    };
  

  const handleSave = async () => {
    try {
      await EmployeesService.editEmployee(editedEmployee);
      onSave(editedEmployee);
      onClose();
    } catch (error) {
      console.error('Error updating employee:', error);
      setApiError('Failed to update employee.');
    }
  };
  
  const handleLanguageLevelChange = (index: number, level: number) => {
    const updated = [...languages];
    updated[index].level = level;
    setLanguages(updated);
  };

  const handleDeleteLanguage = (index: number) => {
    const updated = [...languages];
    updated.splice(index, 1);
    setLanguages(updated);
  };

  const handleAddLanguage = () => {
    if (!selectedLanguage) return;
    if (languages.some((l) => l.language === selectedLanguage)) {
      setLanguageError('Language already exists');
      return;
    }
    setLanguages([...languages, { language: selectedLanguage, languageTypeId: selectedLanguage as any, level: 1 }]);
    setSelectedLanguage('');
  };

  const handleSkillLevelChange = (index: number, level: number) => {
    const updated = [...skills];
    updated[index].level = level;
    setSkills(updated);
  };

  const handleDeleteSkill = (index: number) => {
    const updated = [...skills];
    updated.splice(index, 1);
    setSkills(updated);
  };

  const handleAddSkill = () => {
    if (!selectedSkill) return;
    if (skills.some((s) => s.skill === selectedSkill)) {
      setSkillError('Skill already exists');
      return;
    }
    setSkills([...skills, { skill: selectedSkill, skillTypeId: selectedSkill as any, level: 1 }]);
    setSelectedSkill('');
  };


  return (
    <div className="modal-overlay details-modal">
        <div className="modal-content details-modal">
            <button className="close-button" onClick={onClose}>✖</button>
            <span className="employee-avatar">👤</span>
            <h2 className="employee-name">{employee.employeeName}</h2>

            {uiError && (
                <div className="ui-error" style={{ whiteSpace: 'pre-line' }}>{uiError}</div>
            )}
            
            <div className="details-section">
                <div className="edit-banner">
                    <i className="fas fa-phone"></i>
                    <span>
                        <strong>Phone:</strong>
                        <input
                            type="tel"
                            name="phoneNumber"
                            value={editedEmployee.phoneNumber}
                            onChange={handleInputChange}
                            className="input-field input-medium"
                        />
                    </span>
                </div>
                <div className="edit-banner">
                    <i className="fa-solid fa-envelope"></i>
                    <span>
                        <strong>Email:</strong>
                        <input
                            type="email"
                            name="email"
                            value={editedEmployee.email}
                            onChange={handleInputChange}
                            className="input-field input-large"
                        />
                    </span>
                </div>
                <div className="edit-banner">
                    <i className="fas fa-briefcase"></i>
                    <span>
                        <strong>Years of Experience:</strong>
                        <input
                            type="number"
                            name="yearsExperience"
                            value={editedEmployee.yearsExperience}
                            onChange={handleInputChange}
                            className="input-field input-small"
                            min={0}
                        />
                    </span>
                </div>
                <div className="edit-banner">
                    <i className="fas fa-percentage"></i>
                    <span>
                        <strong>Job Percentage:</strong>
                        <input
                            type="number"
                            name="jobPercentage"
                            value={(editedEmployee.jobPercentage * 100).toFixed(0)}
                            onChange={handleInputChange}
                            className="input-field input-small"
                            min={0}
                            max={100}
                        />
                    </span>
                </div>
                <div className="edit-banner">
                    <i className="fas fa-globe"></i>
                    <span>
                        <strong>Time Zone:</strong>
                        <select
                            id="timeZone"
                            value={editedEmployee.timeZone} 
                            onChange={handleInputChange} 
                            className="dropdown"
                        >
                            <option value={1}>Morning</option>
                            <option value={2}>Noon</option>
                            <option value={3}>Evening</option>
                            <option value={4}>Flexible</option>
                        </select>
                    </span>
                </div>
            </div>

    <div className="details-section">
    <div className="edit-banner wide">
        <div className="skills-section">
        <i className="fas fa-language"></i>
        <strong>  Foreign Languages:</strong> 
          <div className="modal-info-row">
            <label>Add Language: </label>
            <select
              id="languageSelect"
              value={selectedLanguage}
              onChange={(e) => {
                setSelectedLanguage(e.target.value);
                setLanguageError('');
              }}
              className="dropdown"
            >
              <option value="" disabled>Select a language</option>
              {Object.values(LanguageEnum).map((language) => (
                <option key={language} value={language}>
                  {language}
                </option>
              ))}
            </select>
            <button className="add-button" onClick={handleAddLanguage}>
              <i className="fas fa-plus"></i>
            </button>
          </div>

          {languageError && <div className="list-error">{languageError}</div>}

          <table className="skills-input-table">
            <thead>
              <tr>
                <th>Language</th>
                <th>Level</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {languages.map((lang, index) => (
                <tr key={index}>
                  <td>{lang.language}</td>
                  <td>
                    <select
                      value={lang.level}
                      onChange={(e) => handleLanguageLevelChange(index, Number(e.target.value))}
                    >
                      {[1, 2, 3].map((level) => (
                        <option key={level} value={level}>{level}</option>
                      ))}
                    </select>
                  </td>
                  <td>
                    <button className="delete-record-button" onClick={() => handleDeleteLanguage(index)}>
                      <i className="fas fa-trash"></i>
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        </div>

        {/* Skills Section */}
        <div className="edit-banner wide">
            <div className="skills-section">
                <i className="fas fa-tools"></i>
                <strong>     Skills:</strong>
                <div className="modal-info-row">
                    <label>Add Skill: </label>
                    <select
                    id="skillSelect"
                    value={selectedSkill}
                    onChange={(e) => {
                        setSelectedSkill(e.target.value);
                        setSkillError('');
                    }}
                    className="dropdown"
                    >
                    <option value="" disabled>Select a skill</option>
                    {Object.values(SkillEnum).map((skill) => (
                        <option key={skill} value={skill}>
                        {skill}
                        </option>
                    ))}
                    </select>
                    <button className="add-button" onClick={handleAddSkill}>
                    <div className='icon-button'><i className="fas fa-plus"></i></div>
                    </button>
                </div>

                {skillError && <div className="list-error">{skillError}</div>}

                <table className="skills-input-table">
                    <thead>
                    <tr>
                        <th>Skill</th>
                        <th>Level</th>
                        <th></th>
                    </tr>
                    </thead>
                    <tbody>
                    {skills.map((sk, index) => (
                        <tr key={index}>
                        <td>{sk.skill}</td>
                        <td>
                            <select
                            value={sk.level}
                            onChange={(e) => handleSkillLevelChange(index, Number(e.target.value))}
                            >
                            {[1, 2, 3].map((level) => (
                                <option key={level} value={level}>{level}</option>
                            ))}
                            </select>
                        </td>
                        <td>
                            <button className="delete-record-button" onClick={() => handleDeleteSkill(index)}>
                            <i className="fas fa-trash"></i>
                            </button>
                        </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
                </div>
            </div>
            

              {/* Roles Section */}
              <div className="edit-banner wide">
              <div className="skills-section">
                <i className="fa-solid fa-chalkboard-user"></i>
                <strong>  Roles:</strong>
                <table className="roles-table">
            <thead>
              <tr>
                <th>Role Name</th>
                <th>Project ID</th>
                <th>Role</th>
              </tr>
            </thead>
            <tbody>
              {editedEmployee.roles.map((role, index) => (
                <tr key={index}>
                  <td>{role.roleName}</td>
                  <td>{role.projectId}</td>
                  <td>
                    <button className="action-button" disabled>
                      🔗
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        </div>
        </div>

        <div className="modal-actions">
          <button className="save-button" onClick={handleSave}>
            <i className="fas fa-save"></i> Save Changes
          </button>
        </div>
      </div>
    </div>
  );
};

export default EditEmployeeModal;

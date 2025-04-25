import React, { useState, useEffect } from 'react';
import { Role } from '../../../Types/RoleType';
import { formateLanguage } from '../../../Types/LanguageType';
import { formateSkillToString } from '../../../Types/SkillType';
import ProjectsService from '../../../Services/ProjectsService';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import '../../../Styles/DetailsModal.css';
import { LanguageEnum, SkillEnum } from '../../../Types/EnumType';

interface EditRoleModalProps {
  projectId: number;
  role: Role;
  employeeName: string;
  onClose: () => void;
  onSave: (updatedRole: Role) => void;
}

const EditRoleModal: React.FC<EditRoleModalProps> = ({ projectId,  role, employeeName, onClose, onSave }) => {
  const [editedRole, setEditedRole] = useState<Role>({ ...role });
  const [uiError, setUiError] = useState<string | null>(null);
  const [apiError, setApiError] = useState<string | null>(null);
  const [languages, setLanguages] = useState(
    role.foreignLanguages.map(l => ({ language: formateLanguage(l.languageTypeId), languageTypeId: l.languageTypeId, level: l.level }))
  );
  const [skills, setSkills] = useState(
    role.skills.map(s => ({ skill: formateSkillToString(s.skillTypeId), skillTypeId: s.skillTypeId, level: s.level, priority: s.priority }))
  );
 const [selectedLanguage, setSelectedLanguage] = useState<LanguageEnum | "">("");
  const [selectedSkill, setSelectedSkill] = useState<SkillEnum | "">("");
  const [draggedSkillIndex, setDraggedSkillIndex] = useState<number | null>(null);
  const [languageError, setLanguageError] = useState('');
  const [skillError, setSkillError] = useState('');


  useEffect(() => {
    if (apiError) {
      alert(apiError);
    }
  }, [apiError]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    if (name == 'jobPercentage') {
      const jobPercentageNewValue = Number(value) / 100;
      setEditedRole((prev) => ({ ...prev, [name]: jobPercentageNewValue }));
    } else {
      setEditedRole((prev) => ({ ...prev, [name]: value }));
    }  
  };

  

  // Languages: 
  
  const handleAddLanguage = () => {
    if (!selectedLanguage) return;
    if (languages.some((l) => l.language === selectedLanguage)) {
      setLanguageError('Language already exists');
      return;
    }
    setLanguages([...languages, { language: selectedLanguage, languageTypeId: selectedLanguage as any, level: 1 }]);
    setSelectedLanguage('');
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



  // Skills:

  const handleAddSkill = () => {
    if (!selectedSkill || skills.some((s) => s.skill === selectedSkill)) {
      setSkillError('Skill already added or not selected');
      return;
    }
    setSkills([...skills, { skill: selectedSkill, skillTypeId: selectedSkill as any, level: 1, priority: skills.length + 1 }]);
    setSelectedSkill('');
    setSkillError('');
  };

  const handleSkillLevelChange = (index: number, level: number) => {
    const updatedSkills = [...skills];
    updatedSkills[index].level = level;
    setSkills(updatedSkills);
    setSkillError("");
  };

  const handleDeleteSkill = (index: number) => {
    const updatedSkills = skills.filter((_, i) => i !== index); // מסנן את השורה
    setSkills(updatedSkills);
    setSkillError("");
  }; 

  const handleDragStart = (index: number) => {
      setDraggedSkillIndex(index);
      setSkillError("");
  };

  const handleDrop = (index: number) => {
      if (draggedSkillIndex === null || draggedSkillIndex === index) return;

      const reorderedSkills = [...skills];
      const [draggedSkill] = reorderedSkills.splice(draggedSkillIndex, 1);
      reorderedSkills.splice(index, 0, draggedSkill);

      setSkills(reorderedSkills);
      setDraggedSkillIndex(null); 
  };

  
  // Save:

  const handleSave = async () => {
    let errorMessage = "";
    if (!editedRole.roleName.trim()) {
      errorMessage += "• Role name is required.\n";
    }
    if (!editedRole.description.trim()) {
      errorMessage += "• Description is required.\n";
    }
    if (editedRole.yearsExperience < 0) {
      errorMessage += "• Years of experience cannot be negative.\n";
    }
    if (editedRole.jobPercentage <= 0) {
      errorMessage += "• Job percentage must be greater than 0%.\n";
    }
    if (skills.length === 0) {
      errorMessage += "• Please add at least one skill.\n";
    }
    if (languages.length === 0) {
      errorMessage += "• Please add at least one language.\n";
    }
    if (errorMessage) {
      setUiError(errorMessage.trim());
      return;
    }
    setUiError(null);

    const updatedRole: Role = {
      ...editedRole,
      foreignLanguages: languages.map((l, index) => ({
        languageId: index,
        languageTypeId: l.languageTypeId,
        level: l.level
      })),
      skills: skills.map((s, index) => ({
        skillId: index,
        skillTypeId: s.skillTypeId,
        level: s.level,
        priority: index + 1
      }))
    };
    

    try {
      await ProjectsService.editRole(editedRole, projectId);
      onSave(editedRole);
      onClose();
    } catch (error) {
      console.error('Error updating role:', error);
      setApiError('Failed to update role.');
    }
  };


  
  return (
    <div className="modal-overlay details-modal">
        <div className="modal-content details-modal">
          <button className="close-button" onClick={onClose}>✖</button>
      
          <input
                type="text"
                id="projectName"
                name="projectName"
                value={editedRole.roleName}
                onChange={handleInputChange}
                className="input-as-h2-field"
        />
      
          <div className="employee-info">
            <span className="employee-avatar">👤</span>
            <p className="employee-name">
              {employeeName != "" ? employeeName : "No employee assigned"}
            </p>
          </div>

          {uiError && (
              <div className="ui-error" style={{ whiteSpace: 'pre-line' }}>{uiError}</div>
          )}
          
          <div className="details-section">
              
              <div className="edit-banner">
                  <i className="fas fa-briefcase"></i>
                  <span>
                      <strong>Years of Experience:</strong>
                      <input
                          type="number"
                          name="yearsExperience"
                          value={editedRole.yearsExperience}
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
                          value={(editedRole.jobPercentage * 100).toFixed(0)}
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
                          value={editedRole.timeZone} 
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

              <div className="edit-banner wide">
                <i className="fas fa-align-left"></i>
                <div className="field-container">
                <label><strong>Description:</strong></label>
                  <textarea
                      id="description"
                      name="description"
                      value={editedRole.description}
                      onChange={handleInputChange}
                      className="textarea-field"
                    />
                </div>
              </div>
          </div>

          <div className="details-section">
            
            {/* Languages Section */}
            <div className="edit-banner wide">
              <div className="skills-section">
              <i className="fas fa-language"></i>
              <strong>Foreign Languages:</strong> 
                <div className="modal-info-row">
                  <label>Add Language: </label>
                  <select
                    id="languageSelect"
                    value={selectedLanguage}
                    onChange={(e) => {
                      setSelectedLanguage(e.target.value as LanguageEnum);
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
                  <strong>Skills:</strong>
                  <div className="modal-info-row">
                      <label>Add Skill: </label>
                      <select
                        id="skillSelect"
                        value={selectedSkill}
                        onChange={(e) => {
                            setSelectedSkill(e.target.value as SkillEnum);
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
                          <th>Priority</th>
                          <th></th>
                      </tr>
                      </thead>
                      <tbody>
                      {skills.map((skill, index) => (
                          <tr
                              key={index}
                              draggable
                              onDragStart={() => handleDragStart(index)}
                              onDragOver={(e) => e.preventDefault()}
                              onDrop={() => handleDrop(index)}
                          >
                          <td>{skill.skill}</td>
                          <td>
                              <select
                              value={skill.level}
                              onChange={(e) => handleSkillLevelChange(index, Number(e.target.value))}
                              >
                              {[1, 2, 3].map((level) => (
                                  <option key={level} value={level}>{level}</option>
                              ))}
                              </select>
                          </td>
                          <td>{index + 1}</td> 
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

export default EditRoleModal;

import React, { useState, useEffect } from 'react';
import { Role } from '../../../Types/RoleType';
import { formateLanguage } from '../../../Types/LanguageType';
import { formateSkillToString } from '../../../Types/SkillType';
import ProjectsService from '../../../Services/ProjectsService';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import '../../../Styles/DetailsModal.css';
import { getLanguageStringByIndex, getSkillLabel, LanguageEnum, SkillEnum } from '../../../Types/EnumType';
import { useDataContext } from '../../../Context/DataContext';

interface EditRoleModalProps {
  projectId: number;
  role: Role;
  employeeName: string;
  onClose: () => void;
}

const EditRoleModal: React.FC<EditRoleModalProps> = ({ projectId,  role, employeeName, onClose }) => {
  const [editedRole, setEditedRole] = useState<Role>({ ...role });
  const [uiError, setUiError] = useState<string | null>(null);
  const [apiError, setApiError] = useState<string | null>(null);
   const [languages, setLanguages] = useState(
      role.foreignLanguages.map(l => ({
        language: getLanguageStringByIndex(l.languageTypeId) as LanguageEnum,
        languageTypeId: l.languageTypeId,
        level: l.level
      }))
    );
  const [skills, setSkills] = useState<{ skill: SkillEnum; level: number, priority: number}[]>(
    role.skills.map((s) => ({
        skill: s.skillTypeId,
        level: s.level,
        priority: s.priority
      }))
    );
  const [selectedLanguage, setSelectedLanguage] = useState<LanguageEnum | "">("");
  const [selectedSkill, setSelectedSkill] = useState('');
  const [draggedSkillIndex, setDraggedSkillIndex] = useState<number | null>(null);
  const [languageError, setLanguageError] = useState('');
  const [skillError, setSkillError] = useState('');
  const { updateRole } = useDataContext();


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
    setLanguages([...languages, {
      language: selectedLanguage,
      languageTypeId: Object.values(LanguageEnum).indexOf(selectedLanguage),
      level: 1
    }]);
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
      if (selectedSkill === "") 
        return;
      const parsedSkill = Number(selectedSkill) as SkillEnum;
      if (skills.some((s) => s.skill === parsedSkill)) {
        setSkillError('Skill already exists');
        return;
      }
      setSkills([...skills, { skill: parsedSkill, level: 1, priority: skills.length + 1 }]);
      setSelectedSkill('');
    };


  const handleSkillLevelChange = (index: number, level: number) => {
    const updated = [...skills];
    updated[index].level = level;
    setSkills(updated);
    setSkillError("");
  };

  const handleDeleteSkill = (index: number) => {
    const updated = skills.filter((_, i) => i !== index); // מסנן את השורה
    setSkills(updated);
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
    console.log('Languages about to send:', languages);
    let errorMessage = "";
    if (!editedRole.roleName.trim()) {
      errorMessage += "• Role name is required.\n";
    }
    if(!editedRole.startDate) {
      errorMessage += "• Please select a start date.\n";
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
        skillTypeId: s.skill,
        level: s.level,
        priority: index + 1
      }))
    };
    console.log('foreignLanguages about to send:', updatedRole.foreignLanguages);
    try {
      await ProjectsService.editRole(updatedRole, projectId);
      updateRole(updatedRole);
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
                  <i className="fas fa-calendar-alt"></i>
                  <span>
                      <strong>Start Date:</strong>
                      <input
                          type="date"
                          name="startDate"
                          value={editedRole.startDate.toString().split('T')[0]}
                          onChange={handleInputChange}
                          className="input-field input-medium"
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
                          name="timeZone"
                          value={editedRole.timeZone} 
                          onChange={handleInputChange} 
                          className="dropdown"
                      >
                          <option value={0}>Morning</option>
                          <option value={1}>Noon</option>
                          <option value={2}>Evening</option>
                          <option value={3}>Flexible</option>
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
                            setSelectedSkill(e.target.value);
                            setSkillError('');
                        }}
                        className="dropdown"
                      >
                          <option value="" disabled>Select a skill</option>
                          {Object.keys(SkillEnum)
                          .filter((key) => isNaN(Number(key)))
                          .map((key) => (
                            <option key={key} value={SkillEnum[key as keyof typeof SkillEnum]}>
                              {getSkillLabel(SkillEnum[key as keyof typeof SkillEnum])}
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
                      {skills.map((sk, index) => (
                          <tr
                              key={index}
                              draggable
                              onDragStart={() => handleDragStart(index)}
                              onDragOver={(e) => e.preventDefault()}
                              onDrop={() => handleDrop(index)}
                          >
                          <td>{getSkillLabel(sk.skill)}</td>
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

import React, { useEffect, useState } from 'react';
import { Role } from '../../../Types/RoleType';
import '../../../Styles/Modal.css';
import '../../../Styles/DetailsModal.css';
import '../../../Styles/Shared.css';
import { SkillEnum, LanguageEnum, skillEnumToId, getSkillLabel } from '../../../Types/EnumType';
import ProjectsService from '../../../Services/ProjectsService';
import { Language } from '../../../Types/LanguageType';
import { Skill } from '../../../Types/SkillType';
import { useDataContext } from '../../../Context/DataContext';


interface CreateRoleModalProps {
  projectId: number;
  // onRoleCreated: (newRole: Role) => void;
  onClose: () => void;
}

const CreateRoleModal: React.FC<CreateRoleModalProps> = ({ projectId, onClose }) => {
  const [roleName, setRoleName] = useState('');
  const [startDate, setStartDate] = useState('');
  const [description, setDescription] = useState('');
  const [timeZone, setTimeZone] = useState(0);
  const [yearsExperience, setYearsExperience] = useState(0);
  const [jobPercentage, setJobPercentage] = useState(0);
  const [skills, setSkills] = useState<{ skill: SkillEnum; level: number }[]>([]);
  const [selectedSkill, setSelectedSkill] = useState<SkillEnum | "">("");
  const [draggedSkillIndex, setDraggedSkillIndex] = useState<number | null>(null);
  const [languages, setLanguages] = useState<{ language: LanguageEnum; level: number }[]>([]);
  const [selectedLanguage, setSelectedLanguage] = useState<LanguageEnum | "">("");
  const [uiError, setUiError] = useState<string | null>(null);
  const [apiError, setApiError] = useState<string | null>(null);
  const [skillError, setSkillError] = useState<string>("");
  const [languageError, setLanguageError] = useState<string>("");
  const { addRole } = useDataContext();

  useEffect(() => {
    if (apiError) {
      alert(apiError);
    }
  }, [apiError]);

  const addUiError = (message: string) => {
    setUiError((prev) => (prev ? prev + "\n• " + message : "• " + message));
  };


  // Skills Edit:

  const handleAddSkill = () => {
    if (selectedSkill === "") {
      setSkillError("Please select a skill.");
      return;
    }
    const parsedSkill = Number(selectedSkill) as SkillEnum;
    if (skills.some((s) => s.skill === parsedSkill)) {
      setSkillError('Skill already exists');
      return;
    }
    setSkills([...skills, { skill: parsedSkill, level: 1 }]);
    setSelectedSkill('');
    setSkillError("");
  };

  const handleLevelChange = (index: number, level: number) => {
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

  
  // Languages Edit:
  
  const handleAddLanguage = () => {
    if (!selectedLanguage || languages.some((l) => l.language === selectedLanguage)) {
      setLanguageError("Language already added or no language was selected.");
      return;
    }
    setLanguages([...languages, { language: selectedLanguage, level: 1 }]);
    setSelectedLanguage(""); 
  };

  const handleLanguageLevelChange = (index: number, level: number) => {
    const updatedLanguages = [...languages];
    updatedLanguages[index].level = level;
    setLanguages(updatedLanguages);
    setLanguageError("");
  };

  const handleDeleteLanguage = (index: number) => {
    const updatedLanguages = languages.filter((_, i) => i !== index);
    setLanguages(updatedLanguages);
    setLanguageError("");
  };

  const handleSubmit = async () => {
    let errorMessage = "";
    if (!roleName.trim()) {
      errorMessage += "• Role name is required.\n";
    }
    if(!startDate) {
      errorMessage += "• Please select a start date.\n";
    }
    if (yearsExperience < 0) {
      errorMessage += "• Years of experience cannot be negative.\n";
    }
    if (jobPercentage === 0) {
      errorMessage += "• Job percentage must be greater than 0%.\n";
    }
    if (!description.trim()) {
      errorMessage += "• Role description is required.\n";
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

  
    // create "roleSkill" array
    const roleSkills: Skill[] = skills.map((skill, index) => ({
      skillId: index, 
      skillTypeId: skill.skill,
      level: skill.level,
      priority: skills.length - index, 
    }));

    console.log("in CreateRoleModal, skillTypeId: " + roleSkills[0].skillTypeId);
 
    // create "language" array
    const roleLanguages: Language[] = languages.map((lang, index) => ({
      languageId: index, 
      languageTypeId: Object.values(LanguageEnum).indexOf(lang.language), 
      level: lang.level,
    }));
  
    const newRole: Role = {
      roleName,
      roleId: -1, 
      projectId: projectId,
      employeeId: -1,
      description,
      timeZone,
      foreignLanguages: roleLanguages,
      skills: roleSkills, 
      yearsExperience,
      jobPercentage,
      startDate,
    };
  
    try {
      const addedRoles = await ProjectsService.addRolesToProject(projectId, [newRole]);
      console.log("Type of response:", typeof addedRoles);
      console.log("Response keys:", Object.keys(addedRoles));
      newRole.roleId = addedRoles[0].roleId;
      console.log('Role created successfully:', newRole);
      // onRoleCreated(newRole); 
      addRole(newRole);
      setApiError(null);
      onClose();
    } catch (error) {
      console.error('Error creating role:', error);
      setApiError('An error occurred while creating the role');
    }
  };

  return (
    <div className="modal-overlay">
        <div className="modal-content">
          <button className="close-button" onClick={onClose}>✖</button>
          <h2>Create New Role</h2>

          {uiError && (
          <div className="ui-error" style={{ whiteSpace: 'pre-line' }}>
            {uiError}
          </div>
        )}

          <div className="modal-info">
            <div>
              <label>Role Name: </label>
              <input
                type="text"
                value={roleName}
                onChange={(e) => setRoleName(e.target.value)}
                placeholder="Enter role name"
                className="input-field"
              />
            </div>
            <div>
              <label>Start date: </label>
              <input
                type="date"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
                className="input-field"
              />
            </div>
            <div>
              <label>Years of experience: </label>
              <input
                type="number"
                value={yearsExperience}
                onChange={(e) => setYearsExperience(Number(e.target.value))}
                className="number-field"
              />
            </div>
            <div className="slider-container">
              <label>Job Percentage:</label>
              <input
                id="jobPercentage"
                type="range"
                min={0}
                max={100}
                value={jobPercentage * 100} 
                onChange={(e) => setJobPercentage(Number(e.target.value) / 100)} 
                className="slider"
              />
              <span className="slider-value">{Math.round(jobPercentage * 100)}%</span> 
            </div>
            <div>
              <label>Time Zone: </label>
              <select
                id="timeZone"
                value={timeZone} 
                onChange={(e) => setTimeZone(Number(e.target.value))} 
                className="dropdown"
              >
                <option value="" disabled>
                  Select a time zone
                </option>
                <option value={0}>Morning</option>
                <option value={1}>Noon</option>
                <option value={2}>Evening</option>
                <option value={3}>Flexible</option>
              </select>
            </div>
          </div>
          <div className="modal-info">
            <label>Description:</label>
            <textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              placeholder="Enter role description"
              className="textarea-field"
            ></textarea>
          </div>
          
          {/* Try skills */}
          <div className="skills-section">
            <div className="modal-info-row">
              <label>Add Skill: </label>
              <select
                  id="skillSelect"
                  value={selectedSkill}
                  onChange={(e) => {
                    setSelectedSkill(Number(e.target.value) as SkillEnum);
                    setSkillError("");}
                  }
                  className="dropdown"
              >
                  <option value="" disabled>Select a skill</option>
                  {Object.keys(SkillEnum)
                  .filter((key) => isNaN(Number(key)))
                  .map((key) => {
                    const enumKey = key as keyof typeof SkillEnum;
                    return (
                      <option key={key} value={SkillEnum[enumKey]}>
                        {getSkillLabel(SkillEnum[enumKey])}
                      </option>
                    );
                  })}
              </select>
              <button className="add-button" onClick={handleAddSkill}>
                <i className="fas fa-plus"></i>
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
                          <td>{getSkillLabel(skill.skill)}</td>
                          <td>
                              <select
                                  value={skill.level}
                                  onChange={(e) => handleLevelChange(index, Number(e.target.value))}
                              >
                                  {[1, 2, 3].map((level) => (
                                      <option key={level} value={level}>
                                          {level}
                                      </option>
                                  ))}
                              </select>
                          </td>
                          <td>{index + 1}</td> 
                          <td>
                            <button
                              className="delete-record-button"
                              onClick={() => handleDeleteSkill(index)}
                            >
                              <i className="fas fa-trash"></i>
                            </button>
                          </td>
                        </tr>
                    ))}
                </tbody>
            </table>
          </div>

          <div className="skills-section">
            <div className="modal-info-row">
              <label>Add Language: </label>
              <select
                id="languageSelect"
                value={selectedLanguage}
                onChange={(e) => {
                  setSelectedLanguage(e.target.value as LanguageEnum)
                  setLanguageError("");}
                }
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

            <table className="languages-input-table">
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
                          <option key={level} value={level}>
                            {level}
                          </option>
                        ))}
                      </select>
                    </td>
                    <td>
                      <button
                        className="delete-record-button"
                        onClick={() => handleDeleteLanguage(index)}
                      >
                        <i className="fas fa-trash"></i>
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

            {/* <tbody>
            {role.skills.map((skill) => (
                <tr key={skill.skillId}>
                  <td>{skill.skillName}</td>
                  <td>{skill.level}</td>
                  <td>{skill.priority}</td>
                  <td> {0}</td>
                </tr>
              ))}
            </tbody> */}

            {/* // roles.map((role, index) => (
            //   <tr key={index}>
            //     <td>{role.roleName}</td>
            //     <td>
            //       <button
            //         onClick={() =>
            //           setRoles(roles.filter((_, idx) => idx !== index))
            //         }
            //       >
            //         Remove
            //       </button>
            //     </td>
            //   </tr>
            // ))
          //   }
          //   </tbody>
          // </table> */}
          <div className="modal-actions">
            <button className="save-button" onClick={handleSubmit}>
              <i className="fas fa-save"></i> Save Role
            </button>
            {/* {isAddRoleModalOpen && (
            <AddRoleModal
              onSave={(newRole) => {
                setRoles([...roles, newRole]);
                setIsAddRoleModalOpen(false);
              }}
              onClose={() => setIsAddRoleModalOpen(false)}
            />
          )} */}
          </div>
        </div>
      </div>
    );
  };

export default CreateRoleModal;

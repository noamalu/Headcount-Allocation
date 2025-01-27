import React, { useState } from 'react';
import { Role } from '../../../Types/RoleType';
import '../../../Styles/Modal.css';
import '../../../Styles/RoleModal.css';
import '../../../Styles/Shared.css';
import { SkillEnum, LanguageEnum } from '../../../Types/EnumType';
import ProjectsService from '../../../Services/ProjectsService';
import { Language } from '../../../Types/LanguageType';
import { Skill } from '../../../Types/SkillType';

interface CreateRoleModalProps {
  projectId: number;
  onRoleCreated: (newRole: Role) => void;
  onClose: () => void;
}

const CreateRoleModal: React.FC<CreateRoleModalProps> = ({projectId, onClose , onRoleCreated }) => {
  const [roleName, setRoleName] = useState('');
  const [description, setDescription] = useState('');
  const [timeZone, setTimeZone] = useState(0);
  const [yearsExperience, setYearsExperience] = useState(0);
  const [jobPercentage, setJobPercentage] = useState(0);
  const [error, setError] = useState<string>(""); 
  const [skills, setSkills] = useState<{ skill: SkillEnum; level: number }[]>([]);
  const [selectedSkill, setSelectedSkill] = useState<SkillEnum | "">("");
  const [draggedSkillIndex, setDraggedSkillIndex] = useState<number | null>(null);
  const [languages, setLanguages] = useState<{ language: LanguageEnum; level: number }[]>([]);
  const [selectedLanguage, setSelectedLanguage] = useState<LanguageEnum | "">("");

  const handleAddSkill = () => {
      if (!selectedSkill || skills.some((s) => s.skill === selectedSkill)) {
          alert("Skill already added or not selected.");
          return;
      }
      setSkills([...skills, { skill: selectedSkill, level: 1 }]);
      setSelectedSkill(""); // איפוס הבחירה
  };

  const handleDragStart = (index: number) => {
      setDraggedSkillIndex(index);
  };

  const handleDrop = (index: number) => {
      if (draggedSkillIndex === null || draggedSkillIndex === index) return;

      const reorderedSkills = [...skills];
      const [draggedSkill] = reorderedSkills.splice(draggedSkillIndex, 1);
      reorderedSkills.splice(index, 0, draggedSkill);

      setSkills(reorderedSkills);
      setDraggedSkillIndex(null); // איפוס ה-dragging
  };

  const handleLevelChange = (index: number, level: number) => {
      const updatedSkills = [...skills];
      updatedSkills[index].level = level;
      setSkills(updatedSkills);
  };

  const handleDeleteSkill = (index: number) => {
    const updatedSkills = skills.filter((_, i) => i !== index); // מסנן את השורה
    setSkills(updatedSkills);
  }; 
  
  const handleAddLanguage = () => {
    if (!selectedLanguage || languages.some((l) => l.language === selectedLanguage)) {
      alert("Language already added or not selected.");
      return;
    }
    setLanguages([...languages, { language: selectedLanguage, level: 1 }]);
    setSelectedLanguage(""); 
  };

  const handleLanguageLevelChange = (index: number, level: number) => {
    const updatedLanguages = [...languages];
    updatedLanguages[index].level = level;
    setLanguages(updatedLanguages);
  };

  const handleDeleteLanguage = (index: number) => {
    const updatedLanguages = languages.filter((_, i) => i !== index);
    setLanguages(updatedLanguages);
  };

  const handleSubmit = async () => {
    if (!roleName.trim() || !description.trim()) {
      setError("All fields are required, and required hours must be greater than 0.");
      alert('Please fill in all fields.');
      return;
    }
  
    // יצירת מערך של RoleSkill מהנתונים
    const roleSkills: Skill[] = skills.map((skill, index) => ({
      skillId: index, // ערך זמני, יתעדכן בשרת
      SkillTypeId: -1, // יתקבל מהשרת לאחר יצירת ה-Role
      level: skill.level,
      priority: skills.length - index, // חישוב עדיפות
    }));
  
    // יצירת מערך של Language מהנתונים
    const roleLanguages: Language[] = languages.map((lang, index) => ({
      languageId: -1, // ערך זמני, יתעדכן בשרת
      languageTypeId: Object.values(LanguageEnum).indexOf(lang.language), // מיפוי ה-Enum ל-ID מתאים
      level: lang.level,
    }));
  
    const newRole: Role = {
      roleName,
      roleId: -1, // ערך זמני, יתעדכן בשרת
      projectId: projectId,
      employeeId: -1,
      description,
      timeZone,
      foreignLanguages: roleLanguages, // הוספת השפות כנתון
      skills: roleSkills, // הוספת הכישורים כנתון
      yearsExperience,
      jobPercentage,
    };
  
    try {
      // שליחת הנתונים לשרת
      const addedRoles = await ProjectsService.addRolesToProject(projectId, [newRole]);
      console.log("Type of response:", typeof addedRoles);
      console.log("Response keys:", Object.keys(addedRoles));
      newRole.roleId = addedRoles[0].roleId;
      console.log('Role created successfully:', newRole);
      onRoleCreated(newRole); // עדכון ברכיב האב
      onClose(); // סגירת המודל
    } catch (error) {
      console.error('Error creating role:', error);
      setError('An error occurred while creating the role.');
    }
  };

  return (
    <div className="modal-overlay">
        <div className="modal-content">
          <button className="close-button" onClick={onClose}>✖</button>
          <h2>Create New Role</h2>
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
                onChange={(e) => setJobPercentage(Number(e.target.value) / 100)} // מחלקים ב-100 כדי לשמור כחלקי 1
                className="slider"
              />
              <span className="slider-value">{Math.round(jobPercentage * 100)}%</span> {/* מציג את הערך */}
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
                <option value={1}>UTC</option>
                <option value={2}>GMT</option>
                <option value={3}>IST</option>
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
            <label>Add Skill: </label>
            <select
                id="skillSelect"
                value={selectedSkill}
                onChange={(e) => setSelectedSkill(e.target.value as SkillEnum)}
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
              <i className="fas fa-plus"></i>
            </button>
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
            <label>Add Language: </label>
            <select
              id="languageSelect"
              value={selectedLanguage}
              onChange={(e) => setSelectedLanguage(e.target.value as LanguageEnum)}
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

import React, { useState, useEffect } from 'react';
import { Employee } from '../../../Types/EmployeeType';
import EmployeesSpan from './EmployeesSpan';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import EmployeesService from '../../../Services/EmployeesService';
import { LanguageEnum, SkillEnum } from '../../../Types/EnumType';
import { Language } from '../../../Types/LanguageType';
import { Skill } from '../../../Types/SkillType';
// import AddRoleModal from '../Roles/NewRoleModal';


const CreateEmployeeModal: React.FC<{ 
  onClose: () => void;
   onEmployeeCreated: (employee: Employee) => void }>
    = ({
      onClose,
      onEmployeeCreated,
    }) => {
    const [employeeName, setEmployeeName] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');
    const [email, setEmail] = useState('');
    const [timeZone, setTimeZone] = useState(0);
    const [languages, setLanguages] = useState<{ language: LanguageEnum; level: number }[]>([]);
    const [skills, setSkills] = useState<{ skill: SkillEnum; level: number }[]>([]);
    const [yearsExperience, setYearsExperience] = useState(0);
    const [jobPercentage, setJobPercentage] = useState(0);
    const [selectedSkill, setSelectedSkill] = useState<SkillEnum | "">("");
    const [selectedLanguage, setSelectedLanguage] = useState<LanguageEnum | "">("");
    const [password, setPassword] = useState('');
    const [showPassword, setShowPassword] = useState(false);
    const [error, setError] = useState<string>(""); 

    const handleAddSkill = () => {
        if (!selectedSkill || skills.some((s) => s.skill === selectedSkill)) {
            alert("Skill already added or not selected.");
            return;
        }
        setSkills([...skills, { skill: selectedSkill, level: 1 }]);
        setSelectedSkill(""); // איפוס הבחירה
    };

    const handleSkillLevelChange = (index: number, level: number) => {
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
        if (!employeeName || !phoneNumber || !timeZone || yearsExperience < 0 ||  jobPercentage < 0 || !password) {
            setError("All fields are required, and required numeric ones nust be greater than 0.");
            return;
        }

        // Create skills array
        const employeeSkills: Skill[] = skills.map((skill, index) => ({
            skillId: index, 
            skillTypeId: Object.values(SkillEnum).indexOf(skill.skill),
            level: skill.level,
            priority: 0, 
        }));
      
        console.log("in CreateEmployeeModal, skillTypeId: " + employeeSkills[0].skillTypeId);
       
        // Create Languages array
        const employeeLanguages: Language[] = languages.map((lang, index) => ({
            languageId: index, 
            languageTypeId: Object.values(LanguageEnum).indexOf(lang.language), 
            level: lang.level,
        }));

        console.log("in CreateEmployeeModal, languageId: " + employeeLanguages[0].languageId);

      const newEmployee: Employee = {
        employeeId: -1, 
        employeeName,
        phoneNumber,
        email,
        timeZone,
        foreignLanguages: employeeLanguages,
        skills: employeeSkills,
        yearsExperience,
        jobPercentage,
        roles: [],
        password
      };
      try {
        const newEmployeeId = await EmployeesService.sendCreateEmployee(newEmployee); // wont be it - NOA
        newEmployee.employeeId = newEmployeeId;
        console.log('Employee created successfully:', newEmployee);
        onEmployeeCreated(newEmployee);
        onClose(); 
    } catch (error) {
        console.error('Error creating Employee:', error);
        setError('An error occurred while creating the Employee.');
    }
  };
  
    return (
        <div className="modal-overlay">
        <div className="modal-content">
          <button className="close-button" onClick={onClose}>✖</button>
          <h2>Create New Employee</h2>
          <div className="modal-info">
            <div>
              <label>Employee Name: </label>
              <input
                type="text"
                value={employeeName}
                onChange={(e) => setEmployeeName(e.target.value)}
                placeholder="Enter employee name"
                className="input-field"
              />
            </div>
            <div>
                <label>Phone Number: </label>
                <input
                    type="tel"
                    value={phoneNumber}
                    onChange={(e) => setPhoneNumber(e.target.value)}
                    placeholder="05X-XXX-XXXX"
                    className="input-field"
                    pattern="[0-9]{3}-[0-9]{3}-[0-9]{4}"
                    title="Phone number format: 053-456-7890"
                />
            </div>
            <div>
                <label>Email: </label>
                <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    placeholder="Enter employee email"
                    className="input-field"
                    pattern="[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}"
                    title="Please enter a valid email address"
                    required
                />
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
                <option value={1}>Morning</option>
                <option value={2}>Noon</option>
                <option value={3}>Evening</option>
                <option value={4}>Flexible</option>
              </select>
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
          </div>

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
                        <option key={level} value={level}>
                            {level}
                        </option>
                        ))}
                    </select>
                    </td>
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
          <div>
            <label>Password: </label>
            <input
            type={showPassword ? "text" : "password"}
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Enter password"
            className="input-field"
            pattern="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"
            title="Password must be at least 8 characters long and contain both letters and numbers"
            required
            />
            <button
            type="button"
            onClick={() => setShowPassword(!showPassword)}
            className="toggle-password"
            >
            {showPassword ? "Hide" : "Show"}
            </button>
          </div>

          <div className="modal-actions">
            <button className="save-button" onClick={handleSubmit}>
              <i className="fas fa-save"></i> Save Employee
            </button>
          </div>
        </div>
      </div>
    );
  };
  
  export default CreateEmployeeModal;
  

export enum SkillEnum {
    Python = "Python",
    SQL = "SQL",
    API = "API",
    Java = "Java",
    UI = "UI"
}

export enum LanguageEnum {
    English = "English",
    Spanish = "Spanish",
    Hebrew = "Hebrew",
    Russian = "Russian",
    Persian = "Persian"
}

export enum TimeZonesEnum
{
    Morning = "Morning",
    Noon = "Noon",
    Evening = "Evening",
    Flexible = "Flexible"
}

export const skillEnumToId = (skill : SkillEnum): number => {
    const entries = Object.entries(SkillEnum);
    for (const [key, value] of entries) {
        if (key === skill) {
            return Number(value);
        }
    }
    return -1;
}

export const languageEnumToId = (language : LanguageEnum): number => {
    const entries = Object.entries(LanguageEnum);
    for (const [key, value] of entries) {
        if (key === language) {
            return Number(value);
        }
    }
    return -1;
}

export const timeZoneEnumToId = (timeZone : TimeZonesEnum): number => {
    const entries = Object.entries(TimeZonesEnum);
    for (const [key, value] of entries) {
        if (key === timeZone) {
            return Number(value);
        }
    }
    return -1;
}

export const getSkillStringByIndex  = (index: number): string => {
    const skills = Object.values(SkillEnum);
    console.log("skills list: " + skills);
    console.log(index + " " + skills[index]);
    return skills[index] || "Other"; 
}

export const getLanguageStringByIndex  = (index: number): string => {
    const languages = Object.values(LanguageEnum); 
    console.log("Langs list: " + languages);
    console.log(index + " " + languages[index]);
    return languages[index] || "Other"; 
}

export const getTimeZoneStringByIndex  = (index: number): string => {
    const timeZones = Object.values(TimeZonesEnum); 
    console.log("timeZones list: " + timeZones);
    console.log(index + " " + timeZones[index]);
    return timeZones[index] || "Other"; 
}




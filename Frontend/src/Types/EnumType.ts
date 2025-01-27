
export enum SkillEnum {
    Programming = "Programming",
    Communication = "Communication",
    Leadership = "Leadership",
    ProblemSolving = "ProblemSolving",
    Creativity = "Creativity",
}

export enum LanguageEnum {
    English = "English",
    Spanish = "spanish",
    Hebrew = "Hebrew",
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

export const getLanguageStringByIndex  = (index: number): string => {
    const languages = Object.values(LanguageEnum); 
    console.log("Langs list: " + languages);
    console.log(index + " " + languages[index]);
    return languages[index] || "Other"; 
}

export const getSkillStringByIndex  = (index: number): string => {
    const skills = Object.values(SkillEnum);
    console.log("skills list: " + skills);
    console.log(index + " " + skills[index]);
    return skills[index] || "Other"; 
}
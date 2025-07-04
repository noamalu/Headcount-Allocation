export enum SkillEnum {
    Python, // 0
    SQL,    // 1
    API,    // 2
    Java,   // 3
    UI      // 4
  }

export enum LanguageEnum {
    English = "English",
    Hebrew = "Hebrew",
    Spanish = "Spanish"
    // Russian = "Russian",
    // Persian = "Persian"
}

export enum TimeZonesEnum
{
    Morning = "Morning",
    Noon = "Noon",
    Evening = "Evening",
    Flexible = "Flexible"
}

export enum AbsenceReasonEnum {
    ReserveDuty = "Reserve Duty",
    MaternityPaternityLeave = "Maternity / Paternity Leave",
    StudyLeave = "Study Leave",
    SickLeave = "Sick Leave",
    MourningLeave = "Mourning Leave",
    LongVacation = "Long Vacation",
    PersonalLeave = "Personal Leave",
    MissionAbroad = "Mission Abroad",
    Other = "Other"
}

export const skillEnumToId = (skill : SkillEnum): number => {
    // const entries = Object.entries(SkillEnum);
    // for (const [key, value] of entries) {
    //     if (key === skill) {
    //         return Number(value);
    //     }
    // }
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

export const absenceReasonEnumToId = (absenceReason : AbsenceReasonEnum): number => {
    // const entries = Object.entries(AbsenceReasonEnum);
    // for (const [key, value] of entries) {
    //     if (key === absenceReason) {
    //         return Number(value);
    //     }
    // }
    // return -1;
    return Object.values(AbsenceReasonEnum).indexOf(absenceReason);
}

export const getSkillStringByIndex  = (index: number): string => {
    // const skills = Object.values(SkillEnum);
    // return skills[index] || "Other"; 
    return "Temporary"
}

export const getLanguageStringByIndex  = (index: number): string => {
    const languages = Object.values(LanguageEnum); 
    return languages[index] || "Other"; 
}

export const getTimeZoneStringByIndex  = (index: number): string => {
    const timeZones = Object.values(TimeZonesEnum); 
    return timeZones[index] || "Other"; 
}

export const getAbsenceReasonStringByIndex  = (index: number): string => {
    const absenceReasons = Object.values(AbsenceReasonEnum); 
    return absenceReasons[index] || "Other"; 
}

export const getAbsenceReasonStringByEnumString = (enumString: string): string => {
    const entries = Object.entries(AbsenceReasonEnum);
    for (const [key, value] of entries) {
        if (key.toString() === enumString) {
            return value;
        }
    }
    return "Other";
}

export const SkillLabels: Record<SkillEnum, string> = {
    [SkillEnum.Python]: "Python Programming",
    [SkillEnum.SQL]: "SQL Development",
    [SkillEnum.API]: "API Integration",
    [SkillEnum.Java]: "Java Programming",
    [SkillEnum.UI]: "UI Design"
};

export const getSkillLabel = (typeId: SkillEnum): string => {
    return SkillLabels[typeId] ?? "Unknown Skill";
};
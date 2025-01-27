import { LanguageEnum, getLanguageStringByIndex } from "./EnumType";

export interface Language {
    languageId: number; 
    languageTypeId: number; 
    level:number;
}

export const formateLanguage = (type: number): string => {
    return getLanguageStringByIndex(type); // פורמט יום/חודש/שנה
};




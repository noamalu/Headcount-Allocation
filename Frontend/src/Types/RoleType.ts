export interface Role {
    id: number; 
    name: string;
    employee: string,
    description: string;
    attributes: { attribute: string; requiredRank: number | string; priority: number }[];
  }
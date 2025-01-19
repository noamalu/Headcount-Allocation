export const storage = sessionStorage;

export function setToken(value: string) {
  storage.setItem('token', value);
}

export function getToken(): string | null {
  return storage.getItem('token');
}
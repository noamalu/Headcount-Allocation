const API_BASE_URL = 'http://132.73.84.247:5000';

export const APIClient = async (
  endpoint: string,
  options: RequestInit = {}
): Promise<any> => {
  const defaultHeaders = {
    'Content-Type': 'application/json',
  };

  const config: RequestInit = {
    ...options,
    headers: {
      ...defaultHeaders,
      ...options.headers,
    },
  };

  try {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, config);
    console.log('Response:', response); 
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    const data = await response.json();
    console.log('Data:', data); 
    return data;
  } catch (error) {
    console.error('Error in APIClient:', error);
    throw error;
  }
};

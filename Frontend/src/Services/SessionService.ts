import { APIClient } from './APIClient';
import { fetchResponse } from './GeneralService';

export const storage = window.localStorage;
class SessionService {

    static async login(userName: string, password: string): Promise<number | null> {
        console.log("Attempt to login with user: " + userName);
        try {
            const response = await APIClient(`/api/Employee/Login?userName=${encodeURIComponent(userName)}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json', 
                },
                body: `"${password}"`,  
            });
            if (!response.errorOccured) {
                return fetchResponse(response);
            } else {
                throw new Error("Login failed: " + JSON.stringify(response, null, 2));
            }
        } catch (error) {
            console.error("Error during login:", error);
            throw error;
        }
    }

    static async isAdmin(userId: number): Promise<boolean> {
        console.log("Attempt to check if user is an admin: " + userId);
        try {
            const response = await APIClient(`/api/Employee/${userId}/Admin`, { method: 'GET' });
            console.log('IsAdmin Response:', response); 
            if (!response.errorOccured) {
                return  fetchResponse(response); 
            } else {
                throw new Error("Failed to GetEmployeeById: " + JSON.stringify(response, null, 2));
            }
        } catch (error) {
            console.error("Error during login:", error);
            throw error;
        }
    }
}

export default SessionService;

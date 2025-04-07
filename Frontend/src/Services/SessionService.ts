import { APIClient } from './APIClient';
import { fetchResponse } from './GeneralService';

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
}

export default SessionService;

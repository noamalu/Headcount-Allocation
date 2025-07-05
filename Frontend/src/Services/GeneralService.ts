import ClientResponse from './Response';

export async function fetchResponse<T>(
    responsePromise: Promise<any>
  ): Promise<T> {
    try {
      const serverResponse = await responsePromise;
      console.log('Server Response:', serverResponse); 
      if (!serverResponse) {
        throw new Error('Server response is undefined');
      }
  
      if (!('errorOccured' in serverResponse) || !('value' in serverResponse)) {
        console.error('Response format is incorrect:', serverResponse);
        throw new Error('Response format is incorrect');
      }
      if (serverResponse.errorOccured) {
        return Promise.reject(serverResponse.errorMessage || 'Unknown error');
      }
      console.log('Returned value from fetchResponse:', serverResponse.value); 
      return serverResponse.value;
    } catch (e) {
      console.error('Error in fetchResponse:', e);
      return Promise.reject('An error occurred while processing your request.');
    }
    return responsePromise;
  }
  

import ClientResponse from './Response';

export async function fetchResponse<T>(
    responsePromise: Promise<any>
  ): Promise<T> {
    try {
      const serverResponse = await responsePromise;
      console.log('Server Response:', serverResponse); // לוג נוסף לבדיקה
      if (!serverResponse) {
        throw new Error('Server response is undefined');
      }
  
      // בדיקה אם המבנה תואם
      if (!('errorOccured' in serverResponse) || !('value' in serverResponse)) {
        console.error('Response format is incorrect:', serverResponse);
        throw new Error('Response format is incorrect');
      }
  
      if (serverResponse.errorOccured) {
        return Promise.reject(serverResponse.errorMessage || 'Unknown error');
      }
      return serverResponse.value;
    } catch (e) {
      console.error('Error in fetchResponse:', e);
      return Promise.reject('An error occurred while processing your request.');
    }
    return responsePromise;
  }
  

// export async function fetchResponse<T>(
//   responsePromise: Promise<ClientResponse<T>>
// ): Promise<T> {
//   try {
//     const serverResponse = await responsePromise;
//     if (serverResponse.errorOccured) {
//       return Promise.reject(serverResponse.errorMessage);
//     }
//     return serverResponse.value;
//   } catch (error) {
//     console.error('Error:', error);
//     throw new Error('An error occurred while processing your request.');
//   }
// }

// api base url and fetch function

// Platform is a built-in API provided by React Native 
// to help determine which platform (iOS or Android) the app is running on.
import { AppRegistry, Platform } from "react-native";

// const API_URL = "https://10.0.2.2:7093/api/Log";
const LOCAL_IP = "10.108.137.20"; // ZBC
// const LOCAL_IP = "192.168.0.198"; // home
// const LOCAL_IP = "172.20.10.3"; // hotspot

export const API_BASE_URL =
Platform.OS === "android"? "http://10.0.2.2:5105/" //  Android Emulator
: Platform.OS === "ios" ? `http://${LOCAL_IP}:5105/` //  Physical iPhone (use local network IP)
: `http://${LOCAL_IP}:5105/`; //  Web & Physical Android

/* Use TypeScript generics to specify the expected return type
Now, you can specify the expected data type when calling fetchData:

interface User {
  id: number;
  name: string;
  email: string;
}
  self define what type to expect, and TypeScript will enforce it.
const users = await fetchData<User[]>("users");
console.log(users?.[0].name); // Now TypeScript knows 'name' exists on User
*/
export const fetchData = async <T>(
  endpoint: string,
  token?: string
): Promise<T | null>=> {
    const API_URL = `${API_BASE_URL}${endpoint}`;
    try {
        const response = await fetch(API_URL, {
          headers: token? {Authorization: `Bearer ${token}`} : undefined
        });

        if(!response.ok) throw new Error(response.statusText);
        return await response.json() as T; // Convert the response to type T
    } catch (error) {
        console.error(`API fetch error (${endpoint}): `, error);
        return null;
    }
};

// Add token and Authorization for future
// The token is optional, so the type is string | undefined
// The token is only added to the headers if it exists

// POST (or any request with a body) requires a second argument
// The second argument is an object with the method, headers, and body properties
export const postData = async <RequestType, ResponseType>(
  endpoint: string, 
  data: RequestType,
  token?: string
): Promise<ResponseType | null> => {
    const API_URL = `${API_BASE_URL}${endpoint}`;
    try {
        const response = await fetch(API_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                ... (token && { Authorization: `Bearer ${token}` })
            },
            body: JSON.stringify(data)
        });

        if(!response.ok) throw new Error(response.statusText);
        return await response.json() as ResponseType; // Convert the response to type T
    } catch (error) {
        console.error(`API post error (${endpoint}): `, error);
        return null;
    }
}
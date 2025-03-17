// user related api calls
// fetch data to get user by email
import { postData, fetchData } from "./apiConfig";
import { User, UserLogin, UserLoginResponse , UserRegistration, UserUpdate } from "../types/userRelated";
import { API_BASE_URL } from "./apiConfig";

// login user, return token
export const loginUser = async (credentials: UserLogin): Promise<UserLoginResponse | null> => {
    return await postData<UserLogin, UserLoginResponse>("Login", credentials);
};

// get user by email
export const getUserByEmail = async (email: string, token: string): Promise<User | null> => {
    const endpoint = `GetUserIdByEmail?email=${encodeURIComponent(email)}`;
    return await fetchData<User>(endpoint, token);
};

// register new user, no token needed
export async function registerUser(newUser: UserRegistration): Promise<User> {
    // 1) Build query params
    const queryParams = new URLSearchParams({
      // If your endpoint requires `id`, uncomment and ensure it's a string
      // id: newUser.id?.toString() || "0",
  
      first_name: newUser.first_name,
      last_name: newUser.last_name,
      email: newUser.email,
      password: newUser.password,
      telephone: newUser.telephone,
      is_active: "true",
      type: "user",
      // user: newUser.user || "",
      // salt: newUser.salt || "",
    });
  
    console.log(`Register user query params: ${queryParams.toString()}`);
  
    // 2) Make the fetch call
    const response = await fetch(`${API_BASE_URL}AddUser?${queryParams.toString()}`, {
      method: "POST",
      // If your server expects JSON body, you’d do method: "POST", body: JSON.stringify(...)
      // but from your screenshot, it looks like it expects query params
    });
  
    // 3) Check for errors
    if (!response.ok) {
      throw new Error("Failed to register user");
    }
  
    // 4) Parse the newly created user from the response
    const createdUser: User = await response.json();
    return createdUser;
  }

  // update user
  export async function editUser(userId: number, updatedData: UserUpdate): Promise<User> {
    const queryParams = new URLSearchParams({
        id: userId.toString(), // ✅ Pass user ID separately
        ...(updatedData.first_name && { first_name: updatedData.first_name }),
        ...(updatedData.last_name && { last_name: updatedData.last_name }),
        ...(updatedData.telephone && { telephone: updatedData.telephone }),
        ...(updatedData.password && { password: updatedData.password }),
    });

    const response = await fetch(`${API_BASE_URL}UpdateUser?${queryParams.toString()}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
    });

    if (!response.ok) {
        throw new Error("Failed to update user");
    }

    return response.json(); // Return updated user object
}
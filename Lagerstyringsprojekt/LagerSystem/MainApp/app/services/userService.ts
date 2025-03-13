// user related api calls
// fetch data to get user by email
import { postData, fetchData } from "./apiConfig";
import { User, UserLogin, UserLoginResponse , UserRegistration, UserUpdate } from "../types/userRelated";

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
export const registerUser = async (newUser: UserRegistration): Promise<User | null> => {
    return await postData<UserRegistration, User>("AddUser", newUser);
};
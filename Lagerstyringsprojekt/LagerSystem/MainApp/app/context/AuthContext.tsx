// global state for user authentication

import  React, { createContext, useContext, useEffect, useState } from 'react';
import { loginUser, registerUser, getUserByEmail } from '../services/userService';
import { User, UserLogin, UserLoginResponse , UserRegistration, UserUpdate } from '../types/userRelated';
import { API_BASE_URL } from "../services/apiConfig";
//  Used for securely saving sensitive info (token).
import * as SecureStore from "expo-secure-store";

// The AuthContextType interface defines the shape of the context object, 
// specifying the types of data and functions that will be available globally in your application.
// 1. User data, 2. Function to login, 3. Function to register, 
// 4. Function to logout, 5. Function to update user info 

interface AuthContextType {
    // Logged-in user or null if logged out.
    user: User | null;
    token: string | null;
    login: (credentials: UserLogin) => Promise<boolean>;
    register: (user: UserRegistration) => Promise<void>;
    logout: () => Promise<void>;
    // updateUser: (userUpdate: UserUpdate) => Promise<string | null>;
    
    // Function to refresh user data
    // fetchUser: () => void;
}

// Initial default values when context is first created.
// Real values are provided by the AuthProvider below.
// AuthContext object provides initial default values for the context 
// when it is first created. These default values are placeholders 
// and will be replaced by real implementations provided by the AuthProvider.
const AuthContext = createContext<AuthContextType>({
    user: null,
    token: null,
    login: async () => false,
    register: async () => {},
    logout: async () => {},
    
    // fetchUser: () => {}
});

// core of the context
// The AuthProvider component will provide the actual implementations for these functions. 
// This is a wrapper component; it provides state (user, token) 
// and functions to the entire app (wrapped by it).
// These functions will be available to any component that consumes the AuthContext.
export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    // useState() is a React hook that initializes the user state to null.
    // The setUser function is used to update the user state.
    // The token state is initialized to null.
    // The setToken function is used to update the token state.
    const [ user, setUser ] = useState<User | null>(null);
    const [ token, setToken ] = useState<string | null>(null);

    useEffect(() => {
        const initializeAuth = async () => {    
            const storedToken = await SecureStore.getItemAsync("token");
            const storedUserString = await SecureStore.getItemAsync("user");

            if (storedToken) {
                setToken(storedToken);
            }
            if (storedUserString) {
                const storedUser: User = JSON.parse(storedUserString);
                setUser(storedUser);
            }
        };
        initializeAuth();
    }, []);
    // useEffect Runs once when your app loads (due to empty array []).
    // It calls initializeAuth() which retrieves the token from SecureStore.
    // If a token exists, it updates the token state (is logged in).
    // also the logged in user
    // so the app will remember the user's login state even after the app is closed and reopened.
    // unless logout

    // if log in successful, get the user by email
    const login = async (credentials: UserLogin): Promise<boolean> => {
        const response = await loginUser(credentials);

        if (response?.token) {
            setToken(response.token);
            await SecureStore.setItemAsync("token", response.token);

            // fetch user info
            const loggedinUser = await getUserByEmail(credentials.email, response.token);

            if (loggedinUser) {
                setUser(loggedinUser);
                await SecureStore.setItemAsync("user", JSON.stringify(loggedinUser));
                
                console.log("Login successful:", response.message);
                console.log("User info fetched:", loggedinUser);

                return true; // clearly indicate success
            } else {
                console.log("Login successful:", response.message);
                console.log("Failed to fetch user info");

                return false; // clearly indicate failure
            }
            
        } else {
            console.log("Login failed:", response);
            return false; // clearly indicate failure
        }
    };

    const register = async (userInfo: UserRegistration) => {
        const newUser = await registerUser(userInfo);
        if(newUser) {
            console.log("Registration successful:", newUser);
            // Here, you would fetch and set user profile info
            // Usually, prompt user to log in afterward
        }
    };

    const logout = async () => {
        setUser(null);
        setToken(null);
        await SecureStore.deleteItemAsync("token");
        console.log("Logout successful");
    };

    return (
        <AuthContext.Provider value={{ user, token, login, register, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
// The useAuth() hook is a custom hook that provides easy access to the AuthContext object.
// so in other place of code, dont need to :const auth = useContext(AuthContext);
// just use const auth = useAuth();
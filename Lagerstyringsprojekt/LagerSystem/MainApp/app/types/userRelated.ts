export interface User {
    id: number;
    first_name: string;
    last_name: string;
    email: string;
    telephone: string;
    is_active: boolean;
    type: string;
}
export interface UserLogin {
    email: string;
    password: string;
}

export interface UserToken {
    token: string;	
    message: string;
    status_code: number;
}

export interface UserRegistration {
    first_name: string;
    last_name: string;
    email: string;
    telephone: string;
    password: string;
}

export interface UserUpdate {
    first_name?: string;
    last_name?: string;
    telephone?: string;
    password?: string;
}

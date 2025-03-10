export interface IUser{
    id: number;
    first_name: string;
    last_name: string;
    email: string;
    password: string;
    telephone: string;
    is_active: boolean;
    type: string;
    salt: string;   
}

export interface IEditUser{
    id: number;
    first_name: string;
    last_name: string;
    telephone: string;
    password: string;
}
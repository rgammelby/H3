import ApiClient from "./ApiClient";
import { IUser } from "../Interfaces/User";

class UserApi{
    private client: ApiClient;

    constructor(client: ApiClient){
        this.client = client;
    }

    async fetchAllUsers(): Promise<IUser[]>{
        return this.client.request<IUser[]>("GetAllUsers");
    }
}

export default UserApi;
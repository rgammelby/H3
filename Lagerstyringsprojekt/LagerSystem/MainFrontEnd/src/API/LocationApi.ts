import ApiClient from "./ApiClient";
import { ICupboards, IRoom } from "../Interfaces/Location";

class LocationApi{
    private client: ApiClient;

    constructor(client: ApiClient){
        this.client = client;
    }

    async fetchAllCupboards(): Promise<ICupboards[]>{
        return this.client.request<ICupboards[]>("api/Location/GetAllCupboards");
    }

    async fetchAllRooms(): Promise<IRoom[]>{
        return this.client.request<IRoom[]>("api/Location/GetAllRooms");
    }
}

export default LocationApi;
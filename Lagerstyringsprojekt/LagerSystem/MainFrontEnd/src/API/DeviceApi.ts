import ApiClient from "./ApiClient";
import { IDevice } from "../Interfaces/Device";

class DeviceApi {
    private client: ApiClient;

    constructor(client: ApiClient){
        this.client = client;
    }

    async fetchAllDevices(): Promise<IDevice[]>{
        return this.client.request<IDevice[]>("api/Device");
    }
}

export default DeviceApi;
import ApiClient from "./ApiClient";
import { IDeviceTypes } from "../Interfaces/DeviceTypes";

class DeviceTypeApi{
    private client: ApiClient;

    constructor(client: ApiClient){
        this.client = client;
    }

    async fetchDeviceTypes(): Promise<IDeviceTypes[]>{
        return this.client.request<IDeviceTypes[]>("api/DeviceType/GetAllDeviceTypes");
    }
}

export default DeviceTypeApi;
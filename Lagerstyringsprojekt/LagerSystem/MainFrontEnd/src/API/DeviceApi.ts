import ApiClient from "./ApiClient";
import { IDevice, IDeviceForm } from "../Interfaces/Device";

class DeviceApi {
    private client: ApiClient;

    constructor(client: ApiClient){
        this.client = client;
    }

    async fetchAllDevices(): Promise<IDevice[]>{
        return this.client.request<IDevice[]>("api/Device");
    }

    async addDevice(device: IDeviceForm): Promise<IDevice>{
        return this.client.request<IDevice>("api/Device", "POST", device);
    }

    async updateDevice(id: number, device: IDeviceForm) : Promise<IDevice>{
        return this.client.request<IDevice>(`api/Device?id=${id}`, "PUT", device)
    }
}

export default DeviceApi;
import ApiClient from "./ApiClient";
import { IAllDeviceOverview } from "../Interfaces/DeviceOverview";

class DeviceOverviewApi {
    private client: ApiClient;

    constructor(client: ApiClient){
        this.client = client;
    }

    async fetchAllDeviceOverviews(): Promise<IAllDeviceOverview[]> {
        return this.client.request<IAllDeviceOverview[]>("api/DeviceOverview");
    }
}

export default DeviceOverviewApi;
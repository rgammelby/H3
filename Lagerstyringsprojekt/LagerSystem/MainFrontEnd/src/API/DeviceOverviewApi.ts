import ApiClient from "./ApiClient";
import { IAllDeviceOverview, IUpdateDeviceOverview } from "../Interfaces/DeviceOverview";

class DeviceOverviewApi {
    private client: ApiClient;

    constructor(client: ApiClient){
        this.client = client;
    }

    async fetchAllDeviceOverviews(): Promise<IAllDeviceOverview[]> {
        return this.client.request<IAllDeviceOverview[]>("api/DeviceOverview");
    }

    async addDeviceOverview(body: FormData): Promise<IAllDeviceOverview> {
        return this.client.requestFormData("api/DeviceOverview", "POST", body);
    }

    async updateDeviceOverview(id: number, formData: FormData): Promise<IUpdateDeviceOverview> {
        return this.client.requestFormData<IUpdateDeviceOverview>(`api/DeviceOverview/${id}`, "PUT", formData);
    }
}

export default DeviceOverviewApi;
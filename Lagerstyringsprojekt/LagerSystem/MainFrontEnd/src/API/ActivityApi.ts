import ApiClient from "./ApiClient";
import { IActivity } from "../Interfaces/Activity";

class ActivityApi {
    private client: ApiClient;

    constructor(client: ApiClient){
        this.client = client;
    }

    async fetchAllActivities(): Promise<IActivity>{
        return this.client.request<IActivity>("GetAllActivities");
    }
}

export default ActivityApi;
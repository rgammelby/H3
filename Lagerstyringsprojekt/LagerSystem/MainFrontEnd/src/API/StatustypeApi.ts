import ApiClient from "./ApiClient";
import { IStatusTypes } from "../Interfaces/StatusType";

class StatusTypeApi{
    private client: ApiClient;

    constructor(client: ApiClient){
        this.client = client;
    }

    async fetchAllStatusTypes(): Promise<IStatusTypes[]>{
        return this.client.request<IStatusTypes[]>("api/StatusType/GetAllStatusTypes");
    }
}

export default StatusTypeApi;
import ApiClient from "./ApiClient";
import { ILog } from "../Interfaces/Log";

class LogApi{
    private client: ApiClient;

    constructor(client: ApiClient){
        this.client = client;
    }

    async fecthAllLogs(): Promise<ILog>{
        return this.client.request<ILog>("api/Log");
    }
}

export default LogApi;
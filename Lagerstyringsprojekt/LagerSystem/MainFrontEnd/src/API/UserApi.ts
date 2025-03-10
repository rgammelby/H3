import ApiClient from "./ApiClient";
import { IUser, IEditUser } from "../Interfaces/User";

class UserApi {
  private client: ApiClient;

  constructor(client: ApiClient) {
    this.client = client;
  }

  async fetchAllUsers(): Promise<IUser[]> {
    return this.client.request<IUser[]>("GetAllUsers");
  }

  async disbaleUser(id = 0): Promise<Response> {
    return this.client.requestNoJson(`DisableUser?id=${id}`, "PUT");
  }

  async editUser(user: IEditUser): Promise<Response> {
    console.log(`UpdateUser?id=${user.id}${user.first_name ? `&first_name=${user.first_name}` : ""}${
        user.last_name ? `&last_name=${user.last_name}` : ""}
        ${user.telephone ? `&telephone=${user.telephone}` : ""}
        ${user.password ? `&password=${user.password}` : ""}
        `);
    return this.client.requestNoJson(
      `UpdateUser?id=${user.id}${user.first_name ? `&first_name=${user.first_name}` : ""}${
        user.last_name ? `&last_name=${user.last_name}` : ""}
        ${user.telephone ? `&telephone=${user.telephone}` : ""}
        ${user.password ? `&password=${user.password}` : ""}
        `,
      "PUT"
    );
  }
}

export default UserApi;

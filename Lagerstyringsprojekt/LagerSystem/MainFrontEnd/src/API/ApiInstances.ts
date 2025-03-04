import ApiClient from "./ApiClient";
import DeviceOverviewApi from "./DeviceOverviewApi";
import ActivityApi from "./ActivityApi";
import DeviceTypeApi from "./DeviceTypeApi";
import LocationApi from "./LocationApi";
import LogApi from "./LogApi";
import StatusTypeApi from "./StatustypeApi";
import UserApi from "./UserApi";
import DeviceApi from "./DeviceApi";

const apiClient = new ApiClient("http://192.168.1.19:5000/");

export const DeviceOverview = new DeviceOverviewApi(apiClient);

export const Device = new DeviceApi(apiClient);

export const Activity = new ActivityApi(apiClient);

export const DeviceType = new DeviceTypeApi(apiClient);

export const Location = new LocationApi(apiClient);

export const Log = new LogApi(apiClient);

export const StatusType = new StatusTypeApi(apiClient);

export const User = new UserApi(apiClient);
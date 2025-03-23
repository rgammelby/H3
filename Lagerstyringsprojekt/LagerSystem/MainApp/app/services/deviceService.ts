// device-realted api calls to fetch data

import {fetchData} from "./apiConfig";
import { DeviceStatus, DeviceType, DeviceOverview, Room, Cupboard, Device } from "../types/deviceRelated";

//  you don’t need await because fetchData() already returns a Promise.
// This directly returns the fetchData() promise.
// The function does not need async because fetchData() is already asynchronous.
// The component that calls getDeviceOverviews() can use await when calling it.
export const getDeviceOverviews =  () => fetchData<DeviceOverview[]>("api/DeviceOverview") || [];
export const getDeviceTypes =  () => fetchData<DeviceType[]>("api/DeviceType/GetAllDeviceTypes") || [];
export const getDeviceStatuses =  () => fetchData<DeviceStatus[]>("api/StatusType/GetAllStatusTypes") || [];
export const getRooms =  () => fetchData<Room[]>("api/Location/GetAllRooms") || [];
export const getCupboards =  () => fetchData<Cupboard[]>("api/Location/GetAllCupboards") || [];
export const getDevices =  () => fetchData<Device[]>("api/Device") || [];

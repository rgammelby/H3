// Global state for device information

import { createContext, useState, useEffect, ReactNode } from "react";
import { DeviceStatus, DeviceType, DeviceOverview, Room, Cupboard, Device } from "../types/deviceRelated";
import { getDeviceOverviews, getDeviceTypes, getDeviceStatuses, getRooms, getCupboards, getDevices } from "../services/deviceService";
import { API_BASE_URL } from "../services/apiConfig";

// define what to store globally
interface DeviceContextType {
    cupboardList: Cupboard[];
    deviceTypeList: DeviceType[];
    deviceStatusList: DeviceStatus[];
    roomList: Room[];
    deviceList: Device[];
    deviceOverviewList: DeviceOverview[];
    // when the app statrd, load device data once
    // refresh the list (e.g. after add a new device or update a device)
    fetchDevices: () => void; //  Function to refresh device list
}

// createing a Context (storage room) where stores DeviceContextType data
// it could be null before loading data
// defaulted to null (null)
export const DeviceContext = createContext<DeviceContextType | null>(null);

// create provider
// children is everything inside <DeviceProvider>...</DeviceProvider>
// We wrap our app inside DeviceProvider so that every component can access context data.
export const DeviceProvider = ({ children }: {children : ReactNode}) => {
    // Think of useState() as a box where we store values that change over time.
    // The first value is the current value, and the second value is a function to update the value.
    // useState() returns an array with two values: the current state and a function to update the state.
    // The initial value of roomList is an empty array.
    // When the setRoomList function is called, React updates the roomList state >> the UI re-renders
    const [roomList, setRoomList] = useState<Room[]>([]);
    const [cupboardList, setCupboardList] = useState<Cupboard[]>([]);
    const [deviceTypeList, setDeviceTypeList] = useState<DeviceType[]>([]);
    const [deviceStatusList, setDeviceStatusList] = useState<DeviceStatus[]>([]);
    const [deviceOverviewList, setDeviceOverviewList] = useState<DeviceOverview[]>([]);
    const [deviceList, setDeviceList] = useState<Device[]>([]);

    /* Think of useEffect(() => {...}, []) as "Run This Code When the App Starts"
    useEffect() is a hook that runs side effects in function components.
    It runs after the component renders and after every update.
    */
    useEffect(() => {
        // Fetch data from the API
         // We cannot use await directly inside useEffect()
        // So define an async function inside useEffect() and then call it.
        const fetchData = async () => {
            // Call the functions from deviceService.ts
            // getRooms() returns a Promise<Room[]> and sets the roomList state
            try{
                // Log API Calls Before Fetching
                console.log("Fetching data from APIs...");
                
                // fetch all lists at the same time
                const [rooms, cupboards, deviceTypes, deviceStatuses, deviceOverviews] = await Promise.all([
                    getRooms(),
                    getCupboards(),
                    getDeviceTypes(),
                    getDeviceStatuses(),
                    getDeviceOverviews()
                ]);

                // set the state of lists without references
                setRoomList(rooms || []);
                setDeviceTypeList(deviceTypes || []);
                setDeviceStatusList(deviceStatuses || []);

                // merge deviceTypeDetail into DeviceOverviewList
                // deviceTypeDetail is a reference to DeviceType
                // so deviceOverviewList can access deviceTypeDetail to get type_name 
                if( deviceOverviews && deviceTypes){
                    const enrichedDeviceOverviews = deviceOverviews.map((overview) => ({
                        ...overview,
                        deviceTypeDetail: deviceTypes.find( type => type.id === overview.device_type) || undefined,
                        image: overview.image.startsWith("/")
                            ? `${API_BASE_URL}api/image/${overview.id}` // Prepend API_BASE_URL to relative image paths
                            : overview.image
                    })) || [];// Return empty array if `deviceOverviews` is empty
                    
                    setDeviceOverviewList(enrichedDeviceOverviews || []); // Still ensures the list is never null
                    
                }  else {
                    setDeviceOverviewList([]); // Fallback if data is missing
                }

                // merge roomDetail into CupboardList
                if( cupboards&& rooms){
                    const enrichedCupboards = cupboards.map((cupboard) => ({
                        ...cupboard,
                        roomDetail : rooms.find( room => room.id === cupboard.room_id) || undefined
                    })) || [];
                    setCupboardList(enrichedCupboards || []);
                    
                } else {
                    setCupboardList([]);
                }
            } catch (error){
                console.error("Error loading device data: ", error);
            }
        };

        fetchData(); // Call the function when the app starts
    }, []);

    // new useEffect to Run fetchDevices() **only after** cupboardList is updated
    useEffect(() => {
        if(cupboardList.length > 0  && deviceOverviewList.length > 0 && deviceStatusList.length > 0){
            fetchDevices();
        }
    }, [cupboardList, deviceOverviewList, deviceStatusList]); 
    // Only run when these values change (i.e. when the lists are loaded
    
    // Function to refresh device list
    // load the list of all devices separately
    // don't call it inside useEffect() because we might want to refresh it later.
    const fetchDevices = async () => {
        try{
            const devices = await getDevices();
            // Ensure `devices` is an array before proceeding
            if (!devices || !Array.isArray(devices)) {
                console.error("Error: Devices data is missing or not an array.");
                setDeviceList([]);
                return;
            }
    
            if( deviceTypeList && deviceOverviewList && deviceStatusList &&  cupboardList){
                const enrichedDevices = devices.map((device) => ({
                    ...device,
                    deviceOverviewDetail: deviceOverviewList.find(overview => overview.id === device.device_overview_id) || undefined,
                    statusDetail: deviceStatusList.find(status => status.id === device.status) || undefined,
                    locationDetail: cupboardList.find(cupboard => cupboard.id === device.location) || undefined
                }));

                setDeviceList(enrichedDevices || []);
                // console.log("enriched devices: ", enrichedDevices);

            } else {
                console.error("Error: One or more required lists are missing.");
                setDeviceList([]);
                return;
            }
        } catch (error){
            console.log("Error loading device data: ", error);
        }
        
    };

    return(
        // store all lists in context
        // pass the fetchDevices function to the context, so any conponent can call it
        
        // children means anything inside <DeviceProvider>...</DeviceProvider>
        // We wrap our app inside DeviceProvider so that every component can access context data.
        <DeviceContext.Provider value={{roomList, cupboardList, deviceTypeList, deviceStatusList, deviceList, deviceOverviewList, fetchDevices}}>
            {children}
        </DeviceContext.Provider>
    )
}

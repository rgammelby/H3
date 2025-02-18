import React, { useState, useEffect } from "react";
import {
    fetchDevices,
    fetchStatusTypes,
    fetchRooms,
    fetchCupboards,
  } from "../services/deviceService";
import { fetchDeviceOverviews, fetchDeviceTypes } from "../services/deviceOverviewService";

interface Device {
    id: number;
    device_overview_id: number;
    status: number;
    location: number;
    qr: string;
  }
  
  interface DeviceOverview {
    id: number;
    model: string;
    image: string;
    device_type: number;
  }

  interface DeviceType {
    id: number;
    type_name: string;
  }
  
  interface StatusType {
    id: number;
    status_type: string;
  }
  
  interface Room {
    id: number;
    designation: string;
  }
  
  interface Cupboard {
    id: number;
    designation: string;
    room_id: number;
  }

  // customed hook to fetch device-realted data
  export const useDevices = () => {
    const [devices, setDevices] = useState<Device[]>([]);
    const [deviceOverviews, setDeviceOverviews] = useState<DeviceOverview[]>([]);
    const [deviceTypes, setDeviceTypes] = useState<DeviceType[]>([]);
    const [statusTypes, setStatusTypes] = useState<StatusType[]>([]);
    const [rooms, setRooms] = useState<Room[]>([]);
    const [cupboards, setCupboards] = useState<Cupboard[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [
                    devices,
                    deviceOverviews,
                    deviceTypes,
                    statusTypes,
                    rooms,
                    cupboards,
                ] = await Promise.all([
                    fetchDevices(),
                    fetchDeviceOverviews(),
                    fetchDeviceTypes(),
                    fetchStatusTypes(),
                    fetchRooms(),
                    fetchCupboards(),
                ]);

                setDevices(devices);
                setDeviceOverviews(deviceOverviews);
                setDeviceTypes(deviceTypes);
                setStatusTypes(statusTypes);
                setRooms(rooms);
                setCupboards(cupboards);
                setLoading(false);
            } catch (err: any) {
                setError(err.message);
                setLoading(false);
            }
        };
        fetchData();
    }, []);

    return {
        devices,
        deviceOverviews,
        deviceTypes,
        statusTypes,
        rooms,
        cupboards,
        loading,
        error,
      };
    };
    
    export default useDevices;

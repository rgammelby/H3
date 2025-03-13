import React, { useState, useEffect } from "react";
import { Device, DeviceOverview, DeviceType, Location, StatusType, Image } from "../../API/ApiInstances";
import { IDevice } from "../../Interfaces/Device";
import { IAllDeviceOverview } from "../../Interfaces/DeviceOverview";
import { IDeviceTypes } from "../../Interfaces/DeviceTypes";
import { IStatusTypes } from "../../Interfaces/StatusType";
import { ICupboards } from "../../Interfaces/Location";
import { IRoom } from "../../Interfaces/Location";
import DeviceModal from "../../components/ui/DeviceModal";
import UpdateDevice from "../../components/ui/UpdateDevice";

function DevicePage(){
    const [DEVICES, setDevices] = useState<IDevice[]>([]);
    const [OVERVIEWS, setOverviews] = useState<IAllDeviceOverview[]>([]);
    const [DEVICETYPES, setDeviceTypes] = useState<IDeviceTypes[]>([]);
    const [STATUSTYPES, setStatusTypes] = useState<IStatusTypes[]>([]);
    const [ROOMS, setRooms] = useState<IRoom[]>([]);
    const [CUPBOARDS, setCupboards] = useState<ICupboards[]>([]);
    const [DEVICEMODAL, setDeviceModal] = useState<boolean>(false);
    const [UPDATEDEVICEMODAL, setUpdateDeviceModal] = useState<boolean>(false);
    const [SINGLEDEVICE, setSingleDevice] = useState<IDevice>();

    useEffect(() => {
        const fetchData = async () => {
            try{
                const DEVICEDATA: IDevice[] = await Device.fetchAllDevices();
                const OVERVIEWDATA: IAllDeviceOverview[] = await DeviceOverview.fetchAllDeviceOverviews();
                const TYPEDATA: IDeviceTypes[] = await DeviceType.fetchDeviceTypes();
                const STATUSTYPEDATA: IStatusTypes[] = await StatusType.fetchAllStatusTypes();
                const ROOMDATA: IRoom[] = await Location.fetchAllRooms();
                const CUPBOARDDATA: ICupboards[] = await Location.fetchAllCupboards();
                
                setDevices(DEVICEDATA);
                setOverviews(OVERVIEWDATA);
                setDeviceTypes(TYPEDATA);
                setStatusTypes(STATUSTYPEDATA);
                setRooms(ROOMDATA);
                setCupboards(CUPBOARDDATA);
            }
            catch(ex){
                console.error(ex);
            }
        }

        fetchData();
    }, [])

    return(
        <div className="p-6">
        <h1 className="text-3xl font-bold mb-4 text-center">💻 Device Management</h1>

        <div className="overflow-x-auto shadow-lg rounded-lg">
            <table className="table w-full table-zebra">
            <thead className="bg-base-200">
                <tr>
                <th>#</th>
                <th>EnhedsID</th>
                <th>Billed</th>
                <th>Model</th>
                <th>Enheds Type</th>
                <th>Status</th>
                <th>Lokation</th>
                <th>Aktion</th>
                </tr>
            </thead>

            <tbody>
                {/* Loops through all devices using .map() and creates one <tr> row per device.
                    The index is used to display the row number, starting from 1. 
                    DeviceOverview that matches the device_overview_id.
                    Helps us display the model name.
                    Uses deviceOverview.device_type to get the type name (Laptop, Monitor, etc.).
                */}
                {DEVICES.map((device, index) => {
                const overview = OVERVIEWS.find((o) => o.id === device.device_overview_id);
                const deviceType = DEVICETYPES.find((t) => t.id === overview?.device_type);
                const status = STATUSTYPES.find((s) => s.id === device.status);
                const cupboard = CUPBOARDS.find((c) => c.id === device.location);
                const room = ROOMS.find((r) => r.id === cupboard?.room_id);

                return (
                    <tr
                    onClick={() => {
                        setDeviceModal(true);
                        setSingleDevice(device);
                     }}
                    key={device.id}
                    className="hover:bg-base-300 cursor-pointer">
                    <th>{index + 1}</th>
                    <td>{device.id}</td>
                    <td>
                        {
                            overview && overview !== undefined && overview.id ? (
                                <img src={Image(overview.id)} className="w-25 h-auto" />
                            ) :
                            <img src="https://upload.wikimedia.org/wikipedia/commons/6/65/No-Image-Placeholder.svg" style={{width: "25", height: "auto"}} />
                        }
                    </td>
                    <td className="font-semibold flex items-center gap-3">
                        {overview && (
                        <div className="flex-flex-col">
                            <span>{overview.model}</span>
                            <br />
                            <span className="badge badge-ghost badge-sm">
                                {DEVICETYPES.find((type) => type.id === overview.device_type)?.type_name || "Unknown"}
                            </span>
                        </div>
                        )}
                    </td>
                    <td>{deviceType?.type_name || "Unknown"}</td>
                    <td>
                        <span className="badge badge-outline">{status?.status_type || "Unknown"}</span>
                    </td>
                    <td>
                        {room?.designation || "Unknown"} - {cupboard?.designation || "Unknown"}
                    </td>
                    <td onClick={(e) => e.stopPropagation()}>
                        <button
                        className="btn btn-sm btn-primary"
                        onClick={() => {
                            setUpdateDeviceModal(true);
                            setSingleDevice(device);
                        }}
                        >
                            Update
                        </button>
                    </td>
                    </tr>
                );
                })}
            </tbody>
            </table>
        </div>
        {
            DEVICEMODAL && SINGLEDEVICE && (
            <DeviceModal cancelModal={() => setDeviceModal(false)} device={SINGLEDEVICE} />
            )
        }
        {
            UPDATEDEVICEMODAL && SINGLEDEVICE && (
                <UpdateDevice cancelModal={() => setUpdateDeviceModal(false)} />
            )
        }
    </div>
    )
}

export default DevicePage;
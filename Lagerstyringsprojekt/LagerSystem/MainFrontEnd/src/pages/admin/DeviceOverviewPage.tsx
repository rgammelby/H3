import React, { useEffect, useState } from "react";
import { DeviceOverview, DeviceType } from "../../API/ApiInstances";
import { IAllDeviceOverview } from "../../Interfaces/DeviceOverview";
import { IDeviceTypes } from "../../Interfaces/DeviceTypes";

function DeviceOverviewPage() {
    const [OVERVIEWDATA, setOverview] = useState<IAllDeviceOverview[]>([]);
    const [DEVICETYPES, setDeviceTypes] = useState<IDeviceTypes[]>([]);
    const [SELECTEDIMAGE, setSelectedImage] = useState<string | null>();

    useEffect(() => {
        const fetchData = async () => {

            try {
                const OVERVIEW: IAllDeviceOverview[] = await DeviceOverview.fetchAllDeviceOverviews();
                const TYPES: IDeviceTypes[] = await DeviceType.fetchDeviceTypes();

                setOverview(OVERVIEW);
                setDeviceTypes(TYPES);
            }
            catch (ex) {
                console.error(ex);
            }
        };

        fetchData();

    }, [])

    return (
        <div className="p-6">
            <h1 className="text-3xl font-bold mb-4 text-center">📦 Device Overview</h1>
            
            <div className="overflow-x-auto shadow-lg rounded-lg">
                <table className="table w-full table-zebra">
                    {/* Table Head */}
                    <thead className="bg-base-200">
                        <tr>
                            <th>#</th>
                            <th>Model</th>
                            <th>Device Type</th>
                            <th>Image</th>
                            <th>Qty</th>
                            <th>Available</th>
                            <th>Last Ordered</th>
                            <th>Actions</th>
                        </tr>
                    </thead>

                    {/* Table Body */}
                    
                    <tbody>
                        {OVERVIEWDATA.map((device, index) => (
                            <tr key={device.id} className="hover:bg-base-300">
                                <td>{index + 1}</td>
                                <td className="font-semibold">
                                    {device.model}
                                    <br />
                                    <span className="badge badge-ghost badge-sm">{DEVICETYPES.find((type) => type.id === device.device_type)?.type_name || "Unknown"}</span>
                                </td>
                                <td>
                                    {DEVICETYPES.find((type) => type.id === device.device_type)?.type_name || "Unknown"}
                                </td>
                                <td>
                                    <img 
                                        src={ `http://192.168.1.19:5000/api/Image/${device.id}`} 
                                        alt="Device" 
                                        className="w-12 h-12 rounded"
                                        onClick={() => setSelectedImage(`http://192.168.1.19:5000/api/Image/${device.id}`)}
                                     />
                                </td>
                                <td>{device.qty}</td>
                                <td>{device.available_qty}</td>
                                <td>{new Date(device.last_ordered).toLocaleDateString()}</td>
                                <td>
                                <button
                                    className="btn btn-ghost btn-xs ml-2"
                                    onClick={() => setSelectedImage(`http://192.168.1.19:5000/api/Image/${device.id}`)}
                                >
                                    Show Details
                                </button>
                                <button className="btn btn-sm btn-primary">Update</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            {/* Selected Image Modal */}
            {SELECTEDIMAGE && (
                <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
                <div className="bg-white p-6 rounded-lg shadow-lg relative">
                    <button
                    className="absolute top-2 right-2 text-gray-600 hover:text-gray-900"
                    onClick={() => setSelectedImage(null)}
                    >
                    ✖
                    </button>
                    <img src={SELECTEDIMAGE} alt="Full Device" className="max-w-full max-h-[80vh]" />
                </div>
                </div>
            )}
        </div>
    );
}

export default DeviceOverviewPage;
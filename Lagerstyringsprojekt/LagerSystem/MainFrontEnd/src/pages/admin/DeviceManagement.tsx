import React from "react";
import useDevices from "../../hooks/useDevices";

const DeviceManagement = () => {    
    const { devices, deviceOverviews, deviceTypes, statusTypes, rooms, cupboards, loading, error } = useDevices();

    if (loading) return <p>Loading...</p>;
    if (error) return <p className="text-red-500">Error: {error}</p>;

    return(
        <div className="p-6">
        <h1 className="text-3xl font-bold mb-4 text-center">💻 Device Management</h1>

        <div className="overflow-x-auto shadow-lg rounded-lg">
            <table className="table w-full table-zebra">
            <thead className="bg-base-200">
                <tr>
                <th>#</th>
                <th>DeviceID</th>
                <th>Model</th>
                <th>Device Type</th>
                <th>Status</th>
                <th>Location</th>
                <th>QR Code</th>
                <th>Actions</th>
                </tr>
            </thead>

            <tbody>
                {/* Loops through all devices using .map() and creates one <tr> row per device.
                    The index is used to display the row number, starting from 1. 
                    DeviceOverview that matches the device_overview_id.
                    Helps us display the model name.
                    Uses deviceOverview.device_type to get the type name (Laptop, Monitor, etc.).
                */}
                {devices.map((device, index) => {
                const overview = deviceOverviews.find((o) => o.id === device.device_overview_id);
                const deviceType = deviceTypes.find((t) => t.id === overview?.device_type);
                const status = statusTypes.find((s) => s.id === device.status);
                const cupboard = cupboards.find((c) => c.id === device.location);
                const room = rooms.find((r) => r.id === cupboard?.room_id);

                return (
                    <tr key={device.id} className="hover:bg-base-300">
                    <th>{index + 1}</th>
                    <td>{device.id}</td>
                    <td className="font-semibold flex items-center gap-3">
                        {overview && (
                        <div className="flex-flex-col">
                            <span>{overview.model}</span>
                            <br />
                            <span className="badge badge-ghost badge-sm">
                                {deviceTypes.find((type) => type.id === overview.device_type)?.type_name || "Unknown"}
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
                    <td>
                        <span className="text-blue-500">{device.qr}</span>
                    </td>
                    <td>
                        <button className="btn btn-sm btn-primary">Update</button>
                    </td>
                    </tr>
                );
                })}
            </tbody>
            </table>
        </div>
    </div>
  );
};

export default DeviceManagement;
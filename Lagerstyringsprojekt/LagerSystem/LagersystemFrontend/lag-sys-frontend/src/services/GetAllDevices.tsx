import React, { useState, useEffect } from "react";

interface Device {
    id: number;
    description: string;
    device_overview_id: number;
    is_archived: boolean;
    location: number;
    qr: string;
    status: number;
}

interface StatusType {
    [key: number]: string;
}

interface Cupboard {
    [key: number]: string;
}

interface DeviceOverview {
    [key: number]: string;
}

function GetAllDevices() {
    const [devices, setDevices] = useState<Device[]>([]);
    const [statusTypes, setStatusTypes] = useState<StatusType>({});
    const [cupboards, setCupboards] = useState<Cupboard>({});
    const [deviceOverviews, setDeviceOverviews] = useState<DeviceOverview>({});
    const [loading, setLoading] = useState(true);

    // Fetch all devices
    const fetchAllDevices = async () => {
        try {
            const response = await fetch("https://localhost:7093/api/Device/GetAllDevices");
            if (!response.ok) {
                throw new Error("Failed to fetch devices");
            }
            const devicesData: Device[] = await response.json();
            console.log("Devices Data:", devicesData);  // Log to inspect the device data
            setDevices(devicesData);

            // Prepare promises for the related data (status, cupboard, device overview)
            const fetchStatusPromises = devicesData.map((device) =>
                fetch(`https://localhost:7093/GetStatusTypeById/${device.status}`)
                    .then((statusResponse) => statusResponse.json())
                    .then((statusData) => {
                        return { [device.status]: statusData.status_type };
                    })
            );

            const fetchCupboardPromises = devicesData.map((device) =>
                fetch(`https://localhost:7093/GetCupboardById/${device.location}`)
                    .then((cupboardResponse) => cupboardResponse.json())
                    .then(async (cupboardData) => {
                        const cupboardDesignation = cupboardData.designation || "Unknown Cupboard";
                        const roomId = cupboardData.room_id;  // Assuming 'room_id' is present in cupboard data
            
                        let roomDesignation = "Unknown Room";  // Default room designation
                        if (roomId) {
                            try {
                                const roomResponse = await fetch(`https://localhost:7093/GetRoomById/${roomId}`);
                                if (roomResponse.ok) {
                                    const roomData = await roomResponse.json();
                                    roomDesignation = roomData.designation || "Unknown Room";
                                }
                            } catch (error) {
                                console.error(`Failed to fetch room for ID ${roomId}`, error);
                            }
                        }
            
                        return { [device.location]: `${cupboardDesignation} - ${roomDesignation}` };
                    })
            );
            

            const fetchDeviceOverviewPromises = devicesData.map((device) =>
                fetch(`https://localhost:7093/api/DeviceOverview/${device.device_overview_id}`)
                    .then((deviceOverviewResponse) => deviceOverviewResponse.json())
                    .then((deviceOverviewData) => {
                        return { [device.device_overview_id]: deviceOverviewData.model };
                    })
            );

            // Wait for all promises to resolve and gather all data
            const [statusResults, cupboardResults, deviceOverviewResults] = await Promise.all([
                Promise.all(fetchStatusPromises),
                Promise.all(fetchCupboardPromises),
                Promise.all(fetchDeviceOverviewPromises),
            ]);

            // Now update the state once all related data has been fetched
            setStatusTypes(statusResults.reduce((acc, curr) => ({ ...acc, ...curr }), {}));
            setCupboards(cupboardResults.reduce((acc, curr) => ({ ...acc, ...curr }), {}));
            setDeviceOverviews(deviceOverviewResults.reduce((acc, curr) => ({ ...acc, ...curr }), {}));

            setLoading(false);  // Done loading
        } catch (error) {
            console.error("Error fetching data:", error);
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchAllDevices();
    }, []);

    return (
        <>
            {loading ? (
                <div>Loading devices...</div>
            ) : (
                <table>
                    <thead>
                        <tr>
                            <th>Description</th>
                            <th>Status</th>
                            <th>Location</th>
                            <th>QR Code</th>
                            <th>Model</th>
                            <th>Archived</th>
                        </tr>
                    </thead>
                    <tbody>
                        {devices.map((device) => (
                            <tr key={device.id}>
                                <td>{device.description || "No Description"}</td>
                                <td>{statusTypes[device.status] || "Unknown Status"}</td>
                                <td>{cupboards[device.location] || "Unknown Location"}</td>
                                <td>{device.qr || "No QR"}</td>
                                <td>{deviceOverviews[device.device_overview_id] || "Unknown Overview"}</td>
                                <td>{device.is_archived ? "Yes" : "No"}</td>
                            </tr>
                        ))}
                    </tbody>

                </table>
            )}
        </>
    );
}

export default GetAllDevices;

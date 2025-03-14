import React, { useState, useEffect } from "react";

interface DeviceOverview {
    id: number;
    model: string;
    device_type: number;
    image: string;
    qty: number;
    available_qty: number;
    last_ordered: string;
}

function GetAllDeviceOverviews() {
    const [OVERVIEWS, setOverviews] = useState<DeviceOverview[]>([]); // Initialize as an array, not an object
    const [SHOW, setShow] = useState<boolean>(false);

    const GetDeviceOverviews = async () => {
        const response = await fetch('http://localhost:7093/api/DeviceOverview', { method: "GET" });
        const data = await response.json();
        setOverviews(data);  // Assuming the response is an array of devices
    };

    const ShowDeviceOverviews = () => {
        setShow(!SHOW);
    };

    useEffect(() => {
        GetDeviceOverviews();  // Use correct function name here
    }, []);  // Empty dependency array means this runs only once, like componentDidMount

    return (
        <>
            <button onClick={ShowDeviceOverviews}>Show Device Overviews</button>
            {SHOW && (
                <div>
                    {/* Render the device overviews here */}
                    {OVERVIEWS.map((overview) => (
                        <div key={overview.id}>
                            <h3>{overview.model}</h3>
                            <img src={`http://localhost:7093/api/Image/${overview.id}`} alt={overview.model} style={{width: "256px", height: "auto"}}/>
                            <p>Quantity: {overview.available_qty}</p>
                        </div>
                    ))}
                </div>
            )}
        </>
    );
}

export default GetAllDeviceOverviews;

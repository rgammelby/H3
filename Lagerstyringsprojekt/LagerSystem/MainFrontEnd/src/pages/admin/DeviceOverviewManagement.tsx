import React, {useEffect, useState } from "react";
import { fetchDeviceOverviewById, fetchDeviceTypes, fetchDeviceOverviews } from "../../services/deviceOverviewService";
// useEffect allows yo fecth data when the page loads
// useState allows you to store the data fetched in memory

// 1. define the data structures
interface DeviceOverview {
    id: number;
    model: string;
    device_type: number;
    image: string;
    qty: number;
    available_qty: number;
    last_ordered: string;
}

interface DeviceType {
    id: number;
    type_name: string;
}

// 2. define state varialbes (useState)
const DeviceOverviewManagement = () => {
    // deviceOverviews is an array of DeviceOverview objects
    // setDeviceOverviews is a function to update the deviceOverviews array
    // useState([]) initializes the deviceOverviews array as an empty array
    // then setDeviceOverviews updates the deviceOverviews array with the fetched data
    // and the component re-renders with the updated data
    const [deviceOverviews, setDeviceOverviews] = useState<DeviceOverview[]>([]);
    const [deviceTypes, setDeviceTypes] = useState<DeviceType[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [selectedImage, setSelectedImage] = useState<string | null>(null);

    // 3. Fetch Data from API (useEffect)
    // useEffect is a hook that runs after the first render of the component
    // it is used to fetch data from an API, subscribe to a subscription, or perform any side effects
    // useEffect takes a callback function as the first argument and an array of dependencies as the second argument
    // the callback function is called after the component is rendered
    // the array of dependencies is used to specify when the effect should re-run
    // if the array is empty, the effect only runs once after the first render
    // if the array contains variables, the effect runs whenever any of the variables change

    useEffect(() => {
        // useEffect runs the function when the page loads
        // fetchData() calls both APIs at the same time. 
        // Promise.all() waits for both promises to resolve
        // then updates the state variables with the fetched data
        // it stores the result in the user state variable
        const fetchData = async () => {
            try{
                const [deviceOverviews, deviceTypes] = await Promise.all([
                    fetchDeviceOverviews(),
                    fetchDeviceTypes()
                ]);

                setDeviceOverviews(deviceOverviews);
                setDeviceTypes(deviceTypes);
                setLoading(false);
            } catch (err: any) {
                setError(err.message);
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    // 4. Handle Loading and Errors
    if (loading) return <p>Loading...</p>;
    if (error) return <p className="text-red-500">{error}</p>;
    
    // 5. Render the Component
    // The component returns a table with the device overview data
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
                        {deviceOverviews.map((device, index) => (
                            <tr key={device.id} className="hover:bg-base-300">
                                <td>{index + 1}</td>
                                <td className="font-semibold">
                                    {device.model}
                                    <br />
                                    <span className="badge badge-ghost badge-sm">{deviceTypes.find((type) => type.id === device.device_type)?.type_name || "Unknown"}</span>
                                </td>
                                <td>
                                    {deviceTypes.find((type) => type.id === device.device_type)?.type_name || "Unknown"}
                                </td>
                                <td>
                                    <img 
                                        src={ `https://localhost:7093/api/Image/${device.id}`} 
                                        alt="Device" 
                                        className="w-12 h-12 rounded"
                                        onClick={() => setSelectedImage(`https://localhost:7093/api/Image/${device.id}`)}
                                     />
                                </td>
                                <td>{device.qty}</td>
                                <td>{device.available_qty}</td>
                                <td>{new Date(device.last_ordered).toLocaleDateString()}</td>
                                <td>
                                <button
                                    className="btn btn-ghost btn-xs ml-2"
                                    onClick={() => setSelectedImage(`https://localhost:7093/api/Image/${device.id}`)}
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
            {selectedImage && (
                <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
                <div className="bg-white p-6 rounded-lg shadow-lg relative">
                    <button
                    className="absolute top-2 right-2 text-gray-600 hover:text-gray-900"
                    onClick={() => setSelectedImage(null)}
                    >
                    ✖
                    </button>
                    <img src={selectedImage} alt="Full Device" className="max-w-full max-h-[80vh]" />
                </div>
                </div>
            )}
        </div>
    );
};

export default DeviceOverviewManagement;
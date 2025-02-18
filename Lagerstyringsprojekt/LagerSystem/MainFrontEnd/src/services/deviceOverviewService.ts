const API_BASE_URL = "https://localhost:7093/api/";

// Fetch all device overviews
export const fetchDeviceOverviews = async () => {
    // API call to fetch all device overviews
    const response = await fetch(`${API_BASE_URL}DeviceOverview`);
    if (!response.ok) throw new Error("Failed to fetch device overviews");
    return response.json();
};

// Fetch a single device overview by ID
export const fetchDeviceOverviewById = async (id: number) => {
    const response = await fetch(`${API_BASE_URL}DeviceOverview/${id}`);
    if (!response.ok) throw new Error(`Failed to fetch device overview ${id}`);
    return response.json();
};

// fetch all device types
export const fetchDeviceTypes = async () => {
    const response = await fetch(`${API_BASE_URL}DeviceType/GetAllDeviceTypes`);
    if (!response.ok) throw new Error("Failed to fetch device types");
    return response.json();
};
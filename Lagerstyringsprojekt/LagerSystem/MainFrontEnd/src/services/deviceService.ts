const API_BASE_URL = "https://localhost:7093/api/";

// fetch all devices
export const fetchDevices = async () =>{
    const response = await fetch(`${API_BASE_URL}Device`);
    console.log(`${API_BASE_URL}Device`);
    if(!response.ok) throw new Error("Failed to fetch devices");
    return response.json();
}

// Fetch a single device by ID
export const fetchDeviceById = async (id: number) => {
    const response = await fetch(`${API_BASE_URL}Device/${id}`);
    if (!response.ok) throw new Error(`Failed to fetch device ${id}`);
    return response.json();
};

// Fetch all status types
export const fetchStatusTypes = async () => {
    const response = await fetch(`${API_BASE_URL}StatusType/GetAllStatusTypes`);
    if (!response.ok) throw new Error("Failed to fetch status types");
    return response.json();
  };
  
  // Fetch all rooms
  export const fetchRooms = async () => {
    const response = await fetch(`${API_BASE_URL}Location/GetAllRooms`);
    if (!response.ok) throw new Error("Failed to fetch rooms");
    return response.json();
  };
  
  // Fetch all cupboards
  export const fetchCupboards = async () => {
    const response = await fetch(`${API_BASE_URL}Location/GetAllCupboards`);
    if (!response.ok) throw new Error("Failed to fetch cupboards");
    return response.json();
  };
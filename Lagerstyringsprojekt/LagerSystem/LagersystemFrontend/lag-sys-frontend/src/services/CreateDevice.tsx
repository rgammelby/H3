import React, { useState } from "react";
import DeviceOverviewSelect from "./DeviceOverviewSelect.tsx";
import GetAllStatusTypes from "./GetAllStatusTypes";
import GetAllLocations from "./GetAllLocations";

interface FormData {
  "device_overview_id": number;
  "description": string;
  "status": number;
  "location": number;
  "qr" : string;
}

const CreateDevice: React.FC = () => {
  const [formData, setFormData] = useState<FormData>({
    "device_overview_id": 0,
    "description": "",
    "status": 1, // Default value
    "location": 0,
    "qr": "1234", // Default QR value sent to the backend
  });
  

  const [response, setResponse] = useState<string | null>(null);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = event.target;
  
    // Update formData with the new value
    setFormData((prevFormData) => ({
      ...prevFormData,
      [name]: value,
    }));
  
    console.log(`Input/Select value for ${name}:`, value);
  };
  

  const createDevice = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await fetch("http://localhost:7093/api/Device/AddDevice", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(formData),
      });

      if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

      const result = await response.text();
      setResponse(result ? `Device with ID ${JSON.parse(result).id} has been successfully created.` : "Device created successfully.");
    } catch (error: unknown) {
      setResponse(`Error: ${(error as Error).message}`);
    }
  };

  return (
    <div style={{ display: "flex", flexDirection: "column", alignItems: "center", marginTop: "10%" }}>
      <form onSubmit={createDevice} style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
        <DeviceOverviewSelect value={formData["device_overview_id"]} onChange={handleChange} />
        <input
          type="text"
          name="description"
          value={formData["description"]}
          onChange={handleChange}
          placeholder="Description"
          style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
          required
        />
        <GetAllStatusTypes value={formData["status"]} onChange={handleChange} />
        <GetAllLocations value={formData["location"]} onChange={handleChange} />
        <button type="submit" style={{ width: "200px", padding: "10px", backgroundColor: "#007bff", color: "white", border: "none", borderRadius: "5px", cursor: "pointer" }}>
          Create Device
        </button>
      </form>
      <div style={{ marginTop: "20px", width: "50%", textAlign: "center" }}>
        {response && <p>{response}</p>}
      </div>
    </div>
  );
};

export default CreateDevice;

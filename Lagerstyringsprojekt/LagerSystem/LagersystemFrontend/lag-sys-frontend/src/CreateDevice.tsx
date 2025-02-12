import React, { useState } from "react";
import DeviceOverviewSelect from "./DeviceOverviewSelect";  // Import the new component

interface FormData {
  "device_overview_id": number;
  "is_archived": boolean;
  "description": string;
  "status": number;
  "location": number;
  "qr": string;
}

const CreateDevice: React.FC = () => {
  const [formData, setFormData] = useState<FormData>({
    "device_overview_id": 0,
    "is_archived": false,
    "description": '',
    "status": 1, // Always set to 1
    "location": 0,
    "qr": '',
  });
  const [response, setResponse] = useState<string | null>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value,
    });
  };

  const createDevice = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      console.log("Sending request with data:", formData);

      const response = await fetch("https://localhost:7093/api/Device/AddDevice", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(formData),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      const result = await response.text(); // Use .text() to handle empty responses
      console.log("Response received:", result);

      if (result) {
        try {
          const jsonResult = JSON.parse(result);
          setResponse(JSON.stringify(jsonResult, null, 2));
        } catch (e) {
          setResponse(`Error parsing JSON: ${e.message}`);
        }
      } else {
        setResponse("Success: Device created, but no response data received.");
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error("Error creating device:", error);
        setResponse(`Error: ${error.message}`);
      }
    }
  };

  return (
    <div style={{ display: "flex", flexDirection: "column", alignItems: "center", marginTop: "10%" }}>
      <form onSubmit={createDevice} style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
        <DeviceOverviewSelect
          value={formData["device_overview_id"]}
          onChange={handleChange}
        />
        <label style={{ marginBottom: "10px" }}>
          <input
            type="checkbox"
            name="is_archived"
            checked={formData["is_archived"]}
            onChange={handleChange}
            style={{ marginRight: "5px" }}
          />
          Is Archived
        </label>
        <input
          type="text"
          name="description"
          value={formData["description"]}
          onChange={handleChange}
          placeholder="Description"
          style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
          required
        />
        <select
          name="location"
          value={formData["location"]}
          onChange={handleChange}
          style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
          required
        >
          <option value="" disabled>Select Location</option>
          <option value={1}>D.15</option>
          <option value={2}>D.16</option>
          <option value={3}>D.17</option>
        </select>
        <input
          type="text"
          name="qr"
          value={formData["qr"]}
          onChange={handleChange}
          placeholder="QR Code"
          style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
          required
        />
        <button
          type="submit"
          style={{ width: "200px", padding: "10px", backgroundColor: "#007bff", color: "white", border: "none", borderRadius: "5px", cursor: "pointer" }}
        >
          Create Device
        </button>
      </form>
      <div style={{ marginTop: "20px", width: "50%", textAlign: "center", whiteSpace: "pre-wrap", wordWrap: "break-word" }}>
        {response ? <p>Device with ID {JSON.parse(response).id} has been successfully created.</p> : <p>Fill out the form and click "Create Device"</p>}
      </div>
    </div>
  );
};

export default CreateDevice;

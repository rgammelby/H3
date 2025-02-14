import React, { useState, useEffect } from "react";

// Define the shape of the DeviceOverviewDTO (you can modify as needed)
interface DeviceOverviewDTO {
  id: number;
  model: string;
  device_type: number;
  image: string;
  qty: number;
  available_qty: number;
  last_ordered: string;
}

interface DeviceOverviewSelectProps {
  onChange: (event: React.ChangeEvent<HTMLSelectElement>) => void;
  value: number;
}

const DeviceOverviewSelect: React.FC<DeviceOverviewSelectProps> = ({ onChange, value }) => {
  const [deviceOverviews, setDeviceOverviews] = useState<DeviceOverviewDTO[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  // Fetch device overviews from the API
  useEffect(() => {
    const fetchDeviceOverviews = async () => {
      try {
        const response = await fetch("https://localhost:7093/api/DeviceOverview");
        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const data: DeviceOverviewDTO[] = await response.json();
        setDeviceOverviews(data);
        setLoading(false);
      } catch (error) {
        setError(error instanceof Error ? error.message : "An unknown error occurred");
        setLoading(false);
      }
    };

    fetchDeviceOverviews();
  }, []);

  if (loading) {
    return <p>Loading Device Types...</p>;
  }

  if (error) {
    return <p>Error: {error}</p>;
  }

  return (
    <select
      name="device_overview_id"
      value={value}
      onChange={onChange}
      style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
      required
    >
      <option value="" disabled>Select Device Type</option>
      {deviceOverviews.map((deviceOverview) => (
        <option key={deviceOverview.id} value={deviceOverview.id}>
          {deviceOverview.model}
        </option>
      ))}
    </select>
  );
};

export default DeviceOverviewSelect;

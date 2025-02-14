import React, { useEffect, useState } from "react";

interface Cupboard {
  id: number;
  designation: string;
  room_id: number;
}

interface Room {
  id: number;
  designation: string;
}

interface LocationSelectProps {
  value: number;
  onChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
}

const GetAllLocations: React.FC<LocationSelectProps> = ({ value, onChange }) => {
  const [locations, setLocations] = useState<{ id: number; label: string }[]>([]);

  useEffect(() => {
    const fetchLocations = async () => {
      try {
        const cupboardResponse = await fetch("https://localhost:7093/GetAllCupboards");
        const roomResponse = await fetch("https://localhost:7093/GetAllRooms");

        if (!cupboardResponse.ok || !roomResponse.ok) throw new Error("Failed to fetch cupboards or rooms");

        const cupboards: Cupboard[] = await cupboardResponse.json();
        const rooms: Room[] = await roomResponse.json();

        // Match cupboards to their respective rooms
        const combinedLocations = cupboards.map((cupboard) => {
          const room = rooms.find((r) => r.id === cupboard.room_id);
          const roomDesignation = room ? room.designation : "Unknown Room";

          return {
            id: cupboard.id,
            label: `${cupboard.designation} - ${roomDesignation}`,
          };
        });

        setLocations(combinedLocations);
      } catch (error) {
        console.error("Error fetching locations:", error);
      }
    };

    fetchLocations();
  }, []);

  return (
    <select
      name="location"
      value={value}
      onChange={onChange}
      style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
      required
    >
      <option value="" disabled>Select Location</option>
      {locations.map((location) => (
        <option key={location.id} value={location.id}>
          {location.label}
        </option>
      ))}
    </select>
  );
};

export default GetAllLocations;

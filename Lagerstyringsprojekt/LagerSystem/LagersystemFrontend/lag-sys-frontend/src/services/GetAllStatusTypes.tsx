import React, { useEffect, useState } from "react";

interface StatusType {
  id: number;
  status_type: string;
}

interface StatusTypeSelectProps {
  value: number;
  onChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
}

const GetAllStatusTypes: React.FC<StatusTypeSelectProps> = ({ value, onChange }) => {
  const [statusTypes, setStatusTypes] = useState<StatusType[]>([]);

  useEffect(() => {
    const fetchStatusTypes = async () => {
      try {
        const response = await fetch("http://localhost:7093/GetAllStatusTypes");
        if (!response.ok) throw new Error("Failed to fetch status types");

        const data: StatusType[] = await response.json();
        setStatusTypes(data);
      } catch (error) {
        console.error("Error fetching status types:", error);
      }
    };

    fetchStatusTypes();
  }, []);

  return (
    <select
      name="status"
      value={value}
      onChange={onChange}
      style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
      required
    >
      <option value="" disabled>Select Status</option>
      {statusTypes.map((status) => (
        <option key={status.id} value={status.id}>
          {status.status_type}
        </option>
      ))}
    </select>
  );
};

export default GetAllStatusTypes;

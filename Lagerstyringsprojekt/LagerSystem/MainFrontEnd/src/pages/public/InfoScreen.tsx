import React from "react";
import useDevices from "../../hooks/useDevices";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { Card, CardContent } from "../../components/ui/Card";  

const InfoScreen = () => {
  const { devices, deviceOverviews, deviceTypes, statusTypes, rooms, cupboards, loading, error } = useDevices();
  const [selectedDate, setSelectedDate] = React.useState(new Date());
  
  
  if (loading) return <p>Loading...</p>;
  if (error) return <p className="text-red-500">Error: {error}</p>;
    
  return (
    <div className="p-6 w-full max-w-6xl mx-auto">
      {/* Header */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">LagerSystem</h1>
        <div>
        <button className="btn btn-outline mr-2">Login</button>
        <button className="btn btn-outline">Register</button>
        </div>
      </div>
      
      {/* Calendar Selector */}
      <Card className="mb-6">
        <CardContent className="p-4">
          <h2 className="text-lg font-bold mb-4">Select a Date</h2>
          <Calendar onChange={setSelectedDate} value={selectedDate} />
        </CardContent>
      </Card>
      
      {/* Availability Table using DaisyUI */}
<Card>
  <CardContent className="overflow-x-auto p-4">
    <div className="overflow-x-auto shadow-lg rounded-lg">
      <table className="table w-full table-zebra">
        <thead className="bg-base-200">
          <tr>
            <th>#</th>
            <th>Device ID</th>
            <th>Model</th>
            <th>Device Type</th>
            <th>Status</th>
            <th>Availability</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {devices.map((device, index) => {
            const overview = deviceOverviews.find(o => o.id === device.device_overview_id);
            const deviceType = deviceTypes.find(t => t.id === overview?.device_type);
            const status = statusTypes.find(s => s.id === device.status);
            const cupboard = cupboards.find(c => c.id === device.location);
            const room = rooms.find(r => r.id === cupboard?.room_id);
            const isAvailable = status?.status_type === "Available";
            const isBorrowed = status?.status_type === "Borrowed";
            const isOverdue = status?.status_type === "Overdue";

            return (
              <tr key={device.id} className="hover:bg-base-300">
                <th>{index + 1}</th>
                <td>{device.id}</td>
                <td className="font-semibold flex items-center gap-3">
                  {overview && (
                    <div className="flex flex-col">
                      <span>{overview.model}</span>
                      <span className="badge badge-ghost badge-sm border-amber-50">
                        {deviceType?.type_name || "Unknown"}
                      </span>
                    </div>
                  )}
                </td>
                <td>
                  <span className="badge badge-info">{deviceType?.type_name || "Unknown"}</span>
                </td>
                <td>
                  <span className="badge badge-outline">{status?.status_type || "Unknown"}</span>
                </td>
                <td>
                  {isAvailable && <span className="text-green-600 font-bold">🟢 Available</span>}
                  {isBorrowed && <span className="text-yellow-500 font-bold">🟡 Borrowed</span>}
                  {isOverdue && <span className="text-red-500 font-bold">🔴 Overdue</span>}
                </td>
                <td>
                  {isAvailable ? (
                    <button className="btn btn-sm btn-primary">Borrow</button>
                  ) : (
                    <button className="btn btn-sm btn-secondary" disabled>Not Available</button>
                  )}
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  </CardContent>
</Card>

    </div>
  );
}


export default InfoScreen;
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
      
      {/* Availability Table */}
      <Card>
        <CardContent className="overflow-x-auto p-4">
          <table className="w-full border-collapse border border-gray-300">
            <thead>
              <tr className="bg-gray-200">
                <th className="border p-2">Device ID</th>
                <th className="border p-2">Model</th>
                <th className="border p-2">Device Type</th>
                <th className="border p-2">Status</th>
                <th className="border p-2">Availability</th>
                <th className="border p-2">Action</th>
              </tr>
            </thead>
            <tbody>
              {devices.map(device => {
                const overview = deviceOverviews.find(o => o.id === device.device_overview_id);
                const deviceType = deviceTypes.find(t => t.id === overview?.device_type);
                const status = statusTypes.find(s => s.id === device.status);
                const cupboard = cupboards.find(c => c.id === device.location);
                const room = rooms.find(r => r.id === cupboard?.room_id);
                const isAvailable = status?.status_type === "Available";
                const isBorrowed = status?.status_type === "Borrowed";
                const isOverdue = status?.status_type === "Overdue";

                return (
                  <tr key={device.id} className="text-center">
                    <td className="border p-2">{device.id}</td>
                    <td className="border p-2">{overview?.model || "Unknown"}</td>
                    <td className="border p-2">{deviceType?.type_name || "Unknown"}</td>
                    <td className="border p-2">
                      <span className="badge badge-outline">{status?.status_type || "Unknown"}</span>
                    </td>
                    <td className="border p-2">
                    {isAvailable && <span className="text-green-600 font-bold">🟢 Available</span>}
                    {isBorrowed && <span className="text-yellow-500 font-bold">🟡 Borrowed</span>}
                    {isOverdue && <span className="text-red-500 font-bold">🔴 Overdue</span>}
                    </td>
                    <td className="border p-2">
                      {isAvailable ? (
                         <button className="btn btn-primary">Borrow</button>
                      ) : (
                        <button className="btn btn-secondary" disabled>Not Available</button>
                      )}
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </CardContent>
      </Card>
    </div>
  );
}


export default InfoScreen;
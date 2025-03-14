import React, { useState, useEffect } from "react";
import { Device, StatusType, Location, DeviceOverview } from "../../API/ApiInstances";
import { IRoom, ICupboards } from "../../Interfaces/Location";
import { IStatusTypes } from "../../Interfaces/StatusType";
import { IAllDeviceOverview } from "../../Interfaces/DeviceOverview";
import { IDevice, IDeviceForm } from "../../Interfaces/Device";
import { LucideInstagram } from "lucide-react";

interface props {
  cancelModal: (e: boolean) => void;
}

const AddDeviceModal: React.FC<props> = ({ cancelModal }) => {
  const [ROOMS, setRooms] = useState<IRoom[]>([]);
  const [CUPBOARDS, setCupboards] = useState<ICupboards[]>([]);
  const [STATUSTYPE, setStatusType] = useState<IStatusTypes[]>([]);
  const [DEVICEOVERVIEW, setDeviceOverview] = useState<IAllDeviceOverview[]>([]);
  const [DESCRIPTION, setDescription] = useState<string>("");
  const [ISARCHIVED, setIsArchived] = useState<boolean>(false);
  const [DEVICEOVERVIEWID, setDeviceOverviewId] = useState<number>(0);
  const [SELECTEDLOCATION, setSelectedLocation] = useState<number>(0);
  const [SELECTEDTYPE, setSelectedType] = useState<number>(0);
  const [ISDROPDOWNOPEN, setIsDropdownOpen] = useState(false);

  // Adds a new device
  const addDevice = async() => {
    try{
        const body: IDeviceForm = {
            status: SELECTEDTYPE,
            location: SELECTEDLOCATION,
            description: DESCRIPTION,
            is_archived: ISARCHIVED,
            device_overview_id: DEVICEOVERVIEWID,
            qr: "temporary",
        }

        const response: IDevice = await Device.addDevice(body);
    }
    catch(ex){
        console.error(ex);
    }
  }

  // On-mount gets all necessaries data from api
  useEffect(() => {
    const fetchAll = async () => {
      const cupboardsResponse = await Location.fetchAllCupboards();
      const roomResponse = await Location.fetchAllRooms();
      const statusResponse = await StatusType.fetchAllStatusTypes();
      const deviceOverviewResponse = await DeviceOverview.fetchAllDeviceOverviews();

      setRooms(roomResponse);
      setCupboards(cupboardsResponse);
      setStatusType(statusResponse);
      setDeviceOverview(deviceOverviewResponse);
    };

    fetchAll();
  }, []);

  return (
    <div
      style={{ backgroundColor: "rgba(0, 0, 0, 0.5)" }}
      className="fixed flex inset-0 z-999 w-screen h-screen justify-center items-center"
      onClick={() => cancelModal(false)}
    >
      <div
        className="w-100 h-auto bg-white rounded-sm flex flex-col justify-center items-center p-6"
        onClick={(e) => e.stopPropagation()}
      >
        {/* Description input */}
        <p>Beskrivelse</p>
        <input
          className="w-64 p-2 border rounded mb-5"
          type="text"
          onChange={(e) => setDescription(e.target.value)}
        />

        {/* DeviceOverview dropdown */}
        <p>Enheds type</p>
        <select
        className="w-64 p-2 border rounded mb-5"
        onChange={(e) => setDeviceOverviewId(parseInt(e.target.value))}
        >
          <option value={0}>None</option>
          {DEVICEOVERVIEW &&
            DEVICEOVERVIEW.map((overview, index) => <option value={overview.id} key={index}>{overview.model}</option>)}
        </select>

        {/* Location Dropdown */}
        <p>Lokation</p>
        <div className="relative w-64 mb-5">
          {/* Dropdown Button */}
          <div
            className="w-full p-2 border rounded cursor-pointer bg-white"
            onClick={() => setIsDropdownOpen(!ISDROPDOWNOPEN)}
          >
            {SELECTEDLOCATION === 0
              ? "None"
              : `${CUPBOARDS.find((cupboard) => cupboard.id === SELECTEDLOCATION)?.designation} - ${
                  ROOMS.find(
                    (room) =>
                      room.id ===
                      CUPBOARDS.find((cupboard) => cupboard.id === SELECTEDLOCATION)?.room_id
                  )?.designation
                }`}{" "}
            <p style={{ float: "right" }}>⏷</p>
          </div>

          {/* Dropdown List */}
          {ISDROPDOWNOPEN && (
            <div className="absolute w-full bg-white border rounded mt-1 shadow-md max-h-40 overflow-y-auto z-10">
              {/* "None" Option */}
              <div
                className="p-2 hover:bg-gray-200 cursor-pointer"
                onClick={() => {
                  setSelectedLocation(0);
                  setIsDropdownOpen(false);
                }}
              >
                None
              </div>

              {/* Dynamic Cupboards List */}
              {CUPBOARDS.map((cupboard) => {
                const roomDesignation =
                  ROOMS.find((room) => room.id === cupboard.room_id)?.designation || "Unknown";
                return (
                  <div
                    key={cupboard.id}
                    className="p-2 hover:bg-gray-200 cursor-pointer"
                    onClick={() => {
                      setSelectedLocation(cupboard.id);
                      setIsDropdownOpen(false);
                    }}
                  >
                    {cupboard.designation} - {roomDesignation}
                  </div>
                );
              })}
            </div>
          )}
        </div>

        {/* Status dropdown */}
        <p>Status</p>
        <select
          className="w-64 p-2 border rounded mb-5"
          onChange={(e) => setSelectedType(parseInt(e.target.value))}
        >
          <option value="">None</option>
          {STATUSTYPE &&
            STATUSTYPE.map((status, index) => <option value={status.id} key={index}>{status.status_type}</option>)}
        </select>

        {/* Is Archived input */}
        <p>Er Deaktiveret?</p>
        <input
          type="checkbox"
          className="w-8 h-8 accent-green-600 border-2 border-gray-400 rounded-lg cursor-pointer mb-5"
          onChange={(e) => e.target.checked ? setIsArchived(true) : setIsArchived(false)}
        />
        
        {/* Button for adding */}
        <button className="btn btn-outline btn-success" onClick={() => addDevice()}>Tilføj</button>
      </div>
    </div>
  );
};

export default AddDeviceModal;

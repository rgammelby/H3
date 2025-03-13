import React, { useEffect, useState } from "react";
import { Location, Device, StatusType } from "../../API/ApiInstances";
import { ICupboards, IRoom } from "../../Interfaces/Location";
import { IStatusTypes } from "../../Interfaces/StatusType";

interface props {
  cancelModal: (e: boolean) => void;
}

const UpdateDevice: React.FC<props> = ({ cancelModal }) => {
  const [ROOMS, setRooms] = useState<IRoom[]>([]);
  const [CUPBOARDS, setCupboards] = useState<ICupboards[]>([]);
  const [STATUSTYPE, setStatusType] = useState<IStatusTypes[]>([]);
  const [SELECTEDLOCATION, setSelectedLocation] = useState<number>(0);
  const [ISDROPDOWNOPEN, setIsDropdownOpen] = useState(false);

  const updateDevice = async() => {
    try{

    }
    catch (e){
      console.error(e);
    }
  }

  useEffect(() => {
    const fetchAll = async () => {
      const allCupboards = await Location.fetchAllCupboards();
      const allRooms = await Location.fetchAllRooms();
      const allType = await StatusType.fetchAllStatusTypes();

      setCupboards(allCupboards);
      setRooms(allRooms);
      setStatusType(allType);
    };

    fetchAll();
  }, []);

  return (
    <div
      style={{ backgroundColor: "rgba(0, 0, 0, 0.5)" }}
      className="w-screen h-screen z-999 flex fixed inset-0 items-center justify-center"
      onClick={() => cancelModal(true)}
    >
      <div
        className="w-100 h-auto bg-white rounded-sm flex flex-col justify-center items-center p-6"
        onClick={(e) => e.stopPropagation()}
      >
        <p>Beskrivelse</p>
        <input type="text" className="w-64 p-2 border rounded mb-5" />
        <p>Lokation</p>

        {/* Custom Dropdown */}
        <div className="relative w-64">
          {/* Dropdown Button */}
          <div
            className="w-full p-2 border rounded cursor-pointer bg-white"
            onClick={() => setIsDropdownOpen(!ISDROPDOWNOPEN)}
          >
            {SELECTEDLOCATION === 0 ? "None" : `${CUPBOARDS.find((cupboard) => cupboard.id === SELECTEDLOCATION)?.designation} - ${ROOMS.find((room) => room.id === (CUPBOARDS.find((cupboard) => cupboard.id === SELECTEDLOCATION)?.room_id))?.designation}`} <p style={{float: "right"}}>⏷</p>
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
        <p>Status</p>
        <select className="w-64 p-2 border rounded mb-5">
          <option value="">None</option>
          {
            STATUSTYPE && (
              STATUSTYPE.map((status) => (
                <option value={status.id}>{status.status_type}</option>
              ))
            )
          }
        </select>
        <button
        className="btn btn-medium btn-smooth btn-primary"
        >
          Opdater
        </button>
      </div>
    </div>
  );
};

export default UpdateDevice;

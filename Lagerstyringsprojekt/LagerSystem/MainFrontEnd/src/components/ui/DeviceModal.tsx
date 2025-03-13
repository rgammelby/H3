import React, { useEffect, useState } from "react";
import { Image, Location, StatusType } from "../../API/ApiInstances";
import { IDevice } from "../../Interfaces/Device";
import { ICupboards } from "../../Interfaces/Location";
import { IRoom } from "../../Interfaces/Location";
import { IStatusTypes } from "../../Interfaces/StatusType";

interface props {
  cancelModal: (e: boolean) => void;
  device: IDevice;
}

const DeviceModal: React.FC<props> = ({ cancelModal, device }) => {
  const [ROOMS, setRooms] = useState<IRoom[]>([]);
  const [CUPBOARDS, setCupboards] = useState<ICupboards[]>([]);
  const [STATUS, setStatus] = useState<IStatusTypes[]>([]);

  useEffect(() => {
    async function fetchData() {
      const fetchedCupboards = await Location.fetchAllCupboards();
      const fetchedRooms = await Location.fetchAllRooms();
      const status = await StatusType.fetchAllStatusTypes();
      setCupboards(fetchedCupboards);
      setRooms(fetchedRooms);
      setStatus(status);
    }
    fetchData();
  }, []);

  // Find the cupboard matching the device's location_id
  const SELECTEDCUPBOARD = CUPBOARDS.find((cupboard) => cupboard.id === device.location);

  // If the cupboard exists, find the room that matches the cupboard's room_id
  const SELECTEDROOM = SELECTEDCUPBOARD
    ? ROOMS.find((room) => room.id === SELECTEDCUPBOARD.room_id)
    : null;

  const SELECTEDSTATUS = STATUS.find((type) => type.id === device.status);

  return (
    <div
      onClick={() => cancelModal(true)}
      className="w-screen h-screen z-999 flex fixed inset-0 items-center justify-center"
      style={{ backgroundColor: "rgba(0, 0, 0, 0.5)" }}
    >
      <div
        className="w-96 h-auto bg-white rounded-sm flex flex-col justify-center items-center p-6"
        onClick={(e) => e.stopPropagation()}
      >
        <img src={Image(device.device_overview_id)} alt="Device overview" />
        <p>
          {SELECTEDCUPBOARD?.designation} - {SELECTEDROOM?.designation}
        </p>
        <p>{device.description}</p>
        <p>{SELECTEDSTATUS?.status_type}</p>
        <p>{device.is_archived ? <>Available</> : <>Not Available</>}</p>
        <p>{device.qr}</p>
      </div>
    </div>
  );
};

export default DeviceModal;

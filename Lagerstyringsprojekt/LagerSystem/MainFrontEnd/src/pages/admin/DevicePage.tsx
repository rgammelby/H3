import React, { useState, useEffect } from "react";
import {
  Device,
  DeviceOverview,
  DeviceType,
  Location,
  StatusType,
  Image,
} from "../../API/ApiInstances";
import { IDevice } from "../../Interfaces/Device";
import { IAllDeviceOverview } from "../../Interfaces/DeviceOverview";
import { IDeviceTypes } from "../../Interfaces/DeviceTypes";
import { IStatusTypes } from "../../Interfaces/StatusType";
import { ICupboards } from "../../Interfaces/Location";
import { IRoom } from "../../Interfaces/Location";
import DeviceModal from "../../components/ui/DeviceModal";
import UpdateDevice from "../../components/ui/UpdateDevice";
import AddDeviceModal from "../../components/ui/AddDeviceModal";

function DevicePage() {
  const [DEVICES, setDevices] = useState<IDevice[]>([]);
  const [OVERVIEWS, setOverviews] = useState<IAllDeviceOverview[]>([]);
  const [DEVICETYPES, setDeviceTypes] = useState<IDeviceTypes[]>([]);
  const [STATUSTYPES, setStatusTypes] = useState<IStatusTypes[]>([]);
  const [ROOMS, setRooms] = useState<IRoom[]>([]);
  const [CUPBOARDS, setCupboards] = useState<ICupboards[]>([]);
  const [ADDDEVICEMODAL, setAddDeviceModal] = useState<boolean>(false);
  const [DEVICEMODAL, setDeviceModal] = useState<boolean>(false);
  const [UPDATEDEVICEMODAL, setUpdateDeviceModal] = useState<boolean>(false);
  const [SINGLEDEVICE, setSingleDevice] = useState<IDevice>();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const DEVICEDATA: IDevice[] = await Device.fetchAllDevices();
        const OVERVIEWDATA: IAllDeviceOverview[] = await DeviceOverview.fetchAllDeviceOverviews();
        const TYPEDATA: IDeviceTypes[] = await DeviceType.fetchDeviceTypes();
        const STATUSTYPEDATA: IStatusTypes[] = await StatusType.fetchAllStatusTypes();
        const ROOMDATA: IRoom[] = await Location.fetchAllRooms();
        const CUPBOARDDATA: ICupboards[] = await Location.fetchAllCupboards();

        setDevices(DEVICEDATA);
        setOverviews(OVERVIEWDATA);
        setDeviceTypes(TYPEDATA);
        setStatusTypes(STATUSTYPEDATA);
        setRooms(ROOMDATA);
        setCupboards(CUPBOARDDATA);
      } catch (ex) {
        console.error(ex);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="p-6">
      {/* Sticky Header Section */}
      <div className="sticky top-16 bg-[#f0e0c0] p-4 z-50 shadow-md flex items-center justify-between">
        <button className="btn btn-outline btn-success mb-2" onClick={() => setAddDeviceModal(true)}>Tilføj</button>
        <h1 className="text-3xl font-bold text-brown-700 flex-1 text-center">💻 Device Management</h1>
      </div>

      {/* Normal Scrollable Content */}
      <div className="mt-4">
        <table className="table w-full table-zebra">
          <thead className="bg-base-200">
            <tr className="sticky top-36 bg-[#f3ddba]">
              <th>#</th>
              <th>EnhedsID</th>
              <th>Billed</th>
              <th>Model</th>
              <th>Enheds Type</th>
              <th>Status</th>
              <th>Lokation</th>
              <th>Aktion</th>
            </tr>
          </thead>

          <tbody>
            {DEVICES.map((device, index) => {
              const overview = OVERVIEWS.find((o) => o.id === device.device_overview_id);
              const deviceType = DEVICETYPES.find((t) => t.id === overview?.device_type);
              const status = STATUSTYPES.find((s) => s.id === device.status);
              const cupboard = CUPBOARDS.find((c) => c.id === device.location);
              const room = ROOMS.find((r) => r.id === cupboard?.room_id);

              return (
                <tr
                  onClick={() => {
                    setDeviceModal(true);
                    setSingleDevice(device);
                  }}
                  key={device.id}
                  className="hover:bg-base-300 cursor-pointer"
                >
                  <th>{index + 1}</th>
                  <td>{device.id}</td>
                  <td>
                    {overview?.id ? (
                      <img src={Image(overview.id)} className="w-25 h-auto" />
                    ) : (
                      <img
                        src="https://upload.wikimedia.org/wikipedia/commons/6/65/No-Image-Placeholder.svg"
                        className="w-25 h-auto"
                      />
                    )}
                  </td>
                  <td className="font-semibold flex items-center gap-3">
                    {overview && (
                      <div className="flex-flex-col">
                        <span>{overview.model}</span>
                        <br />
                        <span className="badge badge-ghost badge-sm">
                          {deviceType?.type_name || "Unknown"}
                        </span>
                      </div>
                    )}
                  </td>
                  <td>{deviceType?.type_name || "Unknown"}</td>
                  <td>
                    <span className="badge badge-outline">{status?.status_type || "Unknown"}</span>
                  </td>
                  <td>
                    {room?.designation || "Unknown"} - {cupboard?.designation || "Unknown"}
                  </td>
                  <td onClick={(e) => e.stopPropagation()}>
                    <button
                      className="btn btn-sm btn-primary"
                      onClick={() => {
                        setUpdateDeviceModal(true);
                        setSingleDevice(device);
                      }}
                    >
                      Update
                    </button>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>

      {/* Modals */}
      {DEVICEMODAL && SINGLEDEVICE && (
        <DeviceModal cancelModal={() => setDeviceModal(false)} device={SINGLEDEVICE} />
      )}
      {UPDATEDEVICEMODAL && SINGLEDEVICE && (
        <UpdateDevice
          cancelModal={() => setUpdateDeviceModal(false)}
          selectedDevice={SINGLEDEVICE}
        />
      )}
      {ADDDEVICEMODAL && (
        <AddDeviceModal cancelModal={() => setAddDeviceModal(false)} />
      )

      }
    </div>
  );
}

export default DevicePage;

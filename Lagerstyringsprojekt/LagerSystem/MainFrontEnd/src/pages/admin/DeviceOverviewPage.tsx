import React, { useEffect, useState } from "react";
import { DeviceOverview, DeviceType, Image } from "../../API/ApiInstances";
import { IAllDeviceOverview, IUpdateDeviceOverview } from "../../Interfaces/DeviceOverview";
import { IDeviceTypes } from "../../Interfaces/DeviceTypes";

function DeviceOverviewPage() {
  const [OVERVIEWDATA, setOverview] = useState<IAllDeviceOverview[]>([]);
  const [DEVICETYPES, setDeviceTypes] = useState<IDeviceTypes[]>([]);
  const [SELECTEDIMAGE, setSelectedImage] = useState<string | null>(null);
  const [SELECTEDOVERVIEW, setSelectedOverview] = useState<IAllDeviceOverview | null>(null);
  const [OVERVIEWUPDATE, setOverviewUpdate] = useState<IUpdateDeviceOverview | null>(null);
  const [INPUTDATA, setInputData] = useState<IUpdateDeviceOverview | null>(null);
  const [INPUTIMAGE, setInputImage] = useState<File | null>();

  const updateOverview = async () => {
    try {
      if (OVERVIEWUPDATE === null) return;

      const formdata = new FormData();
      formdata.append("model", OVERVIEWUPDATE.model);
      formdata.append("device_type", OVERVIEWUPDATE.device_type.toString());
      formdata.append("image", INPUTIMAGE, INPUTIMAGE?.name);
      formdata.append("qty", OVERVIEWUPDATE.qty.toString());
      formdata.append("last_ordered", OVERVIEWUPDATE.last_ordered);

      const reponse: IUpdateDeviceOverview = await DeviceOverview.updateDeviceOverview();
    } catch (ex) {
      console.error(ex);
    }
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setInputImage(file);
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const OVERVIEW: IAllDeviceOverview[] = await DeviceOverview.fetchAllDeviceOverviews();
        const TYPES: IDeviceTypes[] = await DeviceType.fetchDeviceTypes();

        setOverview(OVERVIEW);
        setDeviceTypes(TYPES);
      } catch (ex) {
        console.error(ex);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="p-6">
      <h1 className="text-3xl font-bold mb-4 text-center">📦 Device Overview</h1>

      <div className="overflow-x-auto shadow-lg rounded-lg">
        <table className="table w-full table-zebra">
          {/* Table Head */}
          <thead className="bg-base-200">
            <tr>
              <th>#</th>
              <th>Model</th>
              <th>Device Type</th>
              <th>Image</th>
              <th>Qty</th>
              <th>Available</th>
              <th>Last Ordered</th>
              <th>Actions</th>
            </tr>
          </thead>

          {/* Table Body */}

          <tbody>
            {OVERVIEWDATA.map((device, index) => (
              <tr key={device.id} className="hover:bg-base-300">
                <td>{index + 1}</td>
                <td className="font-semibold">
                  {device.model}
                  <br />
                  <span className="badge badge-ghost badge-sm">
                    {DEVICETYPES.find((type) => type.id === device.device_type)?.type_name ||
                      "Unknown"}
                  </span>
                </td>
                <td>
                  {DEVICETYPES.find((type) => type.id === device.device_type)?.type_name ||
                    "Unknown"}
                </td>
                <td>
                  <img
                    src={Image(device.id)}
                    alt="Device"
                    className="w-12 h-12 rounded"
                    onClick={() => setSelectedImage(Image(device.id))}
                  />
                </td>
                <td>{device.qty}</td>
                <td>{device.available_qty}</td>
                <td>{new Date(device.last_ordered).toLocaleDateString()}</td>
                <td>
                  <button
                    className="btn btn-ghost btn-xs ml-2"
                    onClick={() => setSelectedImage(Image(device.id))}
                  >
                    Show Details
                  </button>
                  <button
                    onClick={() => setSelectedOverview(device)}
                    className="btn btn-sm btn-primary"
                  >
                    Update
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Selected Image Modal */}
      {SELECTEDIMAGE && (
        <div
          onClick={() => setSelectedImage(null)}
          style={{ backgroundColor: "rgba(0, 0, 0, 0.5)" }}
          className="fixed inset-0 flex items-center justify-center"
        >
          <div
            onClick={(e) => e.stopPropagation()}
            className="bg-white p-6 rounded-lg shadow-lg relative"
          >
            <button
              className="absolute top-2 right-2 text-gray-600 hover:text-gray-900"
              onClick={() => setSelectedImage(null)}
            >
              ✖
            </button>
            <img src={SELECTEDIMAGE} alt="Full Device" className="max-w-full max-h-[80vh]" />
          </div>
        </div>
      )}
      {SELECTEDOVERVIEW && (
        <div
          style={{ backgroundColor: "rgba(0, 0, 0, 0.5)" }}
          className="w-screen h-screen z-999 flex fixed inset-0 items-center justify-center"
          onClick={() => setSelectedOverview(null)}
        >
          <div
            className="w-100 h-auto bg-white rounded-sm flex flex-col justify-center items-center p-6"
            onClick={(e) => e.stopPropagation()}
          >
            <h1 className="text-2xl font-medium text-center mb-5">Opdater Enheds overblik</h1>
            <div className="w-full flex flex-col items-center">
              <label className="block text-l font-medium text-gray-700 mb-2">Model:</label>
              <input
              type="text"
              className="w-64 p-2 border rounded mb-5"
              onChange={}
              />
              <label className="block text-l font-medium text-gray-700 mb-2">Enheds type:</label>
              <select className="w-64 p-2 border rounded mb-5">
                <option value="">None</option>
                {/* Add your options here */}
              </select>

              <label className="block text-l font-medium text-gray-700 mb-2">Billed:</label>
              <input type="file" className="w-64 p-2 border rounded mb-5" />

              <label className="block text-l font-medium text-gray-700 mb-2">Antal:</label>
              <input type="number" className="w-64 p-2 border rounded mb-5" />
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default DeviceOverviewPage;

import React, { useEffect, useState } from "react";
import { DeviceOverview, DeviceType, Image } from "../../API/ApiInstances";
import { IAllDeviceOverview, IUpdateDeviceOverview } from "../../Interfaces/DeviceOverview";
import { IDeviceTypes } from "../../Interfaces/DeviceTypes";
import SelectOverview from "../../components/ui/SelectOverview";
import AddDeviceOverviewModal from "../../components/ui/AddDeviceOverviewModal";

function DeviceOverviewPage() {
  const [OVERVIEWDATA, setOverview] = useState<IAllDeviceOverview[]>([]);
  const [DEVICETYPES, setDeviceTypes] = useState<IDeviceTypes[]>([]);
  const [SELECTEDIMAGE, setSelectedImage] = useState<string | null>(null);
  const [SELECTEDOVERVIEW, setSelectedOverview] = useState<IAllDeviceOverview | null>(null);
  const [AMOUNT, setAmount] = useState<number | null>();
  const [MODEL, setModel] = useState<string | null>();
  const [ADDMODAL, setAddModal] = useState<boolean>(false);
  const [INPUTIMAGE, setInputImage] = useState<File | null>();
  const [SELECTEDTYPE, setSelectedType] = useState<string | null>();

  const updateOverview = async () => {
    try {
      if (SELECTEDOVERVIEW == null) return;

      const formdata = new FormData();
      formdata.append("model", MODEL ? MODEL : "");
      formdata.append("device_type", SELECTEDTYPE ? SELECTEDTYPE : "");
      formdata.append("image", INPUTIMAGE ? INPUTIMAGE : "");
      formdata.append("qty", AMOUNT ? AMOUNT.toString() : "");
      formdata.append("last_ordered", "");

      for (const pair of formdata.entries()) {
        console.log(pair[0] + ": " + pair[1]);
      }

      const reponse: IUpdateDeviceOverview = await DeviceOverview.updateDeviceOverview(
        SELECTEDOVERVIEW.id,
        formdata
      );
    } catch (ex) {
      console.error(ex);
    }
  };

  const handleSelectChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const value = e.target.value;

    setSelectedType(value);
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
      <div className="sticky top-16 bg-[#f0e0c0] p-4 z-50 shadow-md flex items-center justify-between">
        <button
        className="btn btn-outline btn-success mb-2"
        onClick={() => setAddModal(true)}
        >
          Tilføj
        </button>
        <h1 className="text-3xl font-bold text-brown-700 flex-1 text-center">📦 Device Overview</h1>
      </div>
      <div className="mt-4">
      <table className="table w-full table-zebra">
          <thead className="bg-base-200">
            <tr className="sticky top-36 bg-[#f3ddba]">
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
                    className="w-20 h-auto rounded"
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
          className="fixed inset-0 z-999 flex items-center justify-center"
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

      {/* DeviceOverview Update */}
      {SELECTEDOVERVIEW && (
        <SelectOverview
          cancelModal={() => setSelectedOverview(null)}
          handleSelectChange={handleSelectChange}
          setAmount={setAmount}
          setModel={setModel}
          setImage={handleFileChange}
          updateOverview={updateOverview}
          deviceTypes={DEVICETYPES}
        />
      )}

      {/* DeviceOverview Add */}
      {ADDMODAL && (
        <AddDeviceOverviewModal cancelModal={() => setAddModal(false)} />
      )

      }
    </div>
  );
}

export default DeviceOverviewPage;

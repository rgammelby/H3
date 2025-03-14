import React, { useState, useEffect } from "react";
import { DeviceType, DeviceOverview } from "../../API/ApiInstances";
import { IDeviceTypes } from "../../Interfaces/DeviceTypes";
import { IAllDeviceOverview } from "../../Interfaces/DeviceOverview";

interface props {
  cancelModal: (e: boolean) => void;
}
const AddDeviceOverviewModal: React.FC<props> = ({ cancelModal }) => {
  const [DEVICETYPES, setDeviceTypes] = useState<IDeviceTypes[]>([]);
  const [SELECTEDIMAGE, setSelectedImage] = useState<File | null>();
  const [MODEL, setModel] = useState<string>("");
  const [SELECTEDDEVICETYPE, setSelectedDeviceType] = useState<number>(0);

  // Adds a new DeviceOverview
  const addDeviceOverview = async () => {
    try {
      if (SELECTEDDEVICETYPE === 0 || MODEL === "" || MODEL === null) {
        return;
      }

      const now = new Date();
      const formatted = now.toISOString().slice(0, 16); // "YYYY-MM-DDTHH:mm"

      const formdata: FormData = new FormData();
      formdata.append("model", MODEL);
      formdata.append("device_type", SELECTEDDEVICETYPE.toString());
      formdata.append("image", SELECTEDIMAGE ? SELECTEDIMAGE : "");
      formdata.append("last_ordered", formatted);

      const response: IAllDeviceOverview = await DeviceOverview.addDeviceOverview(formdata);
    } catch (ex) {
      console.error(ex);
    }
  };

  useEffect(() => {
    const fetchAll = async () => {
      const deviceTypeResponse = await DeviceType.fetchDeviceTypes();

      setDeviceTypes(deviceTypeResponse);
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
        {/* Model input */}
        <p>Model</p>
        <input
          className="w-64 p-2 border rounded mb-5"
          type="text"
          onChange={(e) => setModel(e.target.value)}
        />

        {/* DeviceType input */}
        <p>Enheds type</p>
        <select
          className="w-64 p-2 border rounded mb-5"
          onChange={(e) => setSelectedDeviceType(parseInt(e.target.value))}
        >
          <option value={0}>None</option>
          {DEVICETYPES &&
            DEVICETYPES.map((type, index) => (
              <option value={type.id} key={index}>
                {type.type_name}
              </option>
            ))}
        </select>

        {/* Image input */}
        <p>Billed</p>
        <input
          type="file"
          className="w-64 p-2 border rounded mb-5"
          onChange={(e) => setSelectedImage(e.target.files ? e.target.files[0] : null)}
        />

        <button className="btn btn-outline btn-success" onClick={() => addDeviceOverview()}>
          Tilføj
        </button>
      </div>
    </div>
  );
};

export default AddDeviceOverviewModal;

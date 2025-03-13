import React from "react";
import { IUpdateDeviceOverview } from "../../Interfaces/DeviceOverview";
import { IDeviceTypes } from "../../Interfaces/DeviceTypes";

interface SelectProps {
  cancelModal: (e: null) => void;
  handleSelectChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
  setAmount: (e: number) => void;
  setModel: (e: string) => void;
  setImage: (e: React.ChangeEvent<HTMLInputElement>) => void;
  updateOverview: () => void;
  deviceTypes: IDeviceTypes[];
}

const SelectOverview: React.FC<SelectProps> = ({
  cancelModal,
  handleSelectChange,
  setAmount,
  setModel,
  setImage,
  updateOverview,
  deviceTypes,
}) => {
  return (
    <div
      style={{ backgroundColor: "rgba(0, 0, 0, 0.5)" }}
      className="w-screen h-screen z-999 flex fixed inset-0 items-center justify-center"
      onClick={() => cancelModal(null)}
    >
      <div
        className="w-100 h-auto bg-white rounded-sm flex flex-col justify-center items-center p-6"
        onClick={(e) => e.stopPropagation()}
      >
        <h1 className="text-2xl font-medium text-center mb-5">Opdater Enheds overblik</h1>

        <div className="w-full flex flex-col items-center">
          {/* Model Input */}
          <label className="block text-l font-medium text-gray-700 mb-2">Model:</label>
          <input
            type="text"
            className="w-64 p-2 border rounded mb-5"
            onChange={(e) => setModel(e.target.value)}
          />

          {/* Select Dropdown */}
          <label className="block text-l font-medium text-gray-700 mb-2">Enheds type:</label>
          <select className="w-64 p-2 border rounded mb-5" onChange={handleSelectChange}>
            <option value="">None</option>
            {deviceTypes &&
              deviceTypes.map((type) => (
                <option key={type.id} value={type.id}>
                  {type.type_name}
                </option>
              ))}
          </select>

          {/* File Upload */}
          <label className="block text-l font-medium text-gray-700 mb-2">Billed:</label>
          <input
            type="file"
            className="file-input bg-white"
            onChange={(e) => setImage(e)}
          />

          {/* Number Input */}
          <label className="block text-l font-medium text-gray-700 mb-2">Antal:</label>
          <input
            type="number"
            className="w-64 p-2 border rounded mb-5"
            onChange={(e) => setAmount(Number(e.target.value))}
          />
        </div>
        <button
        type="button"
        className="btn btn-outline btn-medium btn-success"
        onClick={updateOverview}
        >
          Færdig
        </button>
      </div>
    </div>
  );
};

export default SelectOverview;

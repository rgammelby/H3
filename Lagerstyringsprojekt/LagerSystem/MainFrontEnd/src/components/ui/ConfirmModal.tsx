import React, { useState, useEffect } from "react";

function ConfirmModal({ confirmModal = (accept: boolean) => {} }) {
  return (
    <div
      style={{ backgroundColor: "rgba(0, 0, 0, 0.5)" }}
      className="fixed inset-0 flex items-center justify-center z-999"
    >
      <div className="bg-white p-6 rounded-lg shadow-lg w-84">
        <div className="flex justify-center">
          <h1 className="block text-xl font-medium text-gray-700">Er du sikker</h1>
        </div>
        <div className="flex justify-center">
        <button className="btn btn-medium btn-outline btn-success m-3" onClick={() => confirmModal(true)}>
          Accepter så
        </button>
        <button
          className="btn btn-medium btn-soft btn-error m-3"
          onClick={() => {
            confirmModal(false);
          }}
        >
          Annuller så
        </button>
        </div>
      </div>
    </div>
  );
}

export default ConfirmModal;

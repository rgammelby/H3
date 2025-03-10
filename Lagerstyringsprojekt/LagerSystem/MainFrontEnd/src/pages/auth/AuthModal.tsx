import React from "react";

export function AuthModal({
    isOpen,
    view,
    closeModal,
  }: {
    isOpen: boolean;
    view: "login" | "register" | null;
    closeModal: () => void;
  }) {
    if (!isOpen) return null;
  
    return (
      <div className="fixed inset-0 flex items-center justify-center bg-black/50 z-50" onClick={closeModal}>
        <div 
          className="bg-white p-6 rounded-lg shadow-lg w-96 z-51"
          onClick={(e) => e.stopPropagation()}
        >
          <h2 className="text-xl font-bold mb-4">{view === "login" ? "Log ind" : "Registrér"}</h2>
          <form>
            <input type="email" placeholder="Email" className="w-full mb-3 p-2 border rounded" />
            <input type="password" placeholder="Adgangskode" className="w-full mb-3 p-2 border rounded" />
            <button className="w-full bg-blue-500 text-white py-2 rounded">
              {view === "login" ? "Log ind" : "Registrér"}
            </button>
          </form>
        </div>
      </div>
    );
    
  }
  
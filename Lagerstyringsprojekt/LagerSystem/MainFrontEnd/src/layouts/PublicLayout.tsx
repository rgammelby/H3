import { Outlet } from "react-router-dom";
import Sidebar from "../components/layout/SideBar";
import { useState } from "react";

const PublicLayout = () => {
  return (
    <div className="flex flex-col min-h-screen">
      <div></div> 
      {/* Public pages will be displayed here */}
      
      <main className="flex-1 p-4">
        <Outlet /> {/* InfoScreen.tsx will load here */}
      </main>
    </div>
  );
};

export default PublicLayout;

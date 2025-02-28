import { Outlet } from "react-router-dom";
import Sidebar from "../components/layout/SideBar";
import { useState } from "react";

const MainLayout = ({isSidebarOpen = false}) => {

  return (
    <div className="flex flex-col min-h-screen">
     
      {/* Sidebar & Main Content Wrapper */}
      <div className="flex flex-1 pt-16">
      {/* Sidebar (Hidden by default) */}
        <div 
            className={`fixed top-16 left-0 h-full w-64 bg-white shadow-md transition-transform duration-300 ease-in-out ${
                isSidebarOpen ? "translate-x-0" : "-translate-x-64"
            } z-40`}
        >
            <Sidebar isOpen={isSidebarOpen}/>
        </div>

      {/* Main Content (Auto Fills Page when Sidebar is Hidden) */}
      <main 
          className={`flex-1 p-6 transition-all duration-300 ${
              isSidebarOpen ? "ml-64" : "ml-0"
          }`}
      >
          <Outlet />
      </main>
   </div>
    </div>
  );
};

export default MainLayout;

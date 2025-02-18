import { Outlet } from "react-router-dom";
import PageHeader from "../components/layout/PageHeader";
import Sidebar from "../components/layout/SideBar";
import { useState } from "react";

const PublicLayout = () => {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);
  return (
    <div className="flex flex-col min-h-screen">
      {/* Header at the top */}
      <PageHeader toggleSidebar={() => setIsSidebarOpen(!isSidebarOpen)} />
      <div></div> 
      {/* Public pages will be displayed here */}
      
      <main className="flex-1 p-4">
        <Outlet /> {/* InfoScreen.tsx will load here */}
      </main>
    </div>
  );
};

export default PublicLayout;

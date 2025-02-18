import { Link, useLocation } from "react-router-dom";
// import { useAuth } from "../../hooks/useAuth";
import { useState } from "react";

const Sidebar = ({ isOpen }: { isOpen: boolean }) => {
    // const { isAuthenticated, userRole } = useAuth();
    
    // if (!isAuthenticated) return null; // Hide sidebar if not logged in

    const location = useLocation(); // Get current path to highlight active link
    
    return (
        <div 
        className={`fixed top-0 left-0 h-full w-64 bg-gray-100 shadow-md transform ${
            isOpen ? "translate-x-0" : "-translate-x-64"
            } transition-transform duration-300 ease-in-out`}
        >
            <nav className="flex flex-col gap-4">
                <h2 className="text-xl font-bold">Navigation</h2>
                <Link to="/dashboard"
                    className={`py-2 px-4 rounded-md ${
                        location.pathname === "/dashboard" ? "bg-blue-500 text-white" : "hover:bg-gray-200"
                    }`}
                >
                    📊 Dashboard
                </Link>

                <Link to="/dashboard/devices"
                    className={`py-2 px-4 rounded-md ${
                        location.pathname === "/dashboard/devices" ? "bg-blue-500 text-white" : "hover:bg-gray-200"
                    }`}
                >
                    🖥️ Devices
                </Link>

                <Link to="/dashboard/deviceOverviews"
                    className={`py-2 px-4 rounded-md ${
                        location.pathname === "/dashboard/deviceOverviews" ? "bg-blue-500 text-white" : "hover:bg-gray-200"
                    }`}
                >
                    📋 Device Overview
                </Link>

                <Link to="/dashboard/logs"
                    className={`py-2 px-4 rounded-md ${
                        location.pathname === "/dashboard/logs" ? "bg-blue-500 text-white" : "hover:bg-gray-200"
                    }`}
                >
                    📜 Logs
                </Link>

                <hr className="my-2 border-gray-300" />

                <Link to="/home"
                    className={`py-2 px-4 rounded-md ${
                        location.pathname === "/home" ? "bg-blue-500 text-white" : "hover:bg-gray-200"
                    }`}
                >
                    Home
                </Link>

                <Link to="/home/myborrows"
                    className={`py-2 px-4 rounded-md ${
                        location.pathname === "/home/myborrows" ? "bg-blue-500 text-white" : "hover:bg-gray-200"
                    }`}
                >
                    My Borrows
                </Link>

                <Link to="/home/profile"
                    className={`py-2 px-4 rounded-md ${
                        location.pathname === "/home/profile" ? "bg-blue-500 text-white" : "hover:bg-gray-200"
                    }`}
                >
                    Profile
                </Link>
            </nav>
        </div>
    );
    // return (
    //     <div className="w-64 h-screen bg-gray-100 p-4 shadow-md">
    //         <nav className="flex flex-col gap-4">
    //             {userRole === "admin" ? (
    //                 <>
    //                     <Link to="/dashboard" className="btn btn-primary">📊 Dashboard🏠📑 📝⚙️ 📟</Link>
    //                     <li><Link to="/dashboard/devices">  🖥️ Devices </Link></li>
    //                     <li><Link to="/dashboard/deviceOverviews">  📋 Device Overview </Link></li>
    //                     <li><Link to="/dashboard/logs"> 📜 Logs   🔍👥 🔧 🛠️👤</Link></li>
    //                 </>
    //             ) : (
    //                 <>
    //                     <Link to="/home" className="btn btn-primary">Home</Link>
    //                     <Link to="/myborrows" className="btn">My Borrows</Link>
    //                     <Link to="/profile" className="btn">Profile</Link>
    //                 </>
    //             )}
    //         </nav>
    //     </div>
    // );
};

export default Sidebar;

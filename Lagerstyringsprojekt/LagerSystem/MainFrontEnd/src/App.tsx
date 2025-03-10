import './App.css'
import { themeChange } from "theme-change";
import { useState, useEffect } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import AdminLayout from "./layouts/AdminLayout";
// import UserLayout from "./layouts/UserLayout";
import PublicLayout from "./layouts/PublicLayout";
import Dashboard from "./pages/admin/Dashboard";
// import UserManagement from "./pages/admin/UserManagement";
import DeviceManagement from "./pages/admin/DeviceManagement";
import DeviceOverviewManagement from "./pages/admin/DeviceOverviewManagement";
import LogManagement from "./pages/admin/LogManagement";
import Home from "./pages/user/Home";
//import Profile from "./pages/user/Profile";
//import MyBorrows from "./pages/user/MyBorrows";
import InfoScreen from "./pages/public/InfoScreen";
import Login from "./pages/auth/Login";
import { AuthModal } from "./pages/auth/AuthModal";
// Imports pageheader uses on all sites
import { PageHeader } from './components/layout/PageHeader';
import DeviceOverviewPage from './pages/admin/DeviceOverviewPage';
import DevicePage from './pages/admin/DevicePage';
import UserPage from './pages/admin/UserPage';
// import Register from "./pages/auth/Register";
// import ForgotPassword from "./pages/auth/ForgotPassword";

const App = () => {
  const [authModal, setAuthModal] = useState<{ isOpen: boolean; view: "login" | "register" | null }>({
    isOpen: false,
    view: null,
  });

  const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  const openModal = (view: "login" | "register") => {
    setAuthModal({ isOpen: true, view });
  };

  const closeModal = () => {
    setAuthModal({ isOpen: false, view: null });
  };

  // Initialize themeChange on page load
  useEffect(() => {
    themeChange(false);
  }, []);

  return (
    <div className='pt-10'>
    <Router>
      <div className="fixed top-0 left-0 right-0 bg-white z-50 shadow-md h-16 flex items-center px-4">
        <PageHeader toggleSidebar={() => setIsSidebarOpen(!isSidebarOpen)} openModal={openModal} />
      </div>
      <Routes>
        {/* Public Layout (for /public/* pages) default page*/}
        <Route path="/" element={<PublicLayout />}>
          <Route index element={<InfoScreen />} />
        </Route>

         {/* AdminLayout: dashboard  */}
         <Route path="/dashboard" element={<AdminLayout isSidebarOpen={isSidebarOpen} />}>
          <Route index element={<Dashboard />} />
          <Route path="/dashboard/logs" element={<LogManagement />} />
          {/* <Route path="/dashboard/devices" element={<DeviceManagement />} /> */}
          <Route path="/dashboard/devices" element={<DevicePage />} />
          {/* <Route path="/dashboard/deviceOverviews" element={<DeviceOverviewManagement />} /> */}
          <Route path="/dashboard/deviceOverviews" element={<DeviceOverviewPage />} />
          <Route path="/dashboard/user" element={<UserPage />} />
        </Route>

        {/* User Layout (for /user/* pages) */}
        {/* <Route path="/user" element={<UserLayout />}>
          <Route index element={<Home />} />
          <Route path="profile" element={<Profile />} />
          <Route path="borrows" element={<MyBorrows />} />
        </Route> */}
      </Routes>
    </Router>
      {/* <div className="p-4">
        <h1 className="text-3xl font-bold">Theme Switcher</h1>
        <button className="btn" data-set-theme="light">
          Light Theme
        </button>
        <button className="btn" data-set-theme="dark">
          Dark Theme
        </button>
        <button className="btn" data-set-theme="cupcake">
          Cupcake Theme
        </button>
        <button className="btn" data-set-theme="light">
          Retro
        </button>
    </div>
       <div className="perspective-distant flex items-center justify-center h-screen bg-gray-800">
        
        <h1 className="text-4xl text-white  pd-40" > Hello, version4 </h1>
        <article className="rotate-x-51 rotate-z-43 transform-3d">
          <img
            src="https://cdn.mos.cms.futurecdn.net/UcXeK6DWKBWdc3Ao4TZ9nU.jpg"
            alt=""
            className='max-w-3xl'
            height='600'
            width='500'
          />
        </article>

        <div className="hero bg-base-200 min-h-screen">
          <div className="hero-content flex-col lg:flex-row">
            <img
              src="https://img.daisyui.com/images/stock/photo-1635805737707-575885ab0820.webp"
              className="max-w-sm rounded-lg shadow-2xl" />
            <div>
              <h1 className="text-5xl font-bold">Box Office News!</h1>
              <p className="py-6">
                Provident cupiditate voluptatem et in. Quaerat fugiat ut assumenda excepturi exercitationem
                quasi. In deleniti eaque aut repudiandae et a id nisi.
              </p>
              <button className="btn btn-primary">Get Started</button>
            </div>
          </div>
        </div>
      </div> */}

      {/* Auth Modal (Global) */}
      <AuthModal isOpen={authModal.isOpen} view={authModal.view} closeModal={closeModal} />
    </div>
  )
}

export default App

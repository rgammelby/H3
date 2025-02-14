import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import HomePage from './pages/HomePage';  // Import the HomePage component
import CreateDevicePage from './pages/CreateDevicePage';  // Import CreateDevicePage component
import GetAllDevicesPage from './pages/GetAllDevicesPage'; // Make sure to import this if needed
import GetUserPage from './pages/GetUserPage';
import GetallActivitiesPage from './pages/GetAllActivitiesPage';
import GetAllDeviceOverviewsPage from './pages/GetAllDeviceOverviewsPage';

function App() {
  return (
    <Router>
      <nav>
        <Link to="/">Home</Link> | 
        <Link to="/create-device">Create Device</Link> | 
        <Link to="/get-all-devices">Get all devices</Link> |
        <Link to ="/get-user">Get User</Link> |
        <Link to="/get-all-activities">Get all activities</Link> |
        <Link to="/get-all-deviceoverviews">Get all deviceoverviews</Link>
      </nav>

      <Routes>
        {/* Home Page Route */}
        <Route path="/" element={<HomePage />} />
        
        {/* Get All Devices Route */}
        <Route path="/get-all-devices" element={<GetAllDevicesPage />} />

        {/* Create Device Page Route */}
        <Route path="/create-device" element={<CreateDevicePage />} />

        {/* Get User Route */}
        <Route path="/get-user" element={<GetUserPage />} />

        {/* Get All Activities Route */}
        <Route path="/get-all-activities" element={<GetallActivitiesPage />} />
        
        {/* Get All DeviceOverviews Route */}
        <Route path="/get-all-deviceoverviews" element={<GetAllDeviceOverviewsPage />} />
      </Routes>
    </Router>
  );
}

export default App;

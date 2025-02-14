import React from 'react';
import GetAllDevices from '../services/GetAllDevices'; // Make sure to import this if needed

const GetAllDevicesPage = () => {
  return (
    <div>
      <div style={{ position: 'absolute', left: '35%', top: '2%' }}>
        <h1>Complete device list</h1>
      </div>
      <GetAllDevices /> {/* Include this if it's part of the Create Device page */}
    </div>
  );
};

export default GetAllDevicesPage;

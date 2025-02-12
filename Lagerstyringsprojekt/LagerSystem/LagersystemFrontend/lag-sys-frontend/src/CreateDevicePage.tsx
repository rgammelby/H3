import React from 'react';
import CreateDevice from './CreateDevice'; // Make sure to import this if needed

const CreateDevicePage = () => {
  return (
    <div>
      <div style={{ position: 'absolute', left: '35%', top: '2%' }}>
        <h1>Create device</h1>
        <p>Please input relevant information for the device.</p>
      </div>
      <CreateDevice /> {/* Include this if it's part of the Create Device page */}
    </div>
  );
};

export default CreateDevicePage;

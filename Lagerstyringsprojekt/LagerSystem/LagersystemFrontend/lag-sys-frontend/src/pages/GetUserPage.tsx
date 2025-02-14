import React from 'react';
import GetUser from '../services/GetUser'; // Make sure to import this if needed

const CreateDevicePage = () => {
  return (
    <div>
      <div style={{ position: 'absolute', left: '35%', top: '2%' }}>
        <h1>User by ID</h1>
      </div>
      <GetUser /> {/* Include this if it's part of the Create Device page */}
    </div>
  );
};

export default CreateDevicePage;

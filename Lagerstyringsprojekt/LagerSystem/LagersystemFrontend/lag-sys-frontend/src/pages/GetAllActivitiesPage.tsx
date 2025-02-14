import React from 'react';
import GetAllActivities from '../services/GetAllActivities'; // Make sure to import this if needed

const GetallActivitiesPage = () => {
  return (
    <div>
      <div style={{ position: 'absolute', left: '35%', top: '2%' }}>
        <h1>Complete activity list</h1>
      </div>
      <GetAllActivities /> {/* Include this if it's part of the Create Device page */}
    </div>
  );
};

export default GetallActivitiesPage;

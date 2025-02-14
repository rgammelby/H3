import React from 'react';
import GetAllDeviceOverviews from '../services/GetAllDeviceOverviews';

function GetAllDeviceOverviewsPage(){
    
    return (
    <div>
        <div style={{ position: 'absolute', left: '35%', top: '2%' }}>
        <h1>Complete deviceoverview list</h1>
        </div>
        <GetAllDeviceOverviews />
    </div>
    );
}

export default GetAllDeviceOverviewsPage;
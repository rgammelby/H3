import React from "react";
import CreateDeviceOverview from "../services/CreateDeviceOverview";

function CreateDeviceOverviewPage(){
    return(
        <div>
            <div style={{ position: 'absolute', left: '35%', top: '2%' }}>
                <h1>Create deviceoverview</h1>
                <p>Please input relevant information for the device.</p>
            </div>
            <CreateDeviceOverview />
        </div>
    );
}

export default CreateDeviceOverviewPage;
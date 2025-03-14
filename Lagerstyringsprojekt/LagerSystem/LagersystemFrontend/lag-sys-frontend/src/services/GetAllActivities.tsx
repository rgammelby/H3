import React, { useState, useEffect } from "react";

interface Activity {
    id: number;
    user_id: number;
    device_id: number;
    activity_type: number;
    start_date: string;
    end_date: string;
    created_at: string;
    notes: string;
    lifecycle_id: string;
}

interface User {
    [key: number]: string;
}

interface SingleDevice {
    [key: number]: string;
}

interface ActivityType {
    [key: number]: string;
}

function GetAllActivities() {
    const [activities, setActivities] = useState<Activity[]>([]);
    const [users, setUsers] = useState<User>({});
    const [devices, setDevices] = useState<SingleDevice>({});
    const [activityTypes, setActivityTypes] = useState<ActivityType>({});
    const [loading, setLoading] = useState(true);

    const fetchAllActivities = async () => {
        try {
            const response = await fetch("http://localhost:7093/GetAllActivities");
            if (!response.ok) {
                throw new Error("Failed to fetch activities");
            }
            const activitiesData: Activity[] = await response.json();
            setActivities(activitiesData);
            console.log("activitiesData: ", activitiesData);

            // Fetch related data: users, devices, and activity types
            const fetchUserPromises = activitiesData.map((activity) =>
                fetch(`http://localhost:7093/GetUser/${activity.user_id}`)
                    .then((userResponse) => userResponse.json())
                    .then((userData) => {
                        return { [activity.user_id]: userData.first_name + " " + userData.last_name };
                    })
            );

            const fetchDevicePromises = activitiesData.map((activity) =>
                fetch(`http://localhost:7093/api/Device/${activity.device_id}`)
                    .then((deviceResponse) => deviceResponse.json())
                    .then((deviceData) => {
                        return { [activity.device_id]: deviceData.description };
                    })
            );

            const fetchActivityTypePromises = activitiesData.map((activity) =>
                fetch(`http://localhost:7093/ActivityType/${activity.activity_type}`)
                    .then((activityTypeResponse) => activityTypeResponse.json())
                    .then((activityTypeData) => {
                        return { [activity.activity_type]: activityTypeData.activity_type }; // Assuming 'description' is the correct property
                    })
            );

            // Wait for all promises to resolve
            const [userResults, deviceResults, activityTypeResults] = await Promise.all([
                Promise.all(fetchUserPromises),
                Promise.all(fetchDevicePromises),
                Promise.all(fetchActivityTypePromises),
            ]);

            // Merge all results into state
            setUsers(userResults.reduce((acc, curr) => ({ ...acc, ...curr }), {}));
            setDevices(deviceResults.reduce((acc, curr) => ({ ...acc, ...curr }), {}));
            setActivityTypes(activityTypeResults.reduce((acc, curr) => ({ ...acc, ...curr }), {}));

            setLoading(false); // Done loading
        } catch (error) {
            console.error("Error fetching data:", error);
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchAllActivities();
    }, []);

    return (
        <>
            {loading ? (
                <div>Loading activities...</div>
            ) : (
                <table>
                    <thead>
                        <tr>
                            <th>Activity ID</th>
                            <th>User</th>
                            <th>Device</th>
                            <th>Activity Type</th>
                            <th>Start Date</th>
                            <th>End Date</th>
                            <th>Notes</th>
                        </tr>
                    </thead>
                    <tbody>
                        {activities.map((activity) => (
                            <tr key={activity.id}>
                                <td>{activity.id}</td>
                                <td>{users[activity.user_id] || "Unknown User"}</td>
                                <td>{devices[activity.device_id] || "Unknown Device"}</td>
                                <td>{activityTypes[activity.activity_type] || "Unknown Activity Type"}</td>
                                <td>{activity.start_date || "No Start Date"}</td>
                                <td>{activity.end_date || "No End Date"}</td>
                                <td>{activity.notes || "No Notes"}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </>
    );
}

export default GetAllActivities;
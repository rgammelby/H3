import React, { useState, useEffect } from 'react';
import CheckLogin from './CheckLogin';

interface User {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
}

interface LoanActivity {
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

interface ForeignData {
    [key: number]: string;  // This will be used for users, devices, and activity types
}

const UserProfile: React.FC = () => {
    const [user, setUser] = useState<User | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [loanHistory, setLoanHistory] = useState<LoanActivity[]>([]);
    const [isLoanHistoryVisible, setIsLoanHistoryVisible] = useState(false);
    const [users, setUsers] = useState<ForeignData>({});
    const [devices, setDevices] = useState<ForeignData>({});
    const [activityTypes, setActivityTypes] = useState<ForeignData>({});
    const [error, setError] = useState<string | null>(null);

    // Fetch user data based on email
    const fetchUserData = async () => {
        const userEmail = localStorage.getItem('userEmail');
        if (userEmail) {
            try {
                const response = await fetch(`http://localhost:7093/GetUserIdByEmail?email=${userEmail}`);
                const data = await response.json();

                if (response.ok) {
                    setUser({
                        id: data.id,
                        firstName: data.first_name,
                        lastName: data.last_name,
                        email: data.email,
                        phone: data.telephone,
                    });
                } else {
                    setError('Failed to fetch user data');
                }
            } catch (error) {
                setError('An error occurred while fetching user data');
            }
        }
    };

    // Fetch loan history along with foreign key data
    const fetchLoanHistory = async (userId: string) => {
        console.log("User ID: ", userId);
        try {
            const response = await fetch(`http://localhost:7093/GetActivitiesByUserId/${userId}`);
            const data = await response.json();

            if (response.ok) {
                setLoanHistory(data);

                // Fetch associated foreign data (users, devices, activity types)
                const userPromises = data.map((loan: LoanActivity) =>
                    fetch(`http://localhost:7093/GetUser/${loan.id}`)
                        .then((userResponse) => userResponse.json())
                        .then((userData) => {
                            return { [loan.user_id]: `${userData.first_name} ${userData.last_name}` };
                        })
                );

                const devicePromises = data.map((loan: LoanActivity) =>
                    fetch(`http://localhost:7093/api/Device/${loan.device_id}`)
                        .then((deviceResponse) => deviceResponse.json())
                        .then((deviceData) => {
                            return { [loan.device_id]: deviceData.description };
                        })
                );

                const activityTypePromises = data.map((loan: LoanActivity) =>
                    fetch(`http://localhost:7093/ActivityType/${loan.activity_type}`)
                        .then((activityTypeResponse) => activityTypeResponse.json())
                        .then((activityTypeData) => {
                            return { [loan.activity_type]: activityTypeData.activity_type };
                        })
                );

                // Wait for all promises to resolve and update the state with the foreign data
                const [userResults, deviceResults, activityTypeResults] = await Promise.all([
                    Promise.all(userPromises),
                    Promise.all(devicePromises),
                    Promise.all(activityTypePromises),
                ]);

                setUsers(userResults.reduce((acc, curr) => ({ ...acc, ...curr }), {}));
                setDevices(deviceResults.reduce((acc, curr) => ({ ...acc, ...curr }), {}));
                setActivityTypes(activityTypeResults.reduce((acc, curr) => ({ ...acc, ...curr }), {}));
            } else {
                setError('Failed to fetch loan history');
            }
        } catch (error) {
            setError('An error occurred while fetching loan history');
        }
    };

    useEffect(() => {
        fetchUserData();
    }, []);

    useEffect(() => {
        if (user) {
            fetchLoanHistory(user.id);
        }
    }, [user]);

    const handleEditToggle = () => {
        setIsEditing(!isEditing);
    };

    const handleSaveProfile = async () => {
        if (user) {
            // Prepare the profile update data
            const userData = {
                id: user.id,          // Include user ID in the object
                firstname: user.firstName,
                lastname: user.lastName,
                telephone: user.phone,
                password: '',         // Ensure password remains empty if not modified
            };

            try {
                const response = await fetch('http://localhost:7093/UpdateUser', {  // Using POST method without the ID in the URL
                    method: 'POST',  // Change to POST
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(userData),  // Send the user data as the body
                });

                if (response.ok) {
                    setIsEditing(false);
                } else {
                    setError('Failed to update profile');
                }
            } catch (error) {
                setError('An error occurred while saving profile');
            }
        }
    };

    const handleLogout = () => {
        // Clear user-related data from local storage
        localStorage.removeItem('isLoggedIn');
        localStorage.removeItem('userEmail');

        // Redirect to the home page ("/")
        window.location.href = '/';
    };


    return (
        <div className="profile-container">
            <CheckLogin />
            <h1>User Profile</h1>
            {error && <p style={{ color: 'red' }}>{error}</p>}
    
            {user ? (
                <>
                    <div className="profile-info">
                        <h2>Profile Information</h2>
                        {isEditing ? (
                            <div>
                                <input
                                    type="text"
                                    value={user.firstName}
                                    onChange={(e) => setUser({ ...user, firstName: e.target.value })}
                                />
                                <input
                                    type="text"
                                    value={user.lastName}
                                    onChange={(e) => setUser({ ...user, lastName: e.target.value })}
                                />
                                <input
                                    type="text"
                                    value={user.phone}
                                    onChange={(e) => setUser({ ...user, phone: e.target.value })}
                                />
                                <button onClick={handleSaveProfile}>Save</button>
                                <button onClick={handleEditToggle}>Cancel</button>
                            </div>
                        ) : (
                            <div>
                                <p>First Name: {user.firstName}</p>
                                <p>Last Name: {user.lastName}</p>
                                <p>Email: {user.email}</p>
                                <p>Phone: {user.phone}</p>
                                <button onClick={handleEditToggle}>Edit Profile</button>
                            </div>
                        )}
                    </div>
    
                    <div>
                        <button onClick={() => setIsLoanHistoryVisible(!isLoanHistoryVisible)}>
                            {isLoanHistoryVisible ? 'Hide loan history' : 'View loan history'}
                        </button>
                    </div>
    
                    {isLoanHistoryVisible && (
                        <div className="loan-history">
                            <h2>Loan History</h2>
                            <div className="loan-grid">
                                {loanHistory.length > 0 ? (
                                    loanHistory.map((loan, index) => (
                                        <div className="loan-card" key={`${loan.id}_${loan.user_id}_${index}`}>
                                            <p>Loan ID: {loan.id}</p>
                                            <p>User: {users[loan.user_id] || "Unknown User"}</p>
                                            <p>Device: {devices[loan.device_id] || "Unknown Device"}</p>
                                            <p>Activity Type: {activityTypes[loan.activity_type] || "Unknown Activity Type"}</p>
                                            <p>Start Date: {loan.start_date || "No Start Date"}</p>
                                            <p>End Date: {loan.end_date || "No End Date"}</p>
                                            <p>Notes: {loan.notes || "No Notes"}</p>
                                        </div>
                                    ))
                                ) : (
                                    <p>No loan history available</p>
                                )}
                            </div>
                        </div>
                    )}
    
                    <button onClick={() => { localStorage.clear(); window.location.href = '/'; }}>Log Out</button>
                </>
            ) : (
                <p>Loading user profile...</p>
            )}
    
            <style>{`
            html, body {
                height: 100%;
                margin: 0;
                display: flex;
                justify-content: center;
                align-items: flex-start; /* Align to the top instead of centering */
            }
    
            .profile-container {
                max-width: 800px;
                margin: 0 auto;
                padding: 2rem;
                text-align: center;
                display: flex;
                flex-direction: column;
                align-items: center;
                width: 100%;
                flex-grow: 1; /* Allow container to grow with content */
                justify-content: flex-start; /* Ensure content is at the top */
            }
    
            .profile-info {
                text-align: center;
                width: 100%;
                margin-bottom: 2rem; /* Space between profile info and loan history */
            }
    
            .loan-history {
                width: 100%;
                margin-top: 2rem;
            }
    
            .loan-grid {
                display: grid;
                grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
                gap: 1rem;
                padding: 1rem;
                justify-content: center;
                width: 100%;
                max-width: 800px;
            }
    
            .loan-card {
                background: #f8f9fa;
                padding: 1rem;
                border-radius: 8px;
                box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.1);
                transition: transform 0.2s ease-in-out;
            }
    
            .loan-card:hover {
                transform: scale(1.02);
            }
    
            @media (max-width: 600px) {
                .loan-grid {
                    grid-template-columns: 1fr;
                }
            }
            `}</style>
        </div>
    );
    
    
};

export default UserProfile;

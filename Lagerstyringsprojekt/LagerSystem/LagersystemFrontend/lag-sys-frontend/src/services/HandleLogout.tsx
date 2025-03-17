import React from 'react';

const LogoutButton = () => {
    const handleLogout = () => {
        localStorage.removeItem('isLoggedIn');
        localStorage.removeItem('userEmail');
        window.location.href = '/';
    };

    return <button onClick={handleLogout}>Log Out</button>;
};

export default LogoutButton;

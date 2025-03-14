import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const CheckLogin: React.FC = () => {
  const navigate = useNavigate(); // Initialize navigate hook

  useEffect(() => {
    const isLoggedIn = localStorage.getItem('isLoggedIn');
    const expectedPath = localStorage.getItem('expectedPath');
    
    // If user is not logged in, set the expected path only if it's not already the login page
    if (!isLoggedIn) {
      // Only set expectedPath if the current path isn't already the login page
      if (window.location.pathname !== '/login') {
        localStorage.setItem('expectedPath', window.location.pathname);
      }
      // Redirect to login page
      navigate('/login');
    } else if (expectedPath) {
      // If logged in and there's an expected path, redirect there
      localStorage.removeItem('expectedPath');
      navigate(expectedPath);
    }
  }, [navigate]);

  return <></>; // Nothing to render, just logic for redirecting
};

export default CheckLogin;

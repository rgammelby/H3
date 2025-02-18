import { useState, useEffect } from "react";

export function useAuth () {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [userRole, setUserRole] = useState<"admin" | "user" | null>(null);

    useEffect(() => {
        // Check if user is authenticated
        const token = localStorage.getItem("token");
        if (token) {
            setIsAuthenticated(true);
            setUserRole(localStorage.getItem("role") as "admin" | "user");
            // setUserRole(JSON.parse(token).role); // Assuming { role: "admin" | "user" }
        }
    }, []);

    return { isAuthenticated, userRole };
}

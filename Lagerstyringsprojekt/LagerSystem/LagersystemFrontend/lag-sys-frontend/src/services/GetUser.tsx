import React, { useState } from "react";

function GetUser() {
    const [input, setInput] = useState(""); // Store input as state
    const [deviceOverviews, setDeviceOverviews] = useState<string | null>(null); // Store API result in state

    const FetchUser = async () => {
        const userInput = (document.getElementById("user_id") as HTMLInputElement).value;
        setInput(userInput); // Update state (not just a variable)

        try {
            let response = await fetch(`https://localhost:7093/GetUser/${userInput}`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                throw new Error("Failed to fetch data");
            }

            const data = await response.json(); // Await the JSON response
            setDeviceOverviews(JSON.stringify(data)); // Store API result in state
        } catch (error) {
            console.error("Error fetching data:", error);
            setDeviceOverviews("Error fetching data");
        }
    };

    return (
        <>
            <input id="user_id" style={{position: "absolute", left: "42.5%", top: "79%", width: "15%"}}/>
            <button onClick={FetchUser} style={{position: "absolute", left: "45%", top: "70%", width: "10%"}}>Get User</button>

            <div style={{width: "10%", height: "auto", position: "absolute", left: "7%", top: "85%"}}>
                <a>{deviceOverviews !== null ? deviceOverviews : "Hello"}</a>
            </div>
        </>
    );
}

export default GetUser;

export const fetchLogs = async () => {
    const response = await fetch("https://localhost:7093/api/log");
    // check if the response is successful
    if (!response.ok) throw new Error("Failed to fetch logs");
    // convert the response to json
    return response.json();
  };
  
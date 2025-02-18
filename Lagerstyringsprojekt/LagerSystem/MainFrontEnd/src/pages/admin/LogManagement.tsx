import React, { useState, useEffect } from 'react';
import { fetchLogs } from '../../services/logService';

// define Log class
interface Log {
    id: number;
    log_type: string;
    log_message: string;
    // Often dates come as strings in JSON format.
    timestamp: string;
}   

const LogManagement : React.FC = () => {
    // State to store logs
    // Holds the list of logs fetched from the API. 
    // It starts as an empty array.
    const [logs, setLogs] = useState<Log[]>([]);

    // Loading state
    // A boolean to know if the data is still being fetched. 
    // Starts as true
    const [loading, setLoading] = useState<boolean>(true);

    // Error handling state
    // olds any error message if the fetching fails.
    // It starts as null (meaning no error).
    const [error, setError] = useState<string | null>(null);
    
    // State to store the expanded log
    const [expandedLog, setExpandedLog] = useState<number | null>(null);

    // Sorting order
    const [sortOrder, setSortOrder] = useState<"asc" | "desc">("asc"); // orting order

    // Fetch logs when the component mounts
    useEffect(() => {
        fetchLogs()
        .then((data) => {
            console.log("Fetched data:", data); // Debugging output
            setLogs(data); // Set logs data
            setLoading(false); // Turn off loading
        })
        .catch((err) => {
            // if there is an error, update the error state
            setError(err.message); // Store error message
            setLoading(false);
        });
    }, []);

    //  Define toggle function
    const toggleExpand = (id: number) => {
        setExpandedLog(expandedLog === id ? null : id); // Toggle the expanded row
    };

    // Function to toggle sorting
    const toggleSort = () => {
        const newOrder = sortOrder === "asc" ? "desc" : "asc";
        setSortOrder(newOrder);
    };


    //  Sort logs before rendering
    const sortedLogs = [...logs].sort((a, b) => {
        if (sortOrder === "asc") {
        return a.log_type.localeCompare(b.log_type); // Sort A → Z
        } else {
        return b.log_type.localeCompare(a.log_type); // Sort Z → A
        }
    });

    // Show loading message while data is being fetched
    if (loading) {
        return <h1 className="text-xl font-bold">Loading logs...</h1>;
    }

    // Show error message if fetching failed
    if (error) {
        return <div className="text-red-500">Error: {error}</div>;
    }

    // Display fetched logs
    return (
        <div className="p-6">
      <h1 className="text-3xl font-bold mb-4 text-center">📜 Log Management</h1>

      <div className="overflow-x-auto shadow-lg rounded-lg">
        <table className="table w-full table-zebra">
          {/* Table Head */}
          <thead className="bg-base-200">
            <tr>
              <th className="text-left">#</th>
              <th className="text-left">
                Type{" "}
                <button
                  onClick={toggleSort}
                  className="ml-2 text-blue-500 hover:underline"
                >
                  {sortOrder === "asc" ? "▲" : "▼"}
                </button>
              </th>
              <th className="text-left">Message</th>
              <th className="text-left">Timestamp</th>
              <th className="text-center">Actions</th>
            </tr>
          </thead>

          {/* Table Body */}
          <tbody>
            {sortedLogs.map((log, index) => (
              <React.Fragment key={log.id}>
                {/* Main Row */}
                <tr className="hover:bg-base-300">
                  <th>{index + 1}</th>
                  <td className="font-semibold">{log.log_type}</td>
                  <td>
                    {log.log_message.length > 125 ? (
                      <>
                        {log.log_message.slice(0, 50)}...
                        <button
                          onClick={() => toggleExpand(log.id)}
                          className="ml-2 text-blue-500 hover:underline"
                        >
                          {expandedLog === log.id ? "▲ Show Less" : "▼ Show More"}
                        </button>
                      </>
                    ) : (
                      log.log_message
                    )}
                  </td>
                  <td>{new Date(log.timestamp).toLocaleString()}</td>
                  <td className="flex justify-center space-x-2">
                    <button className="btn btn-sm btn-primary">View</button>
                    <button className="btn btn-sm btn-secondary">Delete</button>
                  </td>
                </tr>

                {/* Expanded Row (Hidden by default, shown when clicked) */}
                {expandedLog === log.id && (
                  <tr className="bg-base-200">
                    <td colSpan={5} className="p-4">
                      <strong>Full Message:</strong>
                      <p className="mt-2 text-gray-700">{log.log_message}</p>
                    </td>
                  </tr>
                )}
              </React.Fragment>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default LogManagement;
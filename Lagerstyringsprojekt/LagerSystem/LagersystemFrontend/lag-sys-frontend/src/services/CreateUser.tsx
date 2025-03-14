import React, { useState } from "react";

interface FormData {
  first_name: string;
  last_name: string;
  email: string;
  password: string;
  telephone: string;
  is_active: boolean;
  type: string;
}

const CreateUser: React.FC = () => {
  const [formData, setFormData] = useState<FormData>({
    first_name: '',
    last_name: '',
    email: '',
    password: '',
    telephone: '',
    is_active: true,
    type: '',
  });
  const [response, setResponse] = useState<string | null>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const createUser = async (e: React.FormEvent) => {
    e.preventDefault();
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    const isValidEmail = emailRegex.test(formData.email);

    if (!isValidEmail) {
      setResponse("Not valid email");
      return;
    }

    try {
      console.log("Sending request with data:", formData);

      const response = await fetch("http://localhost:7093/AddUser", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(formData),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      const result = await response.text(); // Use .text() to handle empty responses
      console.log("Response received:", result);

      if (result) {
        try {
          const jsonResult = JSON.parse(result);
          setResponse(JSON.stringify(jsonResult, null, 2));
        } catch (e: unknown) {
          // TypeScript will now understand that `e` is an `Error` because we've narrowed its type
          setResponse(`Error parsing JSON: ${(e as Error).message}`);
        }
      } else {
        setResponse("Success: User created, but no response data received.");
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error("Error creating user:", error);
        setResponse(`Error: ${error.message}`);
      } else {
        // Fallback case for unknown errors
        setResponse("An unknown error occurred.");
      }
    }
  };


  return (
    <div style={{ display: "flex", flexDirection: "column", alignItems: "center", marginTop: "10%" }}>
      <form onSubmit={createUser} style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
        <input
          type="text"
          name="first_name"
          value={formData.first_name}
          onChange={handleChange}
          placeholder="First Name"
          style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
          required
        />
        <input
          type="text"
          name="last_name"
          value={formData.last_name}
          onChange={handleChange}
          placeholder="Last Name"
          style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
          required
        />
        <input
          type="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
          placeholder="Email"
          style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
          required
        />
        <input
          type="password"
          name="password"
          value={formData.password}
          onChange={handleChange}
          placeholder="Password"
          style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
          required
        />
        <input
          type="text"
          name="telephone"
          value={formData.telephone}
          onChange={handleChange}
          placeholder="Telephone"
          style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
          required
        />
        <select
          name="type"
          value={formData.type}
          onChange={handleChange}
          style={{ width: "250px", padding: "8px", marginBottom: "10px", borderRadius: "5px", border: "1px solid gray" }}
          required
        >
          <option value="" disabled>Select Type</option>
          <option value="admin">Admin</option>
          <option value="user">User</option>
        </select>
        <button
          type="submit"
          style={{ width: "200px", padding: "10px", backgroundColor: "#007bff", color: "white", border: "none", borderRadius: "5px", cursor: "pointer" }}
        >
          Create User
        </button>
      </form>
      <div style={{ marginTop: "20px", width: "50%", textAlign: "center", whiteSpace: "pre-wrap", wordWrap: "break-word" }}>
        {response ? <pre>{response}</pre> : <p>Fill out the form and click "Create User"</p>}
      </div>
    </div>
  );
};

export default CreateUser;

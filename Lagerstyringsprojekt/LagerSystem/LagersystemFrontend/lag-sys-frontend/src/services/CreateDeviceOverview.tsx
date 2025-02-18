import React, { useState } from "react";

const CreateDeviceOverview: React.FC = () => {
    const [model, setModel] = useState<string>("");
    const [deviceType, setDeviceType] = useState<number>(1);
    const [image, setImage] = useState<File | null>(null);
    const [qty, setQty] = useState<number | undefined>(undefined);
    const [availableQty, setAvailableQty] = useState<number | undefined>(undefined);
    const [lastOrdered, setLastOrdered] = useState<string>("");
    const [message, setMessage] = useState<string | null>(null);

    const addDeviceOverview = async (formData: FormData) => {
        try {
            const response = await fetch('https://localhost:7093/api/DeviceOverview', {
                method: 'POST',
                body: formData, // No need to set Content-Type; browser will handle it
            });

            if (!response.ok) {
                throw new Error(`Error: ${response.statusText}`);
            }

            const result = await response.json();
            setMessage("Device overview added successfully!");
            console.log("Success:", result);
        } catch (error) {
            console.error("Error adding device overview:", error);
            setMessage("Failed to add device overview.");
        }
    };

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();

        if (!model || !deviceType || !image || !lastOrdered) {
            setMessage("Please fill all required fields.");
            return;
        }

        const formData = new FormData();
        formData.append("model", model);
        formData.append("device_type", deviceType.toString());
        formData.append("image", image);
        if (qty !== undefined) formData.append("qty", qty.toString());
        if (availableQty !== undefined) formData.append("available_qty", availableQty.toString());
        formData.append("last_ordered", new Date(lastOrdered).toISOString());

        await addDeviceOverview(formData);
    };

    return (
        <div className="p-4 max-w-md mx-auto bg-white rounded-lg shadow-md">
            <h2 className="text-xl font-bold mb-4">Create Device Overview</h2>
            {message && <p className="text-sm text-red-500">{message}</p>}
            <form onSubmit={handleSubmit} className="space-y-4">
                <input 
                    type="text"
                    value={model}
                    onChange={(e) => setModel(e.target.value)}
                    placeholder="Model Name"
                    required
                    className="w-full p-2 border rounded"
                />
                <input 
                    type="number"
                    value={deviceType}
                    onChange={(e) => setDeviceType(Number(e.target.value))}
                    placeholder="Device Type"
                    required
                    className="w-full p-2 border rounded"
                />
                <input 
                    type="file"
                    onChange={(e) => setImage(e.target.files ? e.target.files[0] : null)}
                    required
                    className="w-full p-2 border rounded"
                />
                <input 
                    type="number"
                    value={qty ?? ""}
                    onChange={(e) => setQty(e.target.value ? Number(e.target.value) : undefined)}
                    placeholder="Quantity"
                    className="w-full p-2 border rounded"
                />
                <input 
                    type="number"
                    value={availableQty ?? ""}
                    onChange={(e) => setAvailableQty(e.target.value ? Number(e.target.value) : undefined)}
                    placeholder="Available Quantity"
                    className="w-full p-2 border rounded"
                />
                <input 
                    type="date"
                    value={lastOrdered}
                    onChange={(e) => setLastOrdered(e.target.value)}
                    required
                    className="w-full p-2 border rounded"
                />
                <button 
                    type="submit"
                    className="w-full p-2 bg-blue-500 text-white rounded hover:bg-blue-600"
                >
                    Submit
                </button>
            </form>
        </div>
    );
};

export default CreateDeviceOverview;

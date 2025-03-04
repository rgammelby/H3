import React, { useState, useEffect } from "react";
import { User } from "../../API/ApiInstances";
import { IUser } from "../../Interfaces/User";

function UserPage() {
    const [USERS, setUsers] = useState<IUser[]>([])

    useEffect(() => {
        const fetchData = async () => {
            try {
                const USERDATA: IUser[] = await User.fetchAllUsers();

                setUsers(USERDATA);
            }
            catch (ex) {
                console.error(ex);
            }
        }

        fetchData();
    }, [])

    return (
        <div className="overflow-x-auto">
            <table className="table w-full bg-base-100 shadow-lg rounded-lg">
                {/* Table Header */}
                <thead>
                    <tr className="bg-base-200 text-base font-semibold">
                        <th className="p-3 text-left">First Name</th>
                        <th className="p-3 text-left">Last Name</th>
                        <th className="p-3 text-left">Email</th>
                        <th className="p-3 text-left">Role</th>
                        <th className="p-3 text-center">Actions</th>
                    </tr>
                </thead>

                {/* Table Body */}
                <tbody>
                    {USERS.map((user) => (
                        <tr key={user.id} className="hover:bg-base-300 transition-all">
                            <td className="p-3">{user.first_name}</td>
                            <td className="p-3">{user.last_name}</td>
                            <td className="p-3">{user.email}</td>
                            <td className="p-3">
                                <span
                                    className={`badge ${user.type === "admin" ? "badge-primary" : "badge-secondary"
                                        }`}
                                >
                                    {user.type}
                                </span>
                            </td>
                            <td className="p-3 flex justify-center gap-2">
                                {/* Edit Button */}
                                <button
                                    className="btn btn-sm btn-outline btn-info">
                                    ✏️ Edit
                                </button>

                                {/* Delete Button */}
                                <button
                                    className="btn btn-sm btn-outline btn-error">
                                    🗑️ Delete
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    )
}

export default UserPage;
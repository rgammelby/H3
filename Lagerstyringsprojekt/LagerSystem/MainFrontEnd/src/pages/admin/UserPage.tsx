import React, { useState, useEffect } from "react";
import { User } from "../../API/ApiInstances";
import { IEditUser, IUser } from "../../Interfaces/User";
import ConfirmModal from "../../components/ui/ConfirmModal";

function UserPage() {
  const [USERS, setUsers] = useState<IUser[]>([]);
  const [DISABLEUSER, setDisableUser] = useState<IUser | null>();
  const [SHOWCONFIRM, setShowConfirm] = useState<boolean>(false);
  const [EDITUSERMODAL, setEditUserModal] = useState<boolean>(false);
  const [EDITUSERID, setEditUserId] = useState<number>(0);
  const [EDITUSER, setEditUser] = useState<IEditUser>();
  const [FORMDATA, setFormData] = useState<IEditUser>(
    EDITUSER ?? { id: 0, first_name: "", last_name: "", telephone: "", password: "" }
  );
  const [ERROR, setError] = useState<string>();

  // Handle update user input changes
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...FORMDATA, [e.target.name]: e.target.value });
  };

  const handleSubmit = () => {
    setEditUser(FORMDATA); // Update parent state with new user data
    setEditUserModal(false); // Close modal
  };

  const disableUser = async (id: number) => {
    try {
      const USERRESPONSE = await User.disbaleUser(id);
      if (!USERRESPONSE.ok) {
        setError(USERRESPONSE.statusText);
        return;
      }
      // Update user state after disabling
      setUsers((prevUsers) =>
        prevUsers.map((user) => (user.id === id ? { ...user, is_active: false } : user))
      );
    } catch (ex) {
      console.error(ex);
      setError("Error trying to disable user");
    }
  };

  const handleConfirm = (confirm: boolean) => {
    if (confirm && DISABLEUSER) {
      disableUser(DISABLEUSER.id);
    }
    setShowConfirm(false);
    setDisableUser(null);
  };

  const editModal = async (user: IEditUser) => {
    try {
      user.id = EDITUSERID;
      const USERRESPONSE = await User.editUser(user);

      if (!USERRESPONSE.ok) {
        setError(USERRESPONSE.statusText);
        return;
      }
    } catch (ex) {
      console.error(ex);
      setError("Error trying to edit user");
    }
  };

  useEffect(() => {
    try {
      if (EDITUSER !== null && EDITUSER !== undefined) {
        editModal(EDITUSER);
      }
    } catch (ex) {
      setError("Error updating user");
    }
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const USERDATA: IUser[] = await User.fetchAllUsers();

        setUsers(USERDATA);
      } catch (ex) {
        console.error(ex);
      }
    };

    fetchData();
  }, []);

  useEffect(() => {
    if (DISABLEUSER !== null && DISABLEUSER !== undefined && SHOWCONFIRM === true) {
      disableUser(DISABLEUSER.id);
    } else {
      setDisableUser(undefined);
    }
  }, [SHOWCONFIRM]);

  return (
    <div className="overflow-x-auto">
      {DISABLEUSER && <ConfirmModal confirmModal={handleConfirm} />}
      <table className="table w-full bg-base-100 shadow-lg rounded-lg table-zebra">
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
                  className={`badge ${user.type === "admin" ? "badge-primary" : "badge-secondary"}`}
                >
                  {user.type}
                </span>
              </td>
              <td className="p-3 flex justify-center gap-2">
                {/* Edit Button */}
                <button
                  onClick={() => {
                    setEditUserModal(true);
                    setEditUserId(user.id);
                  }}
                  className="btn btn-sm btn-outline btn-info"
                >
                  ✏️ Edit
                </button>

                {user.is_active ? (
                  <button
                    onClick={() => {
                      setDisableUser(user);
                    }}
                    className="btn btn-sm btn-outline btn-error"
                  >
                    🚫 Disable
                  </button>
                ) : (
                  <label className="btn btn-sm btn-ouline btn-primary ml-1 w-21">Disabled</label>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {EDITUSERMODAL ? (
        <div
          style={{ backgroundColor: "rgba(0, 0, 0, 0.5)" }}
          className="fixed inset-0 flex items-center justify-center"
          onClick={() => setEditUserModal(false)}
        >
          {/* Inner box - stops click from propagating */}
          <div
            className="bg-white p-6 rounded-lg shadow-lg w-96"
            onClick={(e) => e.stopPropagation()}
          >
            <h2 className="text-xl font-semibold mb-4">Update User Info</h2>

            {/* First Name */}
            <label className="block text-sm font-medium text-gray-700">First Name</label>
            <input
              type="text"
              name="first_name"
              value={FORMDATA.first_name}
              onChange={handleChange}
              className="w-full p-2 border rounded mb-3"
              placeholder="John"
            />

            {/* Last Name */}
            <label className="block text-sm font-medium text-gray-700">Last Name</label>
            <input
              type="text"
              name="last_name"
              value={FORMDATA.last_name}
              onChange={handleChange}
              className="w-full p-2 border rounded mb-3"
              placeholder="Doe"
            />

            {/* Telephone */}
            <label className="block text-sm font-medium text-gray-700">Telephone</label>
            <input
              type="tel"
              name="telephone"
              value={FORMDATA.telephone}
              onChange={handleChange}
              className="w-full p-2 border rounded mb-3"
              placeholder="123-456-7890"
            />

            {/* Password */}
            <label className="block text-sm font-medium text-gray-700">Password</label>
            <input
              type="password"
              name="password"
              value={FORMDATA.password}
              onChange={handleChange}
              className="w-full p-2 border rounded mb-3"
              placeholder="********"
            />

            {/* Update Button */}
            <button
              onClick={handleSubmit}
              className="w-full bg-blue-500 text-white p-2 rounded mt-2 hover:bg-blue-600"
            >
              Update
            </button>
          </div>
        </div>
      ) : (
        <></>
      )}
    </div>
  );
}

export default UserPage;

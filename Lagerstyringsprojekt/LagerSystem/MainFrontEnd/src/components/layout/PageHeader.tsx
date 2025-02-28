import { useState, useEffect } from "react";
import { Menu, Bell, Search, User, Mail } from "lucide-react";
import logo from "../../assets/logo.png";

export function PageHeader({
  toggleSidebar,
  openModal,
}: {
  toggleSidebar: () => void;
  openModal: (view: "login" | "register") => void;
}) {
  const [SIGNIN, setSignIn] = useState<boolean>(false);

  const LoggedIn = (bool: boolean) => {
    setSignIn(bool);
    if (bool) {
      localStorage.setItem("user", "true");
    } else {
      localStorage.removeItem("user");
    }
  };

  useEffect(() => {
    let logged = localStorage.getItem("user");
    LoggedIn(!!logged);
  }, []);

  return (
    <div className="flex items-center justify-between w-full h-16 px-6 bg-white shadow-md">
      {/* Left Section: Sidebar Toggle & Logo */}
      <div className="flex items-center gap-4">
        <button className="btn btn-ghost btn-square" onClick={toggleSidebar}>
          <Menu className="w-6 h-6" />
        </button>
        <a href="/">
          <img src={logo} alt="logo" className="h-8" />
        </a>
      </div>

      {/* Center: Search Bar & Mail Icon (Hidden on Small Screens) */}
      <form className="hidden md:flex items-center flex-grow max-w-lg">
        <div className="relative w-full">
          <input
            type="search"
            placeholder="Search..."
            className="w-full py-2 px-4 border border-gray-300 rounded-full shadow-sm focus:border-blue-500 outline-none"
          />
          <button className="absolute right-2 top-1/2 -translate-y-1/2 bg-gray-200 rounded-full p-2">
            <Search className="w-5 h-5 text-gray-600" />
          </button>
        </div>
        <button className="ml-3 bg-gray-200 p-2 rounded-full">
          <Mail className="w-5 h-5 text-gray-600" />
        </button>
      </form>

      {/* Right Section: Icons & User Dropdown */}
      <div className="flex items-center gap-4 md:gap-6 ml-auto">
        {/* Mobile Search & Mail Icons */}
        <button className="md:hidden">
          <Search className="w-5 h-5 text-gray-600" />
        </button>
        <button className="md:hidden">
          <Mail className="w-5 h-5 text-gray-600" />
        </button>

        {/* Bell Icon */}
        <button className="relative">
          <Bell className="w-5 h-5 text-gray-600" />
        </button>

        {/* User Dropdown */}
        <details className="dropdown">
          <summary className="btn m-1">
            <User className="w-5 h-5 text-gray-600" />
          </summary>
          <ul className="menu dropdown-content bg-white border border-gray-200 rounded-md shadow-lg w-32 right-0 p-2 absolute">
            {!SIGNIN ? (
              <>
                <li>
                  <a onClick={() => openModal("login")}>Log ind</a>
                </li>
                <li>
                  <a onClick={() => openModal("register")}>Registrér</a>
                </li>
              </>
            ) : (
              <li>
                <a onClick={() => LoggedIn(false)}>Log ud</a>
              </li>
            )}
          </ul>
        </details>
      </div>
    </div>
  );
}
